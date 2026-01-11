using Ikeda;
using UnityEngine;
using UniRx;
using System.Linq;
using System.Runtime.CompilerServices;

public class Player : MonoBehaviour, IJustAvoidable, IJustGurdable
{

    [Header("カメラに関する設定")]
    [Tooltip("PlayerCamera"), SerializeField] private PlayerCamera playerCamera;

    [Header("プレイヤーの各コンポーネント")]
    [Tooltip("CharacterController"), SerializeField] private CharacterController characterController;
    [Tooltip("Animator"), SerializeField] private Animator animator;

    [Header("移動に関する設定")]
    [Tooltip("移動ベクトル作成用のコンポーネント"), SerializeField] private MoveVectorMaker moveVectorMaker = new MoveVectorMaker();
    [Tooltip("ジャンプ力"), SerializeField] private float jumpPower;
    //[Tooltip("歩行アニメーションの1倍速時の速度"), SerializeField] private float walkAnimSpeed = 1f;
    //[Tooltip("ダッシュアニメーションの1倍速時の速度"), SerializeField] private float dashAnimSpeed = 1.6f;
    private CharConMove normalMoveCharConMove; //CharacterControllerを使って通常移動するためのクラス

    [Header("Strafeに関する設定")]
    [Tooltip("Strafe時の移動ベクトル作成クラス"), SerializeField] private MoveVectorMaker strafeMoveVectorMaker = new MoveVectorMaker();
    [Tooltip("ロックオン対象となる敵を検知するコンポーネント"), SerializeField] private EnemyDetecter enemyDetecter;

    [Header("攻撃ステートに関する設定")]
    [Tooltip("攻撃可能になるゲームフラグの値"), SerializeField] private int canAttackFlag = 1;
    [Tooltip("武器を管理するコンポーネント"), SerializeField] private WeaponContainer weaponContainer;
    private WeaponAttackStrategyFactory<Player> weaponAttackStrategyFactory;

    [Header("漸滅スキルに関する設定")]
    //[Tooltip("斬撃発生器"), SerializeField] private SlashSpawner slashSpawner;
    //private AttributeSkillStrategyFactory<Player> attributeStrategyFactory;


    [Header("重力に関する設定")]
    [Tooltip("重力をかけるコンポーネント"), SerializeField] private VerticalMoveMaker verticalMoveMaker = new VerticalMoveMaker();

    [Header("接地判定に関する設定")]
    [Tooltip("接地判定コンポーネント"), SerializeField] private GroundChecker groundChecker;

    [Header("移動できる状態かどうか"), SerializeField] private bool isMovable;

    [Header("ジャスト回避できるか"), ReadOnly, SerializeField] private bool isJustAvoidable;

    [Header("ジャストガードできるか"), ReadOnly, SerializeField] private bool isJustGurdable;

    [Header("プレイヤーの今の速度"), ReadOnly, SerializeField] private ReactiveProperty<float> playerSpeedProperty = new();

    public bool IsCanChangeState = true;

    //[Tooltip("攻撃中のWallChecker"), SerializeField] private WallChecker wallChecker;

    //[Tooltip("攻撃中のAnimator"), SerializeField] private PlayerAnimator playerAnimator;

    private bool isDie;

    private StateMachine<PlayerState, Player> stateMachine = new StateMachine<PlayerState, Player>();

    public IReadOnlyReactiveProperty<float> PlayerSpeedProperty => playerSpeedProperty;

   
    #region Property
    public AttackState AttackState { get; private set; }
    public CharacterController CharacterController { get => characterController; }
    public Animator Animator { get => animator; }
    public MoveVectorMaker MoveVectorMaker { get => moveVectorMaker; }
    public CharConMove NormalMoveCharConMove { get => normalMoveCharConMove; }
    public WeaponContainer WeaponContainer { get => weaponContainer; }
    public WeaponAttackStrategyFactory<Player> WeaponAttackStrategyFactory { get => weaponAttackStrategyFactory; }
    //public AttributeSkillStrategyFactory<Player> AttributeStrategyFactory { get => attributeStrategyFactory; set => attributeStrategyFactory = value; }
    public VerticalMoveMaker VerticalMoveMaker { get => verticalMoveMaker; }
    public GroundChecker GroundChecker { get => groundChecker; }
    public bool IsMovable { get => isMovable; }
    public StateMachine<PlayerState, Player> StateMachine { get => stateMachine; }
    //public SlashSpawner SlashSpawner { get => slashSpawner; }
    //public WallChecker WallChecker { get => wallChecker; }
    //public PlayerAnimator PlayerAnimator { get => playerAnimator; }
    public int CanAttackFlag { get => canAttackFlag; }
    public MoveVectorMaker StrafeMoveVectorMaker { get => strafeMoveVectorMaker; }
    public EnemyDetecter EnemyDetecter { get => enemyDetecter;}
    public PlayerCamera PlayerCamera { get => playerCamera; }
    //public float WalkAnimSpeed { get => walkAnimSpeed; }
    //public float DashAnimSpeed { get => dashAnimSpeed; }
    public bool IsJustAvoidable { get => isJustAvoidable; }
    public bool IsJustGurdable { get => isJustGurdable; }

    public float NoticedByEnemySpeed { get; private set; }
    
    #endregion
    public enum PlayerState
    {
        Normal,
        Attack,
        Strafe,
        Die
    }

    private void Awake()
    {
        NoticedByEnemySpeed = moveVectorMaker.MoveSpeed;
        ComponentProvider.Instance.SetPlayerTrans(transform);
        ComponentProvider.Instance.SetPlayer(this);
        //playerAnimator.enabled = false;

        //ステートマシンに移動ステート、攻撃ステートを追加
        normalMoveCharConMove = new CharConMove(transform, characterController, moveVectorMaker, verticalMoveMaker, groundChecker, jumpPower);
        SetIfMovable(true);

        stateMachine.AddState(PlayerState.Normal, new MoveState());
        stateMachine.AddState(PlayerState.Attack, AttackState = new AttackState());
        stateMachine.AddState(PlayerState.Strafe, new StrafeState());
        stateMachine.AddState(PlayerState.Die, new DieState());

        weaponAttackStrategyFactory = new WeaponAttackStrategyFactory<Player>();
        weaponAttackStrategyFactory.AddStrategy(Weapon.WeaponType.Physical, new PhysicalAttack());
        weaponAttackStrategyFactory.CreateStrategy(this, Weapon.WeaponType.Physical);

        //移動ステートに変更
        stateMachine.ChangeState(this, PlayerState.Normal);
        //Debug.Log(AttackState != null);

        //GameManager.Instance?.OnGamePauseObservable.Subscribe(_ => SetIfMovable(false)).AddTo(gameObject);
        //GameManager.Instance?.OnGameUnPauseObservable.Subscribe((_ => SetIfMovable(true))).AddTo(gameObject);
        //GameManager.Instance?.OnDieObservable.Where(_ => !isDie).Subscribe(_ =>
        //{
        //    stateMachine.ChangeState(this, PlayerState.Die);
        //    isDie = true;
        //}).AddTo(gameObject);

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update(Time.deltaTime, this);
    }

    public void SetIfMovable(bool setMovable)
    {
        isMovable = setMovable;
        normalMoveCharConMove.SetIfMovable(setMovable);
    }

    public void SetIfJustAvoidable(bool value)
    {
        isJustAvoidable = value;
    }

    public void SetIfJustGurdable(bool value)
    {
        isJustAvoidable = value;
    }

    public void SetPlayerSpeed(float value)
    {
        playerSpeedProperty.Value = value;
    }
}
