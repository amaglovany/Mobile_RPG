using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TDC;

[System.Serializable]
public class DataCharacter
{
    public GameObject leftFist;
    public GameObject rightFist;
    public Transform parentWeapon;
    public LocomotionPlayer locomotion;
    public HealthPlayer health;
    public Inventory inventory;

    public List<GameObject> listWeapon = new List<GameObject>();
    public WeaponData weaponData;
    public GameObject weaponGO;
}

public class SquadData : MonoBehaviourSingleton<SquadData>
{
    public List<DataCharacter> listCharacter = new List<DataCharacter>();
    public PlayerData playerData;

    public int currentIndex = 0;

	// Use this for initialization
	void Start ()
    {
        for (int i = 0; i < listCharacter.Count; i++)
        {
            listCharacter[i].locomotion.transform.SetParent(null);
            listCharacter[i].health.Start();
        }

        SwitchCharaster(0);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchCharaster(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchCharaster(1);
        }
    }

    public void SwitchCharaster(int index)
    {
        Vector3 savePosition = playerData.locomotion.transform.position;
        Quaternion saveRotation = playerData.locomotion.transform.rotation;

        listCharacter[currentIndex].listWeapon = new List<GameObject>(playerData.weaponsList);
        listCharacter[currentIndex].weaponData = playerData.currentWeaponData;
        listCharacter[currentIndex].weaponGO = playerData.currentWeaponGO;

        //--------------------

        playerData.leftHandFist = listCharacter[index].leftFist;
        playerData.rightHandFist = listCharacter[index].rightFist;
        playerData.weaponParent = listCharacter[index].parentWeapon;
        playerData.locomotion = listCharacter[index].locomotion;
        playerData.playerHealth = listCharacter[index].health;

        for (int i = 0; i < listCharacter.Count; i++)
        {
            if (i == index)
            {
                listCharacter[i].locomotion.gameObject.SetActive(true);

                listCharacter[i].locomotion.transform.position = savePosition;
                listCharacter[i].locomotion.transform.rotation = saveRotation;

                playerData.weaponsList = new List<GameObject>(listCharacter[i].listWeapon);
                playerData.currentWeaponData = listCharacter[i].weaponData;
                playerData.currentWeaponGO = listCharacter[i].weaponGO;
            }
            else
            {
                listCharacter[i].locomotion.gameObject.SetActive(false);
            }
        }

        if (InventoryUI.Instance) { InventoryUI.Instance.SwitchInventory(listCharacter[index].inventory); }

        GameplayUI.Instance.HealthBarValueChange(playerData.playerHealth.currentHealth);

        currentIndex = index;
        playerData.CalculateAttackSpeedWeapon();
    }

    public DataCharacter GetCurrentCharacter()
    {
        return listCharacter[currentIndex];
    }
}
