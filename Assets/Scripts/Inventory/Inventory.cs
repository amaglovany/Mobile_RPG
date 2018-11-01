using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public delegate void OnInventoryChange();

    public OnInventoryChange onInventoryChangeCallback;

    public delegate void OnEquipmentChange();

    public OnEquipmentChange onEquipmentChangeCallback;

    public int inventoryCapacity = 27;
    public List<Item> items = new List<Item>();

    public Item firstIndexWeapon;
    public Item secondIndexWeapon;

    #region Gold

    private int gold;

    public void AddGold(int count)
    {
        gold += count;

        onInventoryChangeCallback?.Invoke();
    }

    public void RemoveGold(int count)
    {
        int goldValueAfterRemove = gold - count;

        if (goldValueAfterRemove < 0)
        {
            GameplayUI.Instance.DisplayMessage("Not enough gold", Colors.redMessage, 2f, false);
        }
        else
        {
            gold = goldValueAfterRemove;
            onInventoryChangeCallback?.Invoke();
        }
    }

    public int GetGoldCount()
    {
        return gold;
    }

    #endregion

    #region ItemsInGeneral

    public void AddItem(Item item)
    {
        items.Add(item);

        onInventoryChangeCallback?.Invoke();
    }

    public void DestroyItem(int itemSlotIndex)
    {
        items.RemoveAt(itemSlotIndex);

        onInventoryChangeCallback?.Invoke();
    }

    public bool IsFull()
    {
        if (items.Count >= inventoryCapacity)
        {
            GameplayUI.Instance.DisplayMessage("Inventory is full.", Colors.redMessage, 2f, false);
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

    #region Equipment

    public bool equipMode;

    public void RefreshEquip()
    {
        if (firstIndexWeapon)
        {
            InventoryUI.Instance.equipWeaponSlots[0].slotItem = firstIndexWeapon;

            InventoryUI.Instance.equipWeaponSlots[0].FillSlot(firstIndexWeapon);

            onEquipmentChangeCallback?.Invoke();
        }
        else
        {
            InventoryUI.Instance.equipWeaponSlots[0].ClearSlot();
            onEquipmentChangeCallback?.Invoke();
        }

        if (secondIndexWeapon)
        {
            InventoryUI.Instance.equipWeaponSlots[1].slotItem = secondIndexWeapon;
            InventoryUI.Instance.equipWeaponSlots[1].FillSlot(secondIndexWeapon);
            onEquipmentChangeCallback?.Invoke();
        }
        else
        {
            InventoryUI.Instance.equipWeaponSlots[1].ClearSlot();
            onEquipmentChangeCallback?.Invoke();
        }
    }

    public void EquipWeapon(int weaponSlotIndex, int equipSlotIndex)
    {
        var weaponToEquip = items[weaponSlotIndex] as Weapon;

        if(equipSlotIndex == 0)
        {
            firstIndexWeapon = weaponToEquip;
        }
        else if(equipSlotIndex == 1)
        {
            secondIndexWeapon = weaponToEquip;
        }

        if (weaponToEquip != null)
        {
            InventoryUI.Instance.equipWeaponSlots[equipSlotIndex].slotItem = weaponToEquip;

            DestroyItem(weaponSlotIndex);

            // If we need to swap - SwapSlots()

            onInventoryChangeCallback?.Invoke();

            PlayerData.Instance.AddToEquipSlot(weaponToEquip.itemActivePrefab, equipSlotIndex);
            onEquipmentChangeCallback?.Invoke();
            AudioManager.Instance.WeaponChangeSound();
        }
    }

    public void SwapWeapons(int inventorySlot_Index, int equipWeaponSlot_Index)
    {
        // Caching weapon which was in slot
        var weaponToUnEquip =
            InventoryUI.Instance.equipWeaponSlots[equipWeaponSlot_Index].slotItem;

        // Caching weapon which will be in slot
        var weaponToEquip = (Weapon) items[inventorySlot_Index];

        items.RemoveAt(inventorySlot_Index);
        items.Add(weaponToUnEquip);

        InventoryUI.Instance.equipWeaponSlots[equipWeaponSlot_Index].FillSlot(weaponToEquip);

        PlayerData.Instance.AddToEquipSlot(weaponToEquip.itemActivePrefab, equipWeaponSlot_Index);

        AudioManager.Instance.WeaponChangeSound();

        onEquipmentChangeCallback?.Invoke();
        onInventoryChangeCallback?.Invoke();
    }

    public void UnEquipWeapon(int slotIndex)
    {
        if (IsFull())
        {
            return;
        }

        if (slotIndex == 0)
        {
            firstIndexWeapon = null;
        }
        else if (slotIndex == 1)
        {
            secondIndexWeapon = null;
        }

        var weaponToUnEquip = InventoryUI.Instance.equipWeaponSlots[slotIndex].slotItem;
        InventoryUI.Instance.equipWeaponSlots[slotIndex].slotItem = null;
        PlayerData.Instance.RemoveFromEquipSlot(slotIndex);
        AddItem(weaponToUnEquip);
        InventoryUI.Instance.equipWeaponSlots[slotIndex].ClearSlot();

        onEquipmentChangeCallback?.Invoke();
    }

    #endregion

    #region DropItems

    public List<Item> droppedItems = new List<Item>();

    public void AddToDropList(int itemSlotIndex)
    {
        if (droppedItems.Count == 10)
        {
            GameplayUI.Instance.DisplayMessage("You can drop only 10 items at once", Colors.redMessage, 2f, false);
            return;
        }

        droppedItems.Add(items[itemSlotIndex]);
        items.RemoveAt(itemSlotIndex);
        InventoryUI.Instance.SelectNextSlot(true);


        onInventoryChangeCallback?.Invoke();
    }

    public void GenerateIfDrop()
    {
        if (droppedItems.Count > 0)
        {
            ContainerGenerator.Instance.GenerateAndFillContainer(ContainerGenerator.Instance.containerJunkPrefab,
                PlayerData.Instance.transform, ContainerTypeEnum.Enum.Junk, droppedItems);
            droppedItems.Clear();
        }
    }

    #endregion
}