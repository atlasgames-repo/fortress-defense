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
    public ShopPopup popup;
    public GameObject coinImage;
    public GameObject expImage;
    private Shop.ItemPurchaseType _purchaseType;
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
        _purchaseType = itemData.purchaseType;
        if (_purchaseType == Shop.ItemPurchaseType.Coin)
        {
            coinImage.SetActive(true);
            expImage.SetActive(false);
        }
        else
        {
            expImage.SetActive(true);
            coinImage.SetActive(false);
        }
        Vector2 originalSize = new Vector2(itemImage.rectTransform.sizeDelta.x,itemImage.rectTransform.sizeDelta.y);
        itemImage.SetNativeSize();
        if (itemImage.rectTransform.sizeDelta.x > itemImage.rectTransform.sizeDelta.y)
        {
            float aspectRatio = (float)itemImage.rectTransform.sizeDelta.x / itemImage.rectTransform.sizeDelta.y;
            itemImage.rectTransform.sizeDelta = new Vector2(originalSize.x, originalSize.y / aspectRatio);
        }
        else
        {
            float aspectRatio = (float)itemImage.rectTransform.sizeDelta.y / itemImage.rectTransform.sizeDelta.x;
            itemImage.rectTransform.sizeDelta = new Vector2(originalSize.x / aspectRatio, originalSize.y);
        }
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
        SoundManager.Click();
        if (_purchaseType == Shop.ItemPurchaseType.Coin)
        {
            User.Coin = -itemPrice;
        }
        else
        {
            User.Uxp = -itemPrice;
        }

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

    public void HandleDeactivation()
    {
        popup.gameObject.SetActive(true);
        if (User.Coin < itemPrice)
        {
            popup.OpenDialog(true);
        }else if (hasMaxValue && maxAmount == GlobalValue.GetChosenShopItem(itemName))
        {
            popup.OpenDialog(false);
        }
    }
}
