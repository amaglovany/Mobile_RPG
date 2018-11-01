using UnityEngine;

public class LocomotionEnemy : Locomotion
{
    private AIBattle aiBattle;
    private HealthEnemy healthEnemy;

    private void Awake()
    {
        aiBattle = GetComponent<AIBattle>();
        healthEnemy = GetComponent<HealthEnemy>();
    }

    public override void AttackControl()
    {
        if (healthEnemy.enemyWeaponData)
        {
            animator.SetTrigger(ATTACK_STATE);
            animator.ResetTrigger("Attack_Gun");
        }
        else
        {
            print("Enemy Attack Hands");
            animator.SetTrigger(ATACK_HANDS_STATE);
        }
    }

    public override void RangeAttackControl()
    {
        base.RangeAttackControl();

        if (healthEnemy.enemyWeaponData && healthEnemy.enemyWeaponData.weaponData.rangeAttack)
        {
            animator.SetTrigger("Attack_Gun");
            animator.ResetTrigger(ATTACK_STATE);
        }
    }

    public override void Attack()
    {
        if (typeLocomotion == TLocomotion.Damage)
        {
            if(!healthEnemy.enemyWeaponData)
            {
                aiBattle.OffHandsCollider();
            }
            return;
        }

        base.Attack();

        // When Enemy attacks
        if (targetLocomotion && !targetLocomotion.health.isDead)
        {
            WeaponData data = healthEnemy.enemyWeaponData;

            if (data)
            {
                targetLocomotion.health.Damage(data.weaponData.GetDamage(), data);
            }
            else
            {
                targetLocomotion.health.Damage(15, null);
            }
        }

        if (healthEnemy.enemyWeaponData) { healthEnemy.enemyWeaponData.PlaySound(); }
    }

    public void AttackRightFist()
    {
        if (typeLocomotion == TLocomotion.Damage) { return; }

        aiBattle.SwitchRightHandCollider();
    }

    public void AttackLeftFist()
    {
        if (typeLocomotion == TLocomotion.Damage) { return; }

        aiBattle.SwitchLeftHandCollider();
    }

    public void AttackKick()
    {

    }

    public override void RangeAttack()
    {
        base.RangeAttack();

        if (targetLocomotion && !targetLocomotion.health.isDead)
        {
            WeaponData data = healthEnemy.enemyWeaponData;

            if (data)
            {
                targetLocomotion.health.Damage(data.weaponData.GetDamage(), data);
            }
        }
    }

    public void DisarmWeapon()
    {
        print("Disarm");
        if (!healthEnemy.enemyWeaponData) { return; }
        if (typeLocomotion != TLocomotion.Block) { return; }

        Destroy(healthEnemy.enemyWeaponData.gameObject);
        healthEnemy.enemyWeaponData = null;
        healthEnemy.locomotion.animator.SetBool("Block", false);
    }
}