using UnityEngine;
using UnityEngine.Rendering;

public class DodgeAnimationBehaviour : StateMachineBehaviour
{
    Player player;
    bool isAttack;

    [Tooltip("攻撃入力を受け付けるアニメーションの最小進行度"), Range(0, 1), SerializeField] private float attackStart;
    [Tooltip("攻撃入力を受け付けるアニメーションの最大進行度"), Range(0, 1), SerializeField] private float attackEnd;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isAttack = false;
        player = animator.GetComponent<Player>();
        player.IsCanChangeState = false;
        player.SetIfMovable(false);
        if (player.IsJustAvoidable)
        {
            Time.timeScale = 0.5f;
            animator.SetBool("JustAvoid", true);
        }

    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (InputManager.Instance.IsAttackInput)
        {
            isAttack = true;
        }

        if(isAttack && stateInfo.normalizedTime >= attackStart && stateInfo.normalizedTime <= attackEnd)
        {
            animator.SetTrigger(AnimationParametaName.PhysicalAttackTrigger);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.IsCanChangeState = true;
        player.SetIfMovable(true);
        animator.ResetTrigger(AnimationParametaName.Jump);
        Time.timeScale = 1f;
        
    }
}
