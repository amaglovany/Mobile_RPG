using UnityEngine;

public struct Colors
{
    public static readonly Color32 greenMessage = Color.green;
    public static readonly Color32 yellowMessage = Color.yellow;
    public static readonly Color32 redMessage = Color.red;

    public static readonly Color32 playerEngagedUI = new Color32(255, 255, 255, 255);
    public static readonly Color32 playerCalmUI = new Color32(255, 255, 255, 100);

    public static readonly Color32 playerActiveUI = new Color32(170, 50, 50, 255);
    public static readonly Color32 playerDefaultUI = new Color32(255, 255, 255, 255);

    public static readonly Color32 screenEffect = new Color32(255, 255, 255, 255);

    public static readonly Color32 enemyEngagedHP = new Color32(255, 255, 255, 255);
    public static readonly Color32 enemyCalmHP = new Color32(255, 255, 255, 100);
}