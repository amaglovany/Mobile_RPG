using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerChest : Container
{
    [HideInInspector] public ChestAnimation chestAnim;

    private void Start()
    {
        chestAnim = GetComponent<ChestAnimation>();

        ContainerGenerator.Instance.FillContainer(this, containerType);
    }

    protected override void Interact()
    {
        if (!canInteract || containerItems.Count < 1)
            return;

        base.Interact();

        // Custom Implementation
        chestAnim.OpenChestAnimation();
        OpenContainer();
    }

    public void CloseChest()
    {
        chestAnim.CloseChestAnimation();
    }

    protected override void IfContainerEmpty()
    {
        if (containerItems.Count < 1)
        {
            base.IfContainerEmpty();

            // Custom Implementation
            SwitchInteractableState();
            GetComponent<ChestAnimation>().EmptyChestAnimation();
        }
    }

    private void DisableChest()
    {
        base.IfContainerEmpty();

        // Custom Implementation
        Destroy(GetComponent<Container>());
        Destroy(GetComponent<ChestAnimation>());
        Destroy(GetComponent<Animator>());
    }
}