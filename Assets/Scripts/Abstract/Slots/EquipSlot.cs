using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipSlot : Slot
{
    public override void Select()
    {
        base.Select();
        
        if (slotItem != null)
        {
            if (PlayerData.Instance.inventory.equipMode)
            {
                
            }
            else
            {
                //InventoryUI.Instance.currentEquipWeaponSlot = this;
                //InventoryUI.Instance.MakeSlotActive(this);
            }
        }
    }
}