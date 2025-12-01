using System;
using UniRx;
using UnityEngine;

public class StrafeState : IState<Player>
{
    private float strafeAnimDirection;
    private Vector3 direction;
    private ReactiveProperty<GameObject> enemyObj = new();
    private Quaternion look;
    private float damp = 0.05f;
    private bool isSelected = false;
    CompositeDisposable disposables = new();
    public void Enter(Player owner)
    {
        if (owner.EnemyDetecter.TargetEnemy != null)
        {
            owner.PlayerCamera.SetCamera(false, PlayerCamera.CameraKind.TargetGroup);
            owner.PlayerCamera.SetSecondTarget(owner.EnemyDetecter.transform);
            _ = owner.PlayerCamera.LookAt(PlayerCamera.CameraKind.TargetGroup, owner.transform, owner.EnemyDetecter.TargetEnemy.transform, owner.EnemyDetecter.CameraRotSpeed);
        }

        else
        {
            owner.PlayerCamera.SetCamera(true, PlayerCamera.CameraKind.Player);
        }
            owner.Animator.SetBool(AnimationParametaName.HasShield, true);
        isSelected = false;
        owner.WeaponContainer.StartToUseWeapon(WeaponContainer.WeaponKind.Shield);
        //ターゲットとなる敵が近くに一人もいなければ通常のプレイヤーカメラに切り替える
        enemyObj.Where(x => x == null).Subscribe(__ =>
        {
            Debug.Log("敵がいなくなりました");
            owner.PlayerCamera.SetCamera(true, PlayerCamera.CameraKind.Player);
            isSelected = false;
        }).AddTo(disposables);
        //ターゲットとなる敵がいない状態から初めて敵を見つけたら敵とプレイヤー両方を映すカメラに切り替える
        enemyObj.Where(x => x != null).Subscribe(x =>
        {
            owner.PlayerCamera.SetCamera(false, PlayerCamera.CameraKind.TargetGroup);
            owner.PlayerCamera.SetSecondTarget(x.transform);
            isSelected = true;
            _ = owner.PlayerCamera.LookAt(PlayerCamera.CameraKind.TargetGroup, owner.transform, x.transform, owner.EnemyDetecter.CameraRotSpeed);

        }).AddTo(disposables);

        //最初から2体以上いた場合上のx==1のイベントは発生しないため、2体以上の敵がいたらむりやりTargetGroupカメラに移行させるようにする
        if (owner.EnemyDetecter.EnemyCount.Value >= 2)
        {
            owner.PlayerCamera.SetCamera(false, PlayerCamera.CameraKind.TargetGroup);
            owner.PlayerCamera.SetSecondTarget(owner.EnemyDetecter.TargetEnemy.transform);
            _ = owner.PlayerCamera.LookAt(PlayerCamera.CameraKind.TargetGroup, owner.transform, owner.EnemyDetecter.TargetEnemy.transform, owner.EnemyDetecter.CameraRotSpeed);
        }

    }

    public void Update(Player owner, float deltaTime)
    {
        if ((InputManager.Instance.IsSheildReleased || !InputManager.Instance.IsShieldPushing) && owner.IsCanChangeState)
        {
            //owner.EnemyDetecter.ChangeEnemy();
            owner.StateMachine.ChangeState(owner, Player.PlayerState.Normal);
            owner.Animator.SetBool(AnimationParametaName.HasShield, false);
            owner.PlayerCamera.SetCamera(true, PlayerCamera.CameraKind.Player);
        }

        if (owner.IsMovable)
        {
            Move(owner, deltaTime);
            Animate(owner, deltaTime);
        }

        if (InputManager.Instance.IsAttackInput && owner.IsCanChangeState)
        {
            owner.StateMachine.ChangeState(owner, Player.PlayerState.Attack);
        }

        enemyObj.Value = owner.EnemyDetecter.TargetEnemy;

    }

    public void Exit(Player owner)
    {
        owner.Animator.ResetTrigger(AnimationParametaName.Jump);
        if (InputManager.Instance.IsDashInput)
        {
            owner.WeaponContainer.StopToUseWeapon(WeaponContainer.WeaponKind.Shield);
            owner.WeaponContainer.StopToUseWeapon(WeaponContainer.WeaponKind.Sword);
        }
        if (disposables.Count > 0)
        {
            disposables.Dispose();

        }
    }

    private void Move(Player owner, float deltaTime)
    {
        owner.StrafeMoveVectorMaker.MakeMoveVector();

        if (enemyObj.Value != null)
        {
            owner.StrafeMoveVectorMaker.SetIfTurnToCamera(false);
            direction = (enemyObj.Value.transform.position - owner.transform.position);
            direction.y = 0f;
            look = Quaternion.LookRotation(direction);
            owner.transform.rotation = look;
        }

        else
        {
            owner.StrafeMoveVectorMaker.SetIfTurnToCamera(true);
        }

        owner.CharacterController.Move((owner.transform.right * owner.StrafeMoveVectorMaker.MoveVector.x + owner.transform.forward * owner.StrafeMoveVectorMaker.MoveVector.z) * deltaTime);
    }

    private void Animate(Player owner, float deltaTime)
    {
        if (InputManager.Instance.IsJumpInput)
        {
            owner.Animator.SetTrigger(AnimationParametaName.Jump);
        }

        owner.Animator.SetFloat(AnimationParametaName.ShieldMoveX, owner.StrafeMoveVectorMaker.MoveVector.x, damp, deltaTime);
        owner.Animator.SetFloat(AnimationParametaName.ShieldMoveZ, owner.StrafeMoveVectorMaker.MoveVector.z, damp, deltaTime);
    }


}
