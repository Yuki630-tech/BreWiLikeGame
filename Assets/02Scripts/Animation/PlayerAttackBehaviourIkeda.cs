using UnityEngine;

namespace Ikeda
{
    public class PlayerAttackBehaviourIkeda : StateMachineBehaviour
    {
        [Header("コンボ攻撃可能か"), SerializeField] private bool canComboAttack = true;

        [Header("次の攻撃への派生可能時間（0～1の割合）")]
        [SerializeField, Range(0, 1)]private float start = 0f;
        [SerializeField, Range(0, 1)]private float end = 1f;

        [Header("攻撃方向を向くタイミング")]
        [SerializeField, Range(0, 1)] private float rotateTime = 0f;

        private Player player;
        private AttackState attackState;

        private bool inputAttack = false;   //攻撃入力フラグ
        private bool nextAttack = false;    //次の攻撃を行うフラグ
        private bool isRotate = false;      //回転フラグ

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (player == null)
            {
                player = animator.GetComponent<Player>();
                if (player == null)
                {
                    Debug.LogError("プレイヤーねえよボケ");
                    return;
                }
                attackState = player.AttackState;
                if(attackState == null)
                {
                    Debug.LogError("AttackStateなんてねえよボケ");
                    return;
                }
            }

            inputAttack = false;
            nextAttack = false;
            isRotate = false;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //コンボ攻撃の実行フラグが立っていなければプレイヤーの攻撃終了処理
            if (nextAttack == false) attackState.EndAttack(player);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //攻撃方向
            if (stateInfo.normalizedTime >= rotateTime && isRotate == false)
            {
                attackState.SetAttackDirection(player);
                isRotate = true;
            }

            //コンボ攻撃
            if (canComboAttack)
            {
                //既に他のステートに遷移中ならreturn
                if (animator.IsInTransition(0)) { return; }

                //コンボ入力受付
                if (InputManager.Instance.IsAttackInput)
                {
                    inputAttack = true;
                }

                if (stateInfo.normalizedTime >= start && stateInfo.normalizedTime < end)
                {
                    if (inputAttack == true && nextAttack == false)
                    {
                        attackState.StartComboAttack(player);
                        nextAttack = true;
                    }
                }
            }
        }
    }
}