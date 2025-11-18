using Unity.VisualScripting;
using UnityEngine;

public class MagicalAttackBehaviour : StateMachineBehaviour
{
    private Player player;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(player == null)
        {
            player = animator.transform.parent.GetComponent<Player>();
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.StateMachine.ChangeState(player, Player.PlayerState.Normal);
    }

}
