using UnityEngine;

[System.Serializable]
public class ItemDetails
{
    public int itemID;
    public string itemName;
    public ItemType itemType;
    public Sprite itemIcon;
    public Sprite itemOnworldSprite;
    public string itemDescription;
    public int itemUseRadius;
    public bool canPickedUp;
    public bool canDropped;
    public bool canCarried;             // 是否可以被举起
    public int itemPrice;
    [Range(0,1)]
    public float sellPercentage;        // 卖出时的折扣百分比
}

[System.Serializable]
public struct InventoryItem
{
    public int itemID;
    public int itemAmount;
}