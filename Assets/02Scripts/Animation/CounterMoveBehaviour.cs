using Unity.Cinemachine;
using UnityEngine;

public class CounterMoveBehaviour : StateMachineBehaviour
{
    IJustAvoidable justAvoidable;
    private Transform targetTrans;
    private Transform targetEnemyTrans;
    private Vector3 direction;
    private Quaternion look;
    [Tooltip("ターゲットに向くスピード"), SerializeField] private float rotSpeed = 1080f;

    private float counterMoveSpeed = 10f;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        justAvoidable = animator.GetComponent<IJustAvoidable>();
        targetTrans = justAvoidable.GetTargetTrans();
        targetEnemyTrans = justAvoidable.GetEnemyTrans();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(targetTrans != null)
        {
            direction = (targetEnemyTrans.position - animator.transform.position).normalized;
            look = Quaternion.LookRotation(direction);
            animator.transform.position = Vector3.MoveTowards(animator.transform.position, targetTrans.position, counterMoveSpeed * Time.deltaTime);
            animator.transform.rotation = Quaternion.RotateTowards(animator.transform.rotation, look, rotSpeed * Time.deltaTime);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.position = targetTrans.position;
    }
}
