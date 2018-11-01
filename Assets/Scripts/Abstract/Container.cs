using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Container : Interactable
{
    protected uint containerCapacity = 10; // Value for all containers in game (should be good)

    public ContainerTypeEnum.Enum containerType;

    public List<Item> containerItems = new List<Item>();

    protected void OpenContainer()
    {
        GameplayUI.Instance.ContainerOpen();
        ContainerUI.Instance.InitializeContainerUI(this);
    }

    public void MoveAllItemsToInventory()
    {
        for (int i = containerItems.Count - 1; i > -1; i--)
        {
            MoveItemToInventory(i);
        }
    }

    public virtual bool MoveItemToInventory(int itemIndex)
    {
        if (containerItems[itemIndex] is Gold)
        {
            var gold = (Gold) containerItems[itemIndex];
            int goldAmount = gold.GetGoldAmount();

            PlayerData.Instance.inventory.AddGold(goldAmount);
        }
        else
        {
            if (PlayerData.Instance.inventory.IsFull())
            {
                return false;
            }

            PlayerData.Instance.inventory.AddItem(containerItems[itemIndex]);
        }

        ContainerUI.Instance.SelectNextSlot(false); // If any but first slot was active 
        containerItems.RemoveAt(itemIndex);
        ContainerUI.Instance.UpdateContainerSlots(this);
        ContainerUI.Instance.SelectNextSlot(true); // If first slot was active 
        IfContainerEmpty();
        return true;
    }

    protected virtual void IfContainerEmpty()
    {
        GameplayUI.Instance.ContainerClose();
    }
}