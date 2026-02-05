using Ikeda;
using Unity.VisualScripting;
using UnityEngine;

public class ComboAttackAfterJustAvoidBehaviour : PlayerAttackBehaviourIkeda
{
    [SerializeField] private float justAvoidTimescale = 0.25f;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        Time.timeScale = justAvoidTimescale;
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        if(!canComboAttack || !nextAttack)
        {
            Time.timeScale = 1f;
        }

        animator.SetBool(AnimationParametaName.JustAvoid, false);
    }
}
