using UnityEngine;

public class CounterBehaviour : StateMachineBehaviour
{
    IJustAvoidable justAvoidable;
    float speed = 500f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        justAvoidable = animator.GetComponent<IJustAvoidable>();

    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.position = Vector3.Lerp(animator.transform.position, justAvoidable.GetCounterTrans().position, speed * Time.deltaTime);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.transform.position = justAvoidable.GetCounterTrans().position;
    }
}
