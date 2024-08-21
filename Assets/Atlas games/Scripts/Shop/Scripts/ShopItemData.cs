using System;
using  UnityEngine;

[CreateAssetMenu(fileName = "ShopData", menuName = "ScriptableObjects/ShopData", order = 1)]
public class ShopItemData : ScriptableObject
{

    [Serializable]
    public class ShopItem
    {
        public string itemName;
        public Sprite itemImage;
        public int itemPrice;
        public int itemMaxValue;
        public bool hasMaxValue;
        public Shop.ItemTypes type;
        public Shop.ItemPurchaseType purchaseType;
        public bool levelLock;
        public int levelToUnlock;
        public int id;
        public bool isFree;
    }
    public ShopItem[] ShopData;
}