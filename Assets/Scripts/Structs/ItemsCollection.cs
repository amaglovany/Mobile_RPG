using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsCollection : MonoBehaviour
{
    #region Singleton

    public static ItemsCollection Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public Gold gold;
    public Other stone;
    public Weapon knife;
    public Weapon saw;
    public Weapon scythe_big;
    public Weapon scythe_small;
    public Weapon torch;
    public Weapon fork;
    public Weapon gun;
    public ItemConsumable potion;

    public Weapon GetWeapon(Weapon weapon)
    {
        return weapon;
    }
}