using UnityEngine;

public class MoveState : IState<Player>
{
    private bool isGround;
    private float animationDamp = 0.01f;
    private float animSpeed;
    public void Enter(Player owner)
    {
        owner.Animator.SetBool(AnimationParametaName.HasShield, false);
    }

    public void Update(Player owner, float deltaTime)
    {
        owner.NormalMoveCharConMove.Update(deltaTime);
        owner.SetPlayerSpeed(owner.MoveVectorMaker.Speed);
        if (InputManager.Instance.IsSheildInput && owner.IsMovable)
        {
            owner.StateMachine.ChangeState(owner, Player.PlayerState.Strafe);
        }

        if(InputManager.Instance.WasDashInputThisFrame)
        {
            owner.WeaponContainer.StopToUseWeapon(WeaponContainer.WeaponKind.Shield);
            owner.WeaponContainer.StopToUseWeapon(WeaponContainer.WeaponKind.Sword);
            
        }
        isGround = owner.NormalMoveCharConMove.IsGround;
        //Debug.Log("CanAttack:" + canAttack);
        if (InputManager.Instance.IsAttackInput && isGround && owner.IsMovable)
        {
            owner.StateMachine.ChangeState(owner, Player.PlayerState.Attack);
        }

        Animate(owner, deltaTime);
    }

    public void Exit(Player owner)
    {

    }

    private void Animate(Player owner, float deltaTime)
    {
        if(owner.IsMovable)
        {
            animSpeed = InputManager.Instance.IsDashInput ? 2f * owner.MoveVectorMaker.InputVector.magnitude : 1f * owner.MoveVectorMaker.InputVector.magnitude;
            //animSpeed = owner.MoveVectorMaker.MoveVector.magnitude;

        }
        owner.Animator.SetFloat(AnimationParametaName.Move, animSpeed, animationDamp, deltaTime);
        owner.Animator.SetFloat(AnimationParametaName.FallSpeed, owner.VerticalMoveMaker.VerticalSpeed, animationDamp, Time.deltaTime);
        owner.Animator.SetBool(AnimationParametaName.IsGround, isGround);
        //float animationSpeedForBlentTree = owner.IsMovable ? 1f : 0f;
        //owner.Animator.SetFloat(AnimationParametaName.AnimationSpeedForBlendTree, animationSpeedForBlentTree);

    }
}
