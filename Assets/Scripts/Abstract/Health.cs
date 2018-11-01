using System.Collections.Generic;
using UnityEngine;
using ch.sycoforge.Decal;

[System.Serializable]
public class PeriodicDamage
{
    public enum Type
    {
        Fire
    }

    public Type type;
    public float time;
    public float timeCur;

    public float iterationTime;
    public float iterationCurTime;
    public int damage;
}

public abstract class Health : MonoBehaviour
{
    [Tooltip("CStats reference")] private CharacterStats characterStats;

    public bool isDead;
    public bool isContantGround = false;

    [Tooltip("Locomotion reference")] public Locomotion locomotion;

    [Header("Health Manager")] public int maxHealth;

    public int minHealth;

    public int currentHealth { get; private set; }

    [Range(0f, 1f)] public float powerArmor = 0.5f;

    public List<PeriodicDamage> listPeriodicDamage = new List<PeriodicDamage>();

    [Header("Dissolve")] public List<SkinnedMeshRenderer> listDissolveTarget = new List<SkinnedMeshRenderer>();
    public Material dissolverMaterial;
    public bool enableDissolve = false;

    [Header("Blood")] public List<EasyDecal> listBloodDecal = new List<EasyDecal>();
    public GameObject floorBloodDecal;
    private int stepBloodDecal = 0;

    #region HealthManager

    public virtual void Start()
    {
        currentHealth = maxHealth;
        locomotion = GetComponent<Locomotion>();
        characterStats = GetComponent<CharacterStats>();

        for (int i = 0; i < listBloodDecal.Count; i++)
        {
            listBloodDecal[i].gameObject.SetActive(false);
        }
    }

    public virtual void Update()
    {
        for (int i = 0; i < listPeriodicDamage.Count; i++)
        {
            listPeriodicDamage[i].iterationCurTime += Time.deltaTime;
            listPeriodicDamage[i].timeCur += Time.deltaTime;

            if (listPeriodicDamage[i].iterationCurTime >= listPeriodicDamage[i].iterationTime)
            {
                Damage(listPeriodicDamage[i].damage, null, false);
                listPeriodicDamage[i].iterationCurTime = 0;
            }

            if (listPeriodicDamage[i].timeCur >= listPeriodicDamage[i].time)
            {
                listPeriodicDamage.RemoveAt(i);
                return;
            }
        }

        if (enableDissolve && isContantGround)
        {
            for (int i = 0; i < listDissolveTarget.Count; i++)
            {
                listDissolveTarget[i].material.SetFloat("_Height",
                    Mathf.Lerp(listDissolveTarget[i].material.GetFloat("_Height"), -2, 0.05f));
            }
        }
    }

    public virtual void Heal(int healValue)
    {
        if (isDead && healValue < 0)
        {
            return;
        }

        currentHealth += healValue;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public virtual void Damage(int damageValue, WeaponData weaponData, bool anim = true)
    {
        if (isDead)
        {
            return;
        }

        if (locomotion.typeLocomotion == Locomotion.TLocomotion.Attack &&
            locomotion.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.4f)
        {
            Debug.Log("Crytical damage ->>>>>>>>>>>>>>>>>>>>>> " + (int) (damageValue * 1.5f));
            currentHealth -= (int) (damageValue * 1.5f);
        }
        else
        {
            if (locomotion.typeLocomotion != Locomotion.TLocomotion.Block)
            {
                currentHealth -= ResultDamage(damageValue);
            }
            else
            {
                currentHealth -= Mathf.CeilToInt(ResultDamage(damageValue) * (1 - powerArmor));
            }
        }

        if (currentHealth <= minHealth)
        {
            Death();
        }
        else
        {
            if (anim == true)
            {
                locomotion.Damage();
                Invoke("HitBlood", 0.2f);
            }
        }

        Debug.Log($"{gameObject.name} took {ResultDamage(damageValue)} damage. Current HP: {currentHealth}");
        if (isDead)
        {
            Debug.Log($"{gameObject.name} died.");
        }

        if (weaponData && weaponData.weaponData.fireEffect)
        {
            DamageEffect(weaponData.weaponData.fireEffect);

            PeriodicDamage newDamage = new PeriodicDamage();
            newDamage.type = PeriodicDamage.Type.Fire;
            newDamage.time = 5f;
            newDamage.iterationTime = 1f;
            newDamage.damage = 5;

            listPeriodicDamage.Add(newDamage);
        }

        if (listBloodDecal.Count > 0)
        {
            listBloodDecal[stepBloodDecal].gameObject.SetActive(true);
            listBloodDecal[stepBloodDecal].Reset();

            stepBloodDecal++;

            if (stepBloodDecal >= listBloodDecal.Count)
            {
                stepBloodDecal = 0;
            }
        }
    }

    private void HitBlood()
    {
        if (floorBloodDecal)
        {
            Vector3 fixPosition = transform.position - transform.forward;
            fixPosition.x += Random.Range(-0.3f, 0.3f);
            fixPosition.z += Random.Range(-0.3f, 0.3f);

            Vector3 fixRandom = transform.eulerAngles;
            fixRandom.y = Random.Range(0, 360);
            Quaternion randomRotation = Quaternion.Euler(fixRandom);

            Instantiate(floorBloodDecal, fixPosition, randomRotation);
        }
    }

    private void DamageEffect(GameObject exampleParticle)
    {
        GameObject newEffect = Instantiate(exampleParticle, transform);

        Destroy(newEffect, 5f);
    }

    protected virtual int ResultDamage(int startDamage)
    {
        int resultDamage = startDamage;

        if (BackDamage())
        {
            resultDamage += startDamage * 25 / 100;
            Debug.Log("Back damage bonus: " + startDamage * 25 / 100);
        }

        return resultDamage;
    }

    private bool BackDamage()
    {
        Ray ray = new Ray(transform.position, -transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 3f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected virtual void Death()
    {
        locomotion.animator.SetTrigger("Death");
        locomotion.targetLocomotion = null;

        currentHealth = 0;
        PlayerData.Instance.inBattle = false;
        isDead = true;

        for (int i = 0; i < listDissolveTarget.Count; i++)
        {
            listDissolveTarget[i].material = dissolverMaterial;
        }

        enableDissolve = true;
    }

    #endregion
}