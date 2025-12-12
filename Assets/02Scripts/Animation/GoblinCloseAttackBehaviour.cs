using Unity.VisualScripting;
using UnityEngine;

public class GoblinCloseAttackBehaviour : StateMachineBehaviour
{
    private SwordGoblinEnemy goblin;
    private IJustAvoidable justAvoidable;
    [Tooltip("プレイヤーのジャスト回避を受け入れるスタート地点"), Range(0f, 1f), SerializeField] private float justAvoidableStart = 0f;
    [Tooltip("プレイヤーのジャスト回避を受け入れるゴール地点"), Range(0f, 1f), SerializeField] private float justAvoidableEnd = 0.45f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (goblin == null)
        {
            goblin = animator.GetComponent<SwordGoblinEnemy>();
            justAvoidable = goblin.AttackReceiverDetecter.JustAvoidable;
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(justAvoidable != null)
        {
            if (stateInfo.normalizedTime >= justAvoidableStart && stateInfo.normalizedTime <= justAvoidableEnd)
            {
                justAvoidable.SetIfJustAvoidable(true);
            }

            else
            {
                justAvoidable.SetIfJustAvoidable(false);
            }
        }
    }


    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        goblin.StateMachine.ChangeState(goblin, EnemyBase.EnemyState.Strafe);
    }

}
