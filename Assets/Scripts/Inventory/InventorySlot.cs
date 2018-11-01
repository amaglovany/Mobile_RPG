using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : GeneralSlot
{
    public override void FillSlot(Item newItem)
    {
        base.FillSlot(newItem);
    }

    public override void ClearSlot()
    {
        base.ClearSlot();
    }

    // Modify for all selectable Items
    public override void Select()
    {
        if (slotItem != null)
        {
            InventoryUI.Instance.currentInventorySlot = this;
            InventoryUI.Instance.MakeSlotActive(this);
        }
        else
        {
            InventoryUI.Instance.currentInventorySlot = null;
            InventoryUI.Instance.MakeAllSlotsInactive();
        }
    }
}