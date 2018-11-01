using TDC;
using UnityEngine;
using UnityEngine.AI;

public class AIBattle : MonoBehaviour
{
    public NavMeshAgent agent;
    private bool chasedRecently;

    public EnemyUI enemyUI;

    public bool isBattle;
    public Locomotion locomotion;
    public Transform target;
    public CoreTrigger viewTrigger;
    private HealthEnemy healthEnemy;

    public GameObject rightHandFist;
    public GameObject leftHandFist;

    public float maxDistanceAttack = 0;
    public float delayAttack = 1.2f;
    private float currentTimerAttack = 0;

    public float intervalBlock = 0f;
    private float currentIntervalBlock = 0;
    private float optionDistance;

    #region Unity

    private void Start()
    {
        locomotion = GetComponent<Locomotion>();
        locomotion.animator.SetBool("EnemyZombie", true);
        SetRagdoll(false);
        enemyUI = GetComponent<EnemyUI>();
        healthEnemy = GetComponent<HealthEnemy>();
    }

    private void Update()
    {
        ViewControl();

        if(target && !target.gameObject.activeSelf)
        {
            target = PlayerData.Instance.locomotion.transform;
        }

        if (target && !target.gameObject.GetComponent<Health>().isDead)
        {
            PlayerData.Instance.inBattle = true;
            chasedRecently = true;

            enemyUI.SetHealthBarStatus(true);
            agent.SetDestination(target.position);
            agent.isStopped = false;
            isBattle = true;
        }
        else
        {
            if (chasedRecently)
            {
                PlayerData.Instance.inBattle = false;
                chasedRecently = false;
            }

            enemyUI.SetHealthBarStatus(false);
            agent.isStopped = true;
            isBattle = false;
        }

        Locomotion();
    }

    #endregion

    #region Core

    private void Locomotion()
    {
        if (!target)
        {
            // ToDo: Patrolling fix related to this.
            locomotion.Movement(Vector3.zero);
            return;
        }

        Vector3 fixDirection = Vector3.zero;
        fixDirection = (agent.steeringTarget - transform.position).normalized;
        locomotion.Rotate(fixDirection);

        if (Vector3.Distance(target.position, transform.position) >= Mathf.Clamp(maxDistanceAttack + (agent.stoppingDistance - optionDistance), 0, 10))
        {
            if (healthEnemy.awakeWeapon && healthEnemy.awakeWeapon.rangeAttack)
            {
                currentTimerAttack += Time.deltaTime;
                locomotion.targetLocomotion = target.GetComponent<Locomotion>();

                if(currentTimerAttack >= delayAttack)
                {
                    locomotion.RangeAttackControl();
                    currentTimerAttack = 0;
                    print("Range attack");
                }
            }
            else
            {
                locomotion.targetLocomotion = null;
                currentTimerAttack = delayAttack;
            }
        }
        else if (target.GetComponent<Health>().currentHealth > 0)
        {
            optionDistance = 0;
            locomotion.targetLocomotion = target.GetComponent<Locomotion>();

            if (healthEnemy.enemyWeaponData)
            {
                if (locomotion.typeLocomotion != global::Locomotion.TLocomotion.Attack)
                {
                    if (currentIntervalBlock > 0)
                    {
                        currentIntervalBlock -= Time.deltaTime;
                        locomotion.animator.SetBool("Block", true);
                    }
                    else
                    {
                        currentTimerAttack += Time.deltaTime;
                        locomotion.animator.SetBool("Block", false);
                    }
                }
            }
            else
            {
                optionDistance = 1f;
                currentTimerAttack += Time.deltaTime / 3;
                locomotion.animator.SetBool("Block", false);
            }

            if (currentTimerAttack >= delayAttack)
            {
                locomotion.AttackControl();

                currentTimerAttack = 0;

                if(Random.Range(0, 101) >= 80)
                {
                    currentIntervalBlock = Random.Range(2, 4);
                }
            }
        }

        locomotion.Movement(fixDirection);
    }

    private void ViewControl()
    {
        foreach (Transform tar in viewTrigger.listObject)
            if (tar)
            {
                target = tar;
                locomotion.targetLocomotion = target.GetComponent<Locomotion>();
                return;
            }

        locomotion.targetLocomotion = null;
        target = null;
    }

    private void SetKinematic(bool newValue)
    {
        var bodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in bodies) rb.isKinematic = newValue;
    }

    public void SetRagdoll(bool value)
    {
        SetKinematic(!value);
        GetComponent<Animator>().enabled = !value;

    }

    public void SwitchRightHandCollider()
    {
        foreach (var coll in rightHandFist.GetComponentsInChildren<Collider>())
        {
            coll.enabled = !coll.enabled;
        }
    }

    public void SwitchLeftHandCollider()
    {
        foreach (var coll in leftHandFist.GetComponentsInChildren<Collider>())
        {
            coll.enabled = !coll.enabled;
        }
    }

    public void OffHandsCollider()
    {
        foreach (var coll in leftHandFist.GetComponentsInChildren<Collider>())
        {
            coll.enabled = false;
        }

        foreach (var coll in rightHandFist.GetComponentsInChildren<Collider>())
        {
            coll.enabled = false;
        }
    }

    #endregion
}