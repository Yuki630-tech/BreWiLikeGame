using UnityEngine;

public class StrafeState : IState<Player>
{
    private float strafeAnimDirection;
    private Vector3 direction;
    private GameObject enemyObj;
    private Quaternion look;
    private float damp = 0.05f;
    private bool isSelected = false;
    public void Enter(Player owner)
    {
        owner.Animator.SetBool(AnimationParametaName.HasShield, true);
        isSelected = false;

    }

    public void Update(Player owner, float deltaTime)
    {
        if ((InputManager.Instance.IsSheildReleased || !InputManager.Instance.IsShieldPushing) && owner.IsCanChangeState)
        {
            owner.StateMachine.ChangeState(owner, Player.PlayerState.Normal);
        }

        if (owner.IsMovable)
        {
            Move(owner, deltaTime);
            Animate(owner, deltaTime);
        }
       
    }

    public void Exit(Player owner)
    {
        owner.Animator.SetBool(AnimationParametaName.HasShield, false);
        owner.Animator.ResetTrigger(AnimationParametaName.Jump);
    }

    private void Move(Player owner, float deltaTime)
    {
        owner.StrafeMoveVectorMaker.MakeMoveVector();

        if(owner.EnemyDetecter.EnemyInfoList.Count > 0)
        {
            owner.StrafeMoveVectorMaker.SetIfTurnToCamera(false);
            if (!isSelected)
            {
                enemyObj = owner.EnemyDetecter.GetEnemy();
                isSelected = true;
            }
            direction = (enemyObj.transform.position - owner.transform.position);
            direction.y = 0f;
            look = Quaternion.LookRotation(direction);
            owner.transform.rotation = look;
        }

        else
        {
            isSelected = false;
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
