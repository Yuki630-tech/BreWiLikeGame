using UnityEngine;

public class EnemyCloseRangeAttack : IStrategyWithCondition
{
    
    public bool CanStartStrategy(EnemyBase owner)
    {
        SwordGoblinEnemy goblin = owner as SwordGoblinEnemy;
        return goblin.AttackReceiverDetecter.JustAvoidable != null;
    }

    public void Enter(EnemyBase owner)
    {
        owner.Animator.SetTrigger(AnimationParametaName.CloseAttack);
    }

    public void Update(EnemyBase owner, float deltaTime)
    {
        
    }

    public void Exit(EnemyBase owner)
    {
        owner.Animator.ResetTrigger(AnimationParametaName.CloseAttack);
    }

    public string GetName()
    {
        return "ãﬂê⁄çUåÇ";
    }
}
