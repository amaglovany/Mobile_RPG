using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestAnimation : MonoBehaviour
{
    private Animator chestAnimator;

    private void Awake()
    {
        chestAnimator = GetComponent<Animator>();
    }

    public void OpenChestAnimation()
    {
        //chestAnimator.enabled = true;
        chestAnimator.Play("Chest_Open");
    }

    public void CloseChestAnimation()
    {
        //chestAnimator.enabled = true;
        chestAnimator.Play("Chest_Close");
    }

    public void EmptyChestAnimation()
    {
        //chestAnimator.enabled = true;
        chestAnimator.Play("Chest_Empty");
    }
}