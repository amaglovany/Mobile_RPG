using System.Collections.Generic;
using TDC;
using UnityEngine;

public class PlayerData : MonoBehaviourSingleton<PlayerData>
{
    [Header("Data")]
    public CameraControl cameraControl;
    public bool inBattle;
    public bool bareHands;

    public GameObject rightHandFist;
    public GameObject leftHandFist;

    private int bareHandsMinDamage = 14;
    private int bareHandsMaxDamage = 19;

    private int bareHandsDamage;

    public bool isPaused;
    public Locomotion locomotion;

    [HideInInspector] public Collider playerCollider;

    [Tooltip("Health reference")] [HideInInspector]
    public Health playerHealth;

    public Transform playerTransform;

    [Tooltip("User Interface reference")] [SerializeField]
    private GameplayUI _playerGameplayUI;

    public PlayerView playerView;

    public Inventory inventory;

    #region Unity

    private void Start()
    {
        playerHealth = locomotion.health;
        locomotion.transform.SetParent(null);
        localTransform = transform;

        CheckForEmptyHands();
    }

    private void Update()
    {
        if (stateLockTarget && target != null && !target.GetComponent<Health>().isDead)
        {
            cameraControl.target = target;
            locomotion.targetLocomotion = target.GetComponent<Locomotion>();
        }
        else
        {
            cameraControl.target = null;
            locomotion.targetLocomotion = null;
        }

        Locomotion();
        LockTargetControl();

        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            locomotion.SwitchCrouch();
        }
    }

    #endregion

    #region Locomotion

    [Header("Locomotion")] [SerializeField]
    private float movementSpeed = 1.2f;

    public bool stateLockTarget;
    public Transform target;
    public Vector3 movementDirection;
    private Transform localTransform;

    private void Locomotion()
    {
        if (playerHealth.isDead || isPaused)
        {
            return;
        }

        Vector3 keyboardDirection = Vector3.zero;

        keyboardDirection.x = Input.GetAxisRaw("Horizontal");
        keyboardDirection.z = Input.GetAxisRaw("Vertical");

        keyboardDirection += movementDirection;

        locomotion.Movement(cameraControl.parentCamera.TransformDirection(keyboardDirection));
        localTransform.position =
            Vector3.Lerp(localTransform.position, locomotion.animator.transform.position, movementSpeed);

        if (!target)
        {
            if (keyboardDirection.magnitude > 0)
            {
                locomotion.Rotate(cameraControl.parentCamera.TransformDirection(keyboardDirection));
            }
        }

        if (inBattle)
        {
            AudioManager.Instance.BattleMusicControl(true);
            _playerGameplayUI.SetPlayerBarsStatus(true);
        }
        else
        {
            AudioManager.Instance.BattleMusicControl(false);
            _playerGameplayUI.SetPlayerBarsStatus(false);
        }
    }

    #endregion

    #region LockTarget

    public void LockTarget()
    {
        stateLockTarget = !stateLockTarget;
    }

    private void LockTargetControl()
    {
        if (stateLockTarget)
        {
            if (target != null)
            {
                locomotion.Rotate((target.position - localTransform.position).normalized);
            }

            foreach (Transform item in playerView.listObject)
                if (item && !item.GetComponent<Health>().isDead)
                {
                    target = item;
                    locomotion.targetLocomotion = item.GetComponent<Locomotion>();
                    locomotion.typeSpeed = global::Locomotion.TSpeed.Walk;
                    return;
                }

            target = null;
            locomotion.typeSpeed = global::Locomotion.TSpeed.Run;
        }
        else
        {
            target = null;
            locomotion.typeSpeed = global::Locomotion.TSpeed.Run;
        }
    }

    #endregion

    #region Block

    public bool isBlock;

    public void Block(bool pointerDownValue)
    {
        isBlock = pointerDownValue;

        if (isBlock)
        {
            //GameplayUI.Instance.DisplayMessage(Messages.messageBlockTrue, Colors.greenMessage, 100f, false);

            // ToDO: Add Block animation. The Weapon will be used to block the attack. Spark animation if player and enemy weapons are both metal.
            locomotion.animator.SetBool("Block", true);
        }
        else
        {
            //GameplayUI.Instance.DisplayMessage(Messages.messageBlockFalse, Colors.redMessage, 1f, false);
            locomotion.animator.SetBool("Block", false);
        }
    }

    #endregion

    #region Weapon

    [Header("Weapons")] public int currentWeaponIndex;

    [SerializeField] public Transform weaponParent;
    [SerializeField] public List<GameObject> weaponsList = new List<GameObject>();
    [SerializeField] public WeaponData currentWeaponData;
    [SerializeField] public GameObject currentWeaponGO;

    [HideInInspector] public Collider[] currentWeaponColliders;

    public void AddToEquipSlot(GameObject weaponGO, int slotIndex)
    {
        // Add copy of Weapon into weaponsList
        weaponsList[slotIndex] = Instantiate(weaponGO, weaponParent);

        // Disable it instantly
        weaponsList[slotIndex].SetActive(false);

        // If hands are empty, equipping this Weapon
        if (currentWeaponGO == null || currentWeaponIndex == slotIndex)
        {
            DrawWeapon(slotIndex);
        }
    }

    public void RemoveFromEquipSlot(int slotIndex)
    {
        weaponsList[slotIndex] = null;

        if (GetWeaponsEquippedCount() < 1)
        {
            Destroy(currentWeaponGO);
            currentWeaponData = null;
            currentWeaponGO = null;
        }

        SwitchWeapon();
    }

    public void SwitchWeapon()
    {
        #region If Player have only 1 weapon equipped, method won't execute

        if (GetWeaponsEquippedCount() < 2)
            return;

        #endregion

        int prevIndex = currentWeaponIndex;

        currentWeaponIndex++;

        if (currentWeaponIndex >= weaponsList.Count)
        {
            currentWeaponIndex = 0;
        }

        if (prevIndex != currentWeaponIndex)
        {
            DrawWeapon(currentWeaponIndex);
        }

        CalculateAttackSpeedWeapon();
    }

    private void DrawWeapon(int index)
    {
        // Disabling "Previous", if it was, and Enabling "New"
        if (currentWeaponGO != null)
        {       
            currentWeaponGO.SetActive(false);
        }

        // Assign new currentWeapon Index
        currentWeaponIndex = index;

        // Assign current weaponGameObject to selected one
        currentWeaponGO = weaponsList[currentWeaponIndex];
        // Doing the same with WeaponData
        currentWeaponData = currentWeaponGO.GetComponent<WeaponData>();

        // Instantly enabling Weapon object
        currentWeaponGO.SetActive(true);

        // Disabling it's colliders, to prevent alwaysDamage on Enemies
        DisableCurrentWeaponColliders();

        // Setting number to current index in UI and Play short sound
        GameplayUI.Instance.SetWeaponNumberText(currentWeaponIndex + 1);
        AudioManager.Instance.WeaponChangeSound();

        CalculateAttackSpeedWeapon();
    }

    private int GetWeaponsEquippedCount()
    {
        int listCount = 0;
        foreach (var element in weaponsList)
        {
            if (element != null)
            {
                listCount++;
            }
        }

        return listCount;
    }

    public void CalculateAttackSpeedWeapon()
    {
        if (!locomotion) { return; }

        if(!currentWeaponData)
        {
            locomotion.animator.SetFloat("Attack_Speed", 1f);
            return;
        }

        switch(currentWeaponData.weaponData.Speed)
        {
            case Weapon.SpeedEnum.Fast: locomotion.animator.SetFloat("Attack_Speed", 1.2f); break;
            case Weapon.SpeedEnum.Normal: locomotion.animator.SetFloat("Attack_Speed", 1f); break;
            case Weapon.SpeedEnum.Slow: locomotion.animator.SetFloat("Attack_Speed", 0.8f); break;
        }
    }

    public void CheckForEmptyHands()
    {
        if (GetWeaponsEquippedCount() == 0)
        {
            GameplayUI.Instance.SwitchWeaponUI(false);
            bareHands = true;
            DisableCurrentWeaponColliders();
        }
        else
        {
            for (int i = 0; i < weaponsList.Count; i++)
            {
                if (weaponsList[i] != null && GetWeaponsEquippedCount() < 2)
                {
                    DrawWeapon(i);
                    break;
                }
            }

            GameplayUI.Instance.SwitchWeaponUI(true);
            bareHands = false;
        }
    }

    private Collider[] GetCurrentWeaponColliders()
    {
        return currentWeaponColliders = currentWeaponGO.GetComponentsInChildren<Collider>();
    }

    private void DisableCurrentWeaponColliders()
    {
        if (bareHands)
        {
            foreach (var coll in rightHandFist.GetComponentsInChildren<Collider>())
            {
                coll.enabled = false;
            }

            foreach (var coll in leftHandFist.GetComponentsInChildren<Collider>())
            {
                coll.enabled = false;
            }
        }
        else
        {
            foreach (var coll in GetCurrentWeaponColliders())
            {
                coll.enabled = false;
            }
        }
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

    public void SwitchWeaponColliders()
    {
        foreach (Collider weaponCollider in GetCurrentWeaponColliders())
        {
            weaponCollider.enabled = !weaponCollider.enabled;
        }

        currentWeaponData.PlaySound();
    }

    public void DisableWeaponColliders()
    {
        foreach (Collider weaponCollider in GetCurrentWeaponColliders())
        {
            weaponCollider.enabled = false;
        }
    }

    public GameObject GetCurrentWeaponGO()
    {
        return currentWeaponGO;
    }

    private int GetBareHandsDamage()
    {
        return Random.Range(bareHandsMinDamage, bareHandsMaxDamage + 1);
    }

    public int GetCurrentWeaponDamage()
    {
        return bareHands ? GetBareHandsDamage() : currentWeaponData.weaponData.GetDamage();
    }

    #endregion
}