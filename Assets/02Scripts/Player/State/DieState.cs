using UnityEngine;

public class DieState : IState<Player>
{
    public void Enter(Player owner)
    {
        owner.Animator.SetTrigger(AnimationParametaName.Die);
    }

    public void Update(Player owner, float deltaTime)
    {
        
    }

    public void Exit(Player owner)
    {
        owner.Animator.ResetTrigger(AnimationParametaName.Die);
    }

   
}
