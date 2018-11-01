using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Base Abstract Class for All Slots
public abstract class Slot : MonoBehaviour
{
    public Item slotItem;

    public Image slotIcon;
    public Button slotButton;

    public virtual void FillSlot(Item newItem)
    {
        slotItem = newItem;
        slotIcon.sprite = newItem.itemSprite;
        slotIcon.enabled = true;
    }

    public virtual void ClearSlot()
    {
        slotItem = null;

        slotIcon.sprite = null;
        slotIcon.enabled = false;
    }

    public virtual void Select()
    {
    }
}