using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCorpse : Container
{
    public GameObject associatedEnemyWeapon;

    protected override void Interact()
    {
        if (!canInteract || containerItems.Count < 1)
        {
            print("Dont step");
            return;
        }

        base.Interact();

        // Custom Implementation
        OpenContainer();
        print("Done");
    }

    protected override void IfContainerEmpty()
    {
        if (containerItems.Count < 1)
        {
            base.IfContainerEmpty();

            // Custom Implementation
            Destroy(gameObject);
        }
    }

    public override bool MoveItemToInventory(int itemIndex)
    {
        var movedItem = containerItems[itemIndex];

        if (!base.MoveItemToInventory(itemIndex))
        {
            return false;
        }

        // Check if we are taking Weapon, which is in enemy's hand
        if (associatedEnemyWeapon != null && movedItem == associatedEnemyWeapon.GetComponent<WeaponData>().weaponData)
        {
            Destroy(associatedEnemyWeapon);
        }

        return true;
    }
}