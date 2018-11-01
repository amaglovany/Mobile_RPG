using UnityEngine;

public class HealthPlayer : Health
{
    [SerializeField] private Transform hitTransform;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Heal(int healValue)
    {
        base.Heal(healValue);

        // Custom implementation
        GameplayUI.Instance.HealthBarValueChange(currentHealth);
    }

    public override void Damage(int damageValue, WeaponData weaponData, bool anim = true)
    {
        if (!CheatManager.Instance.GOD_MODE)
        {
            base.Damage(damageValue, weaponData, anim);

            // Custom implementation
            GameplayUI.Instance.HealthBarValueChange(currentHealth);
            GameplayUI.Instance.ActivatePlayerHitScreenEffect();

            if (locomotion.typeLocomotion != Locomotion.TLocomotion.Block)
            {
                EffectsManager.Instance.ActivateBloodEffect(hitTransform);
            }
            else
            {
                EffectsManager.Instance.ActivateHitBlockEffect(hitTransform);
            }
        }

        SquadData.Instance.GetCurrentCharacter().locomotion.AddSpecialPower();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Fists") && !isDead)
        {
            Damage(5, null);

            if (locomotion.typeLocomotion != Locomotion.TLocomotion.Block)
            {
                EffectsManager.Instance.ActivateBloodEffect(collider.transform);
            }
            else
            {
                EffectsManager.Instance.ActivateHitBlockEffect(collider.transform);
            }
        }
    }

    protected override void Death()
    {
        base.Death();

        // Custom implementation
        GameplayUI.Instance.ShowDeathScreen();
    }
}