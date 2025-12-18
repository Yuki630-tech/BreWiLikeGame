using Ikeda;
using Unity.VisualScripting;
using UnityEngine;

public class ComboAttackAfterJustAvoidBehaviour : PlayerAttackBehaviourIkeda
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        Time.timeScale = 0.5f;
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        if(!canComboAttack || !nextAttack)
        {
            Time.timeScale = 1f;
        }
    }
}
