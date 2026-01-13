using UnityEngine;

public class CounterMoveBehaviour : StateMachineBehaviour
{
    IJustAvoidable justAvoidable;
    private Transform targetTrans;

    private float counterMoveSpeed = 10f;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        justAvoidable = animator.GetComponent<IJustAvoidable>();
        targetTrans = justAvoidable.GetTargetTrans();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(targetTrans != null)
        {
            animator.transform.position = Vector3.Lerp(animator.transform.position, targetTrans.position, counterMoveSpeed * Time.deltaTime);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.position = targetTrans.position;
    }
}
