using UnityEngine;

public class LocomotionPlayer : Locomotion
{
    [Space] public int specialPower;

    public override void AttackControl()
    {
        animator.SetTrigger(PlayerData.Instance.bareHands ? ATACK_HANDS_STATE : ATTACK_STATE);
    }

    public override void SpecialAttack()
    {
        if (specialPower < 100)
        {
            return;
        }

        animator.SetTrigger(ATTACK_STATE_SPECIAL);

        specialPower = 0;
    }

    public override void Attack()
    {
        if (typeLocomotion == TLocomotion.Damage)
        {
            return;
        }

        // When Player attacks
        PlayerData.Instance.SwitchWeaponColliders();
        DisableCrouch();
    }

    public void AttackRightFist()
    {
        if (typeLocomotion == TLocomotion.Damage)
        {
            return;
        }

        PlayerData.Instance.SwitchRightHandCollider();
        DisableCrouch();
    }

    public void AttackLeftFist()
    {
        if (typeLocomotion == TLocomotion.Damage)
        {
            return;
        }

        PlayerData.Instance.SwitchLeftHandCollider();
        DisableCrouch();
    }

    private void AttackKick()
    {
        if (typeLocomotion == TLocomotion.Damage)
        {
            return;
        }

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1f))
        {
            print(hit.transform.name);
            if (hit.transform.CompareTag("EnemyZombie"))
            {
                hit.transform.GetComponent<HealthEnemy>().Damage(5, null);
                hit.transform.GetComponent<LocomotionEnemy>().DisarmWeapon();
            }
        }

        DisableCrouch();
    }

    public void DisableWeapon()
    {
        
        PlayerData.Instance.DisableWeaponColliders();
    }

    public void AddSpecialPower()
    {
        int valuePower = 15;

        specialPower += valuePower;
        Debug.Log(gameObject.name + " added special power " + valuePower);
    }

    // ToDo: Squad Menu.
    private void OnMouseDown()
    {
        if (gameObject.CompareTag("Player"))
        {
            GameplayUI.Instance.CellWindowSquad(true);
            Debug.Log(gameObject.name);
        }
    }
}