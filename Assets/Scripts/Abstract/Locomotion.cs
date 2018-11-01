using UnityEngine;
using System.Collections.Generic;

public abstract class Locomotion : Characteristics
{
    public enum TLocomotion
    {
        Idle,
        Movement,
        Attack,
        Attack_Hands,
        Dead,
        Block,
        Damage
    }

    public enum TSpeed
    {
        Walk,
        Run
    }

    [Header("Locomotion")] [Space] public Animator animator;

    [Tooltip("Health reference")] public Health health;

    private Transform localTransform;
    private float speedFactor = 1f;

    public Locomotion targetLocomotion;

    public List<Transform> listSupport = new List<Transform>();

    public TLocomotion typeLocomotion = TLocomotion.Idle;
    public TSpeed typeSpeed = TSpeed.Walk;

    #region AnimationStates

    protected const string MOVEMENT_STATE = "Movement";
    protected const string MOVEMENT_X = "MovX";
    protected const string MOVEMENT_Y = "MovY";
    protected const string ATTACK_STATE = "Attack";
    protected const string BLOCK_STATE = "Block";
    protected const string ATACK_HANDS_STATE = "Attack_Hands";
    protected const string ATTACK_STATE_SPECIAL = "Attack_Special";

    #endregion

    #region Unity

    public virtual void Start()
    {
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        localTransform = transform;
    }

    public virtual void Update()
    {
        ControlState();
    }

    #endregion

    #region Locomotion

    public void Movement(Vector3 direction)
    {
        Vector3 inversDirection = localTransform.InverseTransformDirection(direction);

        switch (typeSpeed)
        {
            case TSpeed.Walk:
                speedFactor = Mathf.Lerp(speedFactor, 1, 0.1f);
                break;
            case TSpeed.Run:
                speedFactor = Mathf.Lerp(speedFactor, 2, 0.1f);
                break;
        }

        if (inversDirection.magnitude > 0)
        {
            animator.SetFloat(MOVEMENT_X,
                Mathf.Lerp(animator.GetFloat(MOVEMENT_X), inversDirection.x * speedFactor, 0.05f));
            animator.SetFloat(MOVEMENT_Y,
                Mathf.Lerp(animator.GetFloat(MOVEMENT_Y), inversDirection.z * speedFactor, 0.05f));
        }

        animator.SetBool(MOVEMENT_STATE, inversDirection.magnitude > 0);

        HeightControl();
    }

    public void Rotate(Vector3 movDirection)
    {
        if (typeLocomotion == TLocomotion.Dead)
        {
            return;
        }

        var lerpFactor = 0.1f;

        if (typeLocomotion == TLocomotion.Attack || typeLocomotion == TLocomotion.Attack_Hands)
        {
            lerpFactor /= 2;
        }

        Vector3 fixDir = movDirection;
        fixDir.y = 0;

        Quaternion fixRotation = Quaternion.Lerp(localTransform.rotation, Quaternion.LookRotation(fixDir, Vector3.up),
            lerpFactor);
        localTransform.rotation = fixRotation;
    }

    private float failTimer = 0;

    private void HeightControl()
    {
        if (listSupport.Count == 0)
        {
            return;
        }

        RaycastHit hit;
        int failCounter = 0;

        for (int i = 0; i < listSupport.Count; i++)
        {
            if (Physics.Raycast(listSupport[i].position, Vector3.down, out hit, 0.7f))
            {
            }
            else
            {
                failCounter++;
            }
        }

        if (failCounter >= listSupport.Count)
        {
            animator.applyRootMotion = false;
            animator.SetBool("Fail", true);
            failTimer += Time.deltaTime;
        }
        else if (failCounter == 0)
        {
            if (animator.GetBool("Fail") == true && failTimer >= 0.25f)
            {
                health.Damage(Mathf.CeilToInt(failTimer * 15), null, false);
            }

            animator.applyRootMotion = true;
            animator.SetBool("Fail", false);
        }
    }

    public virtual void Damage()
    {
        animator.SetTrigger("Damage");
    }

    public virtual void AttackControl()
    {
        // This meant to be overridden
    }

    public virtual void SpecialAttack()
    {
    }

    public virtual void RangeAttackControl()
    {
    }

    public virtual void RangeAttack()
    {
    }

    private void ControlState()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle"))
        {
            typeLocomotion = TLocomotion.Idle;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Movement"))
        {
            typeLocomotion = TLocomotion.Movement;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Damage"))
        {
            typeLocomotion = TLocomotion.Damage;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            typeLocomotion = TLocomotion.Attack;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Block"))
        {
            typeLocomotion = TLocomotion.Block;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack_Hands"))
        {
            typeLocomotion = TLocomotion.Attack_Hands;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Dead"))
        {
            typeLocomotion = TLocomotion.Dead;
        }

        if (targetLocomotion)
        {
            animator.SetBool("Dodge", true);
        }
        else
        {
            animator.SetBool("Dodge", false);
        }
    }

    public virtual void Attack()
    {
        // This meant to be overridden
    }

    public void SetIndexWeaponAnimator(int index)
    {
        animator.SetFloat("WeaponID", index);
    }

    public virtual void DisableCrouch()
    {
        animator.SetBool("Crouch", false);
    }

    public virtual void SwitchCrouch()
    {
        animator.SetBool("Crouch", !animator.GetBool("Crouch"));
    }

    #endregion
}