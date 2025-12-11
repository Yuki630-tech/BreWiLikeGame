using UnityEngine;

public class SwordGoblinEnemy : PatrolEnemyBase
{
    [Header("攻撃に関する設定")]
    [Tooltip("AttackReceiverDetecter"), SerializeField] private AttackReceiverDetecter attackReceiverDetecter;

    [Header("対峙に関する設定")]
    [Tooltip("プレイヤーの右方向にどれだけ離れた位置を目的地とするか"), SerializeField] private float rightOffsetFromPlayerToStrafeDes;
    public AttackReceiverDetecter AttackReceiverDetecter { get => attackReceiverDetecter; }
    public float RightOffsetFromPlayerToStrafeDes { get => rightOffsetFromPlayerToStrafeDes; }

    protected override void Awake()
    {
        base.Awake();
        stateMachine.AddState(EnemyState.Strafe, new GoblinStrafeState());

        enemyAttackStrategyFactory.AddStrategy(new EnemyCloseRangeAttack());

    }


}
