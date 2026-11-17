
using UnityEngine;

public enum ItemType
{
    Coin,
    Heart,
    Hazard
}

[CreateAssetMenu(
    fileName = "ItemData",
    menuName = "Rabbit Adventure/Item Data"
)]
public class ItemData : ScriptableObject
{
    public string itemName;
    public ItemType itemType;

    [Header("Values")]
    public int value = 1;
    public int healAmount = 1;
    public int damageAmount = 1;

    [Header("Lifetime")]
    public float lifetime = 10f;
}
