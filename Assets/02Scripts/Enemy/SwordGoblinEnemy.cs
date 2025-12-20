using UnityEngine;

public class SwordGoblinEnemy : PatrolEnemyBase
{
    [Header("攻撃に関する設定")]
    [Tooltip("AttackReceiverDetecter"), SerializeField] private AttackReceiverDetector attackReceiverDetecter;

    [Header("対峙に関する設定")]
    [Tooltip("プレイヤーの右方向にどれだけ離れた位置を目的地とするか"), SerializeField] private float rightOffsetFromPlayerToStrafeDes;
    [Tooltip("対峙中の移動スピード"), SerializeField] private float strafeSpeed = 1.5f;
    public AttackReceiverDetector AttackReceiverDetecter { get => attackReceiverDetecter; }
    public float RightOffsetFromPlayerToStrafeDes { get => rightOffsetFromPlayerToStrafeDes; }
    public float StrafeSpeed { get => strafeSpeed; }

    protected override void Awake()
    {
        base.Awake();
        stateMachine.AddState(EnemyState.Strafe, new GoblinStrafeState());

        enemyAttackStrategyFactory.AddStrategy(new EnemyCloseRangeAttack());

    }


}
