using UnityEngine;

public class StrafeState : IState<Player>
{
    private float strafeAnimDirection;
    public void Enter(Player owner)
    {
        owner.Animator.SetBool(AnimationParametaName.HasShield, true);
    }

    public void Update(Player owner, float deltaTime)
    {
        if (InputManager.Instance.IsAltReleased)
        {
            owner.StateMachine.ChangeState(owner, Player.PlayerState.Normal);
        }
    }

    public void Exit(Player owner)
    {
        
    }

    private void Move(Player owner)
    {
        owner.CharacterController.Move(new Vector3(owner.MoveVectorMaker.MoveVector.x, 0f, 0f));
    }

    private void Animate(Player owner)
    {
        if(owner.MoveVectorMaker.MoveVector.x > 0f)
        {
            strafeAnimDirection = 1f;
        }

        else if(owner.MoveVectorMaker.MoveVector.x < 0f)
        {
            strafeAnimDirection = -1f;
        }

        else
        {
            strafeAnimDirection = 0f;
        }

       
    }

   
}
