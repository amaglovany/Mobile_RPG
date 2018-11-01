using UnityEngine;

public abstract class Item : ScriptableObject
{
    public InventoryUI.TCategory type;
    public string itemName;
    public Sprite itemSprite;
    public string itemDescription;
}