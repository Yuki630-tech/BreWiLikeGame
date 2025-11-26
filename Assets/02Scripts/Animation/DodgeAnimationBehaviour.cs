using UnityEngine;

public class DodgeAnimationBehaviour : StateMachineBehaviour
{
    Player player;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = animator.GetComponent<Player>();
        player.IsCanChangeState = false;
        player.SetIfMovable(false);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.IsCanChangeState = true;
        player.SetIfMovable(true);
        animator.ResetTrigger(AnimationParametaName.Jump);
    }
}
