using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stat agility; //MoveSpeed

    // Magic
    public Stat arcane; //Can do magic, but are also more resistant to magic
    public Stat endurance; //HP
    public Stat fire; //Weak to water damage/strong to fire damage
    public Stat stamina; //SP
    public Stat strength; //DMG
    public Stat water; //weak to fire damage/strong to water damage
}