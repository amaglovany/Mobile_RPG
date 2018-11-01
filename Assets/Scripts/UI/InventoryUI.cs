using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TDC;

public class InventoryUI : MonoBehaviour
{
    #region Singleton

    public static InventoryUI Instance;

    public enum TCategory
    {
        Weapon,
        Other
    }

    public TCategory typeCategory = TCategory.Weapon;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    [SerializeField] private Text goldCountText;

    public Transform itemsContainer;
    public InventorySlot[] inventorySlots;
    [HideInInspector] public EquipWeaponSlot[] equipWeaponSlots;

    public InventorySlot currentInventorySlot;
    public EquipWeaponSlot currentEquipWeaponSlot;

    #region UI_Update_Logic

    public void SwitchInventory(Inventory newInventory)
    {
        PlayerData.Instance.inventory = newInventory;
        PlayerData.Instance.inventory.onInventoryChangeCallback = null;
        PlayerData.Instance.inventory.onInventoryChangeCallback += UpdateInventoryUI;
        PlayerData.Instance.inventory.onEquipmentChangeCallback += PlayerData.Instance.CheckForEmptyHands;
        inventorySlots = itemsContainer.GetComponentsInChildren<InventorySlot>();
        equipWeaponSlots = gameObject.GetComponentsInChildren<EquipWeaponSlot>();

        PlayerData.Instance.inventory.RefreshEquip();
    }

    public void SwitchCategory(int index)
    {
        switch (index)
        {
            case 0: typeCategory = TCategory.Weapon; break;
            case 1: typeCategory = TCategory.Other; break;
        }


        UpdateInventoryUI();
    }

    private void UpdateInventoryUI()
    {
        goldCountText.text = PlayerData.Instance.inventory.GetGoldCount().ToString();
        SelectNextSlot(false);

        int indexCounter = 0;

        for (var i = 0; i < inventorySlots.Length; i++)
        {
            if (i < PlayerData.Instance.inventory.items.Count)
            {
                if (PlayerData.Instance.inventory.items[i].type == typeCategory)
                {
                    inventorySlots[i].ClearSlot();
                    inventorySlots[indexCounter].FillSlot(PlayerData.Instance.inventory.items[i]);
                    indexCounter++;
                }
                else
                {
                    inventorySlots[i].ClearSlot();
                }
            }
            else
            {
                inventorySlots[i].ClearSlot();
            }
        }

        SelectNextSlot(true);
    }

    private void OnEnable()
    {
        if (!PlayerData.Instance) { return; }
        if (!PlayerData.Instance.inventory) { return; }

        UpdateInventoryUI();
        StopWeaponEquipment();
    }

    private void SelectFirstSlot()
    {
        inventorySlots[0].Select();
    }

    public void SelectNextSlot(bool isFirstSlotActive)
    {
        if (isFirstSlotActive)
        {
            foreach (InventorySlot slot in inventorySlots)
            {
                if (slot.slotButton.GetComponent<Image>().color == Colors.playerActiveUI)
                {
                    return;
                }
            }

            SelectFirstSlot();
        }
        else
        {
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (inventorySlots[i] == currentInventorySlot)
                {
                    inventorySlots[i].ClearSlot();
                    MakeAllSlotsInactive();

                    if (i > 0)
                    {
                        inventorySlots[i - 1].Select();
                    }
                }
            }
        }
    }

    public void MakeSlotActive(Slot clickedSlot)
    {
        MakeAllSlotsInactive();
        clickedSlot.slotButton.GetComponent<Image>().color = Colors.playerActiveUI;

        if (clickedSlot.slotItem is ItemStack)
        {
            ((InventorySlot) clickedSlot).countText.color = Color.white;
        }

        ActivateItemInfo(true, clickedSlot.slotItem);

        // ToDo: Improve [Begin]
        if (clickedSlot is EquipSlot)
        {
            //useButton.GetComponentInChildren<Text>().text = "UnEquip";
            useButton.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(false);
            unEquipButton.gameObject.SetActive(true);

            DropButtonsVisibility(false);
        }

        if (clickedSlot is InventorySlot)
        {
            DropButtonsVisibility(true);

            if (clickedSlot.slotItem is Weapon || clickedSlot.slotItem is ItemConsumable)
            {
                //useButton.GetComponentInChildren<Text>().text = "Equip";
                useButton.gameObject.SetActive(false);
                equipButton.gameObject.SetActive(true);
                unEquipButton.gameObject.SetActive(false);
            }
            else
            {
                //useButton.GetComponentInChildren<Text>().text = "Use";
                useButton.gameObject.SetActive(true);
                equipButton.gameObject.SetActive(false);
                unEquipButton.gameObject.SetActive(false);
            }
        }

        // ToDo Improve [End]
    }

    public void MakeAllSlotsInactive()
    {
        foreach (InventorySlot inventorySlot in inventorySlots)
        {
            inventorySlot.slotButton.GetComponent<Image>().color = Colors.playerDefaultUI;
            inventorySlot.countText.color = Color.black;
        }

        foreach (var equipSlot in equipWeaponSlots)
        {
            equipSlot.GetComponent<Image>().color = Colors.playerDefaultUI;
        }

        ActivateItemInfo(false, null);
    }

    private void DropButtonsVisibility(bool value)
    {
        dropButton.gameObject.SetActive(value);
        destroyButton.gameObject.SetActive(value);
    }

    #endregion

    #region EquipZone

    [Header("Control Area")]
    [SerializeField] private Button useButton;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button unEquipButton;

    [SerializeField] private Button dropButton;
    [SerializeField] private Button destroyButton;

    [Header("Equip Area")] [SerializeField]
    private Animator equipAnimator;

    [SerializeField] private Text hintSelectEquipText;
    [SerializeField] private Image backgroundOnEquipImage;
    [SerializeField] private Button weaponOneButton;
    [SerializeField] private Button weaponTwoButton;

    private void StartWeaponEquipment()
    {
        hintSelectEquipText.gameObject.SetActive(true);
        backgroundOnEquipImage.gameObject.SetActive(true);
        PlayerData.Instance.inventory.equipMode = true;
        equipAnimator.Play("Glow");
    }

    public void StopWeaponEquipment()
    {
        hintSelectEquipText.gameObject.SetActive(false);
        backgroundOnEquipImage.gameObject.SetActive(false);
        PlayerData.Instance.inventory.equipMode = false;
        equipAnimator.Play("Default");
    }

    public void EquipSlotOnClick(EquipWeaponSlot equipWeaponSlot)
    {
        // Behaviour - If we are equipping weapon
        if (PlayerData.Instance.inventory.equipMode && equipWeaponSlot.slotItem == null)
        {
            equipWeaponSlot.FillSlot(inventorySlots[GetCurrentInventorySlotIndex()].slotItem);
            PlayerData.Instance.inventory.EquipWeapon(GetCurrentInventorySlotIndex(), equipWeaponSlot.equipWeaponSlotIndex);

            StopWeaponEquipment();
        }
        // Behaviour - If we want to replace weapon by another
        else if (PlayerData.Instance.inventory.equipMode && equipWeaponSlot.slotItem != null)
        {
            PlayerData.Instance.inventory.SwapWeapons(GetCurrentInventorySlotIndex(), equipWeaponSlot.equipWeaponSlotIndex);
            //equipWeaponSlot.FillSlot(inventorySlots[GetCurrentInventorySlotIndex()].slotItem);

            StopWeaponEquipment();
        }

        currentInventorySlot = null;
    }

    #endregion

    #region InfoWindow

    [Header("Info Area")] [SerializeField] private Text infoItemDamage;
    [SerializeField] private Text infoItemDescription;
    [SerializeField] private Text infoItemName;
    [SerializeField] private Image infoItemSprite;
    [SerializeField] private Text infoItemTypes;

    private void ActivateItemInfo(bool value, Item item)
    {
        useButton.gameObject.SetActive(value);

        if (value)
        {
            if (item != null)
            {
                if (item is Weapon)
                {
                    var weapon = (Weapon) item;
                    FillInfoWindowWithWeapon(weapon.itemSprite, weapon.itemName, weapon.minDamage,
                        weapon.maxDamage, weapon.DamageType, weapon.Speed, weapon.Range,
                        weapon.itemDescription);
                }
            }
        }
        else
        {
            ClearInfoWindow();
        }
    }

    private void FillInfoWindowWithWeapon(Sprite sprite, string name, uint minDamage, uint maxDamage,
        Weapon.DamageTypeEnum damageType, Weapon.SpeedEnum speed, Weapon.RangeEnum range,
        string description)
    {
        infoItemSprite.sprite = sprite;
        infoItemName.text = name;
        infoItemDamage.text = "<color=#AA3232FF>" + minDamage + " - " + maxDamage + "</color> DMG (" + damageType + ")";
        infoItemTypes.text = speed + " speed, " + range + " range";
        infoItemDescription.text = description;
    }

    private void ClearInfoWindow()
    {
        infoItemSprite.sprite = null;
        infoItemName.text = string.Empty;
        infoItemDamage.text = string.Empty;
        infoItemTypes.text = string.Empty;
        infoItemDescription.text = string.Empty;
    }

    #endregion

    #region ButtonClicks

    public void SelectItem(InventorySlot slot)
    {
        slot.Select();
    }

    public void UseItem()
    {
        if (currentInventorySlot != null)
        {
            if (inventorySlots[GetCurrentInventorySlotIndex()].slotItem is Weapon)
            {
                StartWeaponEquipment();
                return;
            }
            else if (inventorySlots[GetCurrentInventorySlotIndex()].slotItem is ItemConsumable)
            {
                // ToDo: Potion Equip mode.
                return;
            }
        }
        // If we selected EquipSlot - then UnEquip
        else if (currentEquipWeaponSlot != null)
        {
            PlayerData.Instance.inventory.UnEquipWeapon(currentEquipWeaponSlot.equipWeaponSlotIndex);
        }
    }

    public void DropItem()
    {
        if (currentInventorySlot != null)
        {
            PlayerData.Instance.inventory.AddToDropList(GetCurrentInventorySlotIndex());
        }
    }

    public void DestroyItem()
    {
        if (currentInventorySlot != null)
        {
            PlayerData.Instance.inventory.DestroyItem(GetCurrentInventorySlotIndex());
        }
    }

    public void InventoryCloseClick()
    {
        PlayerData.Instance.inventory.GenerateIfDrop();
        GameplayUI.Instance.InventoryClose();
    }

    private int GetCurrentInventorySlotIndex()
    {
        int slotIndex = 0;
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (currentInventorySlot == inventorySlots[i])
            {
                slotIndex = i;
            }
        }

        return slotIndex;
    }

    #endregion
}