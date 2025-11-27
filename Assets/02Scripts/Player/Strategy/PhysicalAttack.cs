using UnityEngine;

public class PhysicalAttack : IStrategy<Player>
{
    Vector3 attackDirection;
    float rotSpeed;
    public virtual void Enter(Player owner)
    {
        owner.Animator.SetTrigger(AnimationParametaName.PhysicalAttackTrigger);
        owner.Animator.SetFloat("Move", 0f);
        owner.WeaponContainer.StartToUseWeapon(WeaponContainer.WeaponKind.Sword);
    }

    public void Update(Player owner, float deltaTime)
    {
        if (owner.GroundChecker.IsGround)
        {
            //Vector3 newPos = player.transform.position + groundChecker.GroundOffset;
            //player.transform.position = newPos;
        }

        if (attackDirection.magnitude >= 0.1f)
        {
            Quaternion look = Quaternion.LookRotation(attackDirection);
            owner.transform.rotation = Quaternion.RotateTowards(owner.transform.rotation, look, rotSpeed * Time.deltaTime);
        }
    }
    public void Exit(Player owner)
    {
        //UŒ‚‚ÉŒü‚­•ûŒü‚ğƒŠƒZƒbƒg
        attackDirection = Vector3.zero;

        owner.Animator.ResetTrigger(AnimationParametaName.PhysicalAttackTrigger);
    }

   
}
