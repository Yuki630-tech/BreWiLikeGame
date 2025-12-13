using System;
using UniRx;
using UnityEngine;

public class StrafeState : IState<Player>
{
    private Vector3 direction;
    private Quaternion look;
    private float damp = 0.05f;
    //private bool isSelected = false;
    CompositeDisposable disposables;
    public void Enter(Player owner)
    {
        disposables = new();
        if(owner.EnemyDetecter.TargetEnemy.Value != null)
        {
            owner.PlayerCamera.SetCamera(false, PlayerCamera.CameraKind.TargetGroup);
            owner.PlayerCamera.SetSecondTarget(owner.EnemyDetecter.TargetEnemy.Value.transform);
            _ = owner.PlayerCamera.LookAt(PlayerCamera.CameraKind.TargetGroup, owner.transform, owner.EnemyDetecter.TargetEnemy.Value.transform, owner.PlayerCamera.RotSpeed);
            TargetMarkerSpawner markerSpwner = owner.EnemyDetecter.TargetEnemy.Value.GetComponent<TargetMarkerSpawner>();
            markerSpwner.SetTarget(true);
        }
        owner.Animator.SetBool(AnimationParametaName.HasShield, true);
        owner.WeaponContainer.StartToUseWeapon(WeaponContainer.WeaponKind.Shield);
        
    }

    public void Update(Player owner, float deltaTime)
    {
        if ((InputManager.Instance.IsSheildReleased || !InputManager.Instance.IsShieldPushing) && owner.IsCanChangeState)
        {
            //owner.EnemyDetecter.ChangeEnemy();
            owner.StateMachine.ChangeState(owner, Player.PlayerState.Normal);
            owner.Animator.SetBool(AnimationParametaName.HasShield, false);
            //owner.PlayerCamera.SetCamera(true, PlayerCamera.CameraKind.Player);
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
        owner.PlayerCamera.SetCamera(true, PlayerCamera.CameraKind.Player);
        if(owner.EnemyDetecter.TargetEnemy.Value != null)
        {
            TargetMarkerSpawner markerSpwner = owner.EnemyDetecter.TargetEnemy.Value.GetComponent<TargetMarkerSpawner>();
            markerSpwner?.SetTarget(false);
        }

    }

    private void Move(Player owner, float deltaTime)
    {
        owner.StrafeMoveVectorMaker.MakeMoveVector();

        if (owner.EnemyDetecter.TargetEnemy.Value != null)
        {
            owner.StrafeMoveVectorMaker.SetIfTurnToCamera(false);
            direction = (owner.EnemyDetecter.TargetEnemy.Value.transform.position - owner.transform.position).normalized;
            direction.y = 0f;
            look = Quaternion.LookRotation(direction);
            owner.transform.rotation = look;
        }

        else
        {
            owner.StrafeMoveVectorMaker.SetIfTurnToCamera(true);
        }

        owner.VerticalMoveMaker.Update(deltaTime);

        owner.CharacterController.Move((owner.transform.right * owner.StrafeMoveVectorMaker.MoveVector.x + owner.VerticalMoveMaker.FallVector.y * owner.transform.up + owner.transform.forward * owner.StrafeMoveVectorMaker.MoveVector.z) * deltaTime);
        owner.SetPlayerSpeed(owner.StrafeMoveVectorMaker.Speed);
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
