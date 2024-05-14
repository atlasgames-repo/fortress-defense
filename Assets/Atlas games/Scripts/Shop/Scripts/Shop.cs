using System;
using System.Collections;
using System.Collections.Generic;
using DynamicScrollRect;
using UnityEngine;

public class Shop : MonoBehaviour
{

    public ScrollContent content;
    public Transform scrollContentParent;
    public enum ItemTypes
    {
        Pet,Feature,Magic,Monster,Website
    }

    public enum ItemPurchaseType
    {
        Exp,Coin
    }
    [Serializable]
    public class ShopItemData
    {
        public string itemName;
        public Sprite itemImage;
        public int itemPrice;
        public int itemMaxValue;
        public bool hasMaxValue;
        public ItemTypes type;
        public ItemPurchaseType purchaseType;
    }
    public ShopItemData[] shopItems;
    ItemTypes _chosenType;
    private List<ShopItemData> _chosenItems;
    public void OpenMenu(string menuName)
    {
        SoundManager.Instance.PauseMusic(true);
        switch (menuName)
        {
            case "pets":
                _chosenType = ItemTypes.Pet;
                break;
            case "features":
                _chosenType = ItemTypes.Feature;
                break;
            case "magics":
                _chosenType = ItemTypes.Magic;
                break;
            case "monsters":
                _chosenType = ItemTypes.Monster;
                break;
            case "website":
                _chosenType = ItemTypes.Website;
                break;
        }
        List<ScrollItemData> contentDatas = new List<ScrollItemData>();
        for (int i = 0; i < shopItems.Length; i++)
        {
            if (shopItems[i].type == _chosenType)
            {
                contentDatas.Add(new ScrollItemData(shopItems[i]));
            }
        }

        for (int i = 0; i < scrollContentParent.childCount; i++)
        {
            if (scrollContentParent.GetChild(i).name != "RefItem")
            {
                Destroy(scrollContentParent.GetChild(i).gameObject);
            }            
        }
        content.InitScrollContent(contentDatas);
    }
}
