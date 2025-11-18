using UnityEngine;

public class OneHitAttackBehaviour : StateMachineBehaviour
{
    Player player;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(player == null)
        {
            player = animator.GetComponentInParent<Player>();
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
