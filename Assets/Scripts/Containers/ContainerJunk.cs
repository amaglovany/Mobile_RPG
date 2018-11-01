using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerJunk : Container
{
    protected override void Interact()
    {
        if (!canInteract || containerItems.Count < 1)
            return;

        base.Interact();

        // Custom Implementation
        OpenContainer();
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
}