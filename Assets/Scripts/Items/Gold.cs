using UnityEngine;

[CreateAssetMenu(fileName = "Gold", menuName = "Gold")]
public class Gold : ItemStack
{
    public int GetGoldAmount()
    {
        return amount;
    }
}