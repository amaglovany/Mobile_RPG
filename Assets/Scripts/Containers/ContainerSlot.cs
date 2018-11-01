using UnityEngine;
using UnityEngine.UI;

public class ContainerSlot : GeneralSlot
{
    public void FillSlot(Item newItem)
    {
        base.FillSlot(newItem);

        // Amount check. If Item is ItemStack - displays count in cell
        var stackItem = newItem as ItemStack;
        if (stackItem != null)
        {
            int amount = stackItem.GetAmount();

            if (amount > 1) // To display only multiple Items count
            {
                countText.text = amount.ToString();
            }
        }
    }

    public void ClearSlot()
    {
        base.ClearSlot();

        countText.text = null;
    }

    public void Select()
    {
        if (slotItem != null)
        {
            ContainerUI.Instance.currentSelectedSlot = this;
            ContainerUI.Instance.MakeSlotActive(this);
        }
        else
        {
            ContainerUI.Instance.currentSelectedSlot = null;
            ContainerUI.Instance.MakeAllSlotsInactive();
        }
    }
}