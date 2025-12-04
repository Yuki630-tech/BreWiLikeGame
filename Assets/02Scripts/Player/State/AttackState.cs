using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

namespace Ikeda
{
    /// <summary>
    /// 攻撃状態を実行するクラス
    /// </summary>
    public sealed class AttackState : IState<Player>
    {

        public bool IsAttacking { get; private set; }

        private Vector3 attackDirection;    //プレイヤーの攻撃方向
        //private float rotSpeed = 1080f;     //プレイヤーの回転速度

        public void Enter(Player owner)
        {
            IsAttacking = true;
            StartAttack(owner);
        }
        public void Update(Player owner, float deltaTime)
        {
            owner.WeaponAttackStrategyFactory.GetStrategy().Update(owner, deltaTime);
            //if(attackDirection.magnitude > 0)
            //{
            //    var look = Quaternion.LookRotation(attackDirection);
            //    owner.transform.rotation = Quaternion.RotateTowards(owner.transform.rotation, look, rotSpeed * deltaTime);

            //}
            if (!IsAttacking)
            {
                if (InputManager.Instance.IsShieldPushing)
                {
                    owner.StateMachine.ChangeState(owner, Player.PlayerState.Strafe);
                }

                else
                {
                    owner.StateMachine.ChangeState(owner, Player.PlayerState.Normal);
                }
            }
            
        }
        public void Exit(Player owner)
        {
            //Debug.Log("終了→攻撃処理");
            attackDirection = Vector3.zero;
            //owner.PlayerAnimator.enabled = false;
        }

        /// <summary>
        /// 攻撃開始処理
        /// </summary>
        public void StartAttack(Player owner)
        {

            IsAttacking = true;
            owner.WeaponAttackStrategyFactory.GetStrategy().Enter(owner);

        }

        public void EndAttack(Player owner)
        {
            IsAttacking = false;

            owner.WeaponAttackStrategyFactory.GetStrategy().Exit(owner);
        }

        /// <summary>
        /// 地面に接地しているかつ入力値があった場合TRUE
        /// </summary>
        /// <returns></returns>
        public bool CheckCanAttack(Player owner)
        {
            if (!owner.GroundChecker.IsGround) { return false; }
            if (owner.VerticalMoveMaker.VerticalSpeed < 0f) { return false; }
            if (InputManager.Instance.IsAttackInput)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// コンボ攻撃
        /// </summary>
        /// <returns></returns>
        public bool CheckComboAttack(Player owner)
        {
            return CheckCanAttack(owner);
        }

        /// <summary>
        /// コンボ開始処理
        /// ここではフラグを二重に立てない
        /// </summary>
        public void StartComboAttack(Player owner)
        {
            owner.WeaponAttackStrategyFactory.GetStrategy().Enter(owner);
        }

        public void SetAttackDirection(Player owner)
        {
            //攻撃方向を取得・正規化し、攻撃方向をカメラへと合わせる
            Vector3 moveInput = owner.MoveVectorMaker.MoveVector;
            moveInput = moveInput.normalized;

            Quaternion cameraRot = Quaternion.Euler(new Vector3(0f, Camera.main.transform.localEulerAngles.y, 0f));
            attackDirection = cameraRot * moveInput;
        }
    }
}
