using System.Collections;
using System.Collections.Generic;
using DynamicScrollRect;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : ScrollItem<ScrollItemData>
{
    public DynamicScrollRect.DynamicScrollRect dynamicScroll = null;
   [HideInInspector]public string itemName;
   [HideInInspector]public int itemPrice;
   [HideInInspector]public bool hasMaxValue;
   [HideInInspector]public int maxAmount;
    public Text itemPriceText;
    public Text itemCountText;
    public Text itemNameText;
    public Button purchaseButton;
    public Image itemImage;
    public Image deactivationHandler;
    protected override void InitItemData(ScrollItemData data)
    {
        Init(data.Data);
        base.InitItemData(data);
    }
    // Start is called before the first frame update
    public void Init(Shop.ShopItemData itemData)
    {
        hasMaxValue = itemData.hasMaxValue;
        maxAmount = itemData.itemMaxValue;
        itemImage.sprite = itemData.itemImage;
        itemPrice = itemData.itemPrice;
        itemName = itemData.itemName;

        itemPriceText.text = itemPrice.ToString();
        int itemCount = GlobalValue.GetChosenShopItem(itemName);
        itemNameText.text = itemName;
        itemCountText.text = itemCount.ToString();
        if (User.Coin> itemPrice)
        {
            if (hasMaxValue)
            {
                if (itemCount < maxAmount)
                {
                    purchaseButton.interactable = true;
                    deactivationHandler.gameObject.SetActive(false);
                }
                else
                {
                    purchaseButton.interactable = false;
                    deactivationHandler.gameObject.SetActive(true);
                }
            }
            else
            {
                purchaseButton.interactable = true;
                deactivationHandler.gameObject.SetActive(false);
            }
        }
        else
        {
            purchaseButton.interactable = false;
            deactivationHandler.gameObject.SetActive(true);
        }
    }

    public void BuyItem()
    {
        
        GlobalValue.IncrementChosenShopItem(itemName);
        int itemCount = GlobalValue.GetChosenShopItem(itemName);
        itemCountText.text = itemCount.ToString();
        User.Coin = -itemPrice;
        if (User.Coin> itemPrice)
        {
            if (hasMaxValue)
            {
                if (itemCount < maxAmount)
                {
                    purchaseButton.interactable = true;
                    deactivationHandler.gameObject.SetActive(false);
                }
                else
                {
                    purchaseButton.interactable = false;
                    deactivationHandler.gameObject.SetActive(true);
                }
            }
            else
            {
                purchaseButton.interactable = true;
                deactivationHandler.gameObject.SetActive(false);
            }
        }
        else
        {
            purchaseButton.interactable = false;
            deactivationHandler.gameObject.SetActive(true);
        }
    }
}
