using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TimedItemManager : MonoBehaviour
{
    public Text counterText;
    public Text buttonText;
    [HideInInspector] public string itemName;

    [HideInInspector] public bool isInit;
    [HideInInspector] public bool isTimed;
    public Image coinSprite;
    public Image expSprite;
    [HideInInspector] public bool purchaseWithCoin;

    public enum ItemDuration
    {
        Minute,
        Second
    }

    [HideInInspector] public ItemDuration duration;
    [HideInInspector] public string[] allTimedItems;
    private bool _activeItem;
    private string _activeItemName;

    public void Init(string name, ItemDuration dur, string[] otherItems, bool coinWithPurchase)
    {
        purchaseWithCoin = coinWithPurchase;
        coinSprite.gameObject.SetActive(purchaseWithCoin);
        expSprite.gameObject.SetActive(!purchaseWithCoin);
        isTimed = true;
        allTimedItems = otherItems;
        itemName = name;
        duration = dur;
        _activeItem = false;

        for (int i = 0; i < allTimedItems.Length; i++)
        {
            if (GlobalValue.GetItemState(allTimedItems[i]))
            {
                _activeItemName = allTimedItems[i];
                _activeItem = true;
                break;
            }
        }

        if (_activeItem)
        {
            isInit = true;
            UpdateWithTimeTick();
            buttonText.gameObject.SetActive(false);
            coinSprite.gameObject.SetActive(false);
            expSprite.gameObject.SetActive(false);
            counterText.gameObject.SetActive(true);
        }
        else
        {
            expSprite.gameObject.SetActive(!purchaseWithCoin);
            coinSprite.gameObject.SetActive(purchaseWithCoin);
        }
    }

    public void UpdateWithTimeTick()
    {
        _activeItem = false;
        if (allTimedItems.Length > 0)
        {
            for (int i = 0; i < allTimedItems.Length; i++)
            {
                if (GlobalValue.GetItemState(allTimedItems[i]))
                {
                    _activeItemName = allTimedItems[i];
                    _activeItem = true;
                }
            }
        }

        if (_activeItem && !isInit)
        {
            isInit = true;
            buttonText.gameObject.SetActive(false);
            counterText.gameObject.SetActive(true);
        }

        if (isInit)
        {
            counterText.text = CountDownText();

            if (ConvertedStringToDate(TimeChecker.Instance.GetCurrentDateTimeString()) >
                ConvertedStringToDate(GlobalValue.ItemOpened(_activeItemName))
                    .AddSeconds(itemDuration()) && _activeItemName == itemName)
            {
                TimeChecker.Instance.InitTimedItems();
                isInit = false;
                coinSprite.gameObject.SetActive(purchaseWithCoin);
                expSprite.gameObject.SetActive(!purchaseWithCoin);
                GlobalValue.SetItemState(false, itemName);
                buttonText.gameObject.SetActive(true);
                counterText.gameObject.SetActive(false);
            }
        }
    }

    int itemDuration()
    {
        switch (duration)
        {
            case ItemDuration.Minute:
                return 60;
            case ItemDuration.Second:
                return 1;
            default:
                return 0;
        }
    }

    static DateTime ConvertedStringToDate(string insertedTime)
    {
        DateTime parsedDate;
        parsedDate = DateTime.ParseExact(insertedTime, "M/dd/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
        return parsedDate;
    }

    string CountDownText()
    {
        TimeSpan remaining = ConvertedStringToDate(GlobalValue.ItemOpened(_activeItemName))
            .AddSeconds(GlobalValue.ItemDuration(_activeItemName)) -
            ConvertedStringToDate(TimeChecker.Instance.GetCurrentDateTimeString());

        return remaining.ToString(@"hh\:mm\:ss");
    }

    public void GetTime(ItemDuration duration)
    {
        GlobalValue.SetItemActivationTime(itemName, TimeChecker.Instance.GetCurrentDateTimeString());
        GlobalValue.SetItemState(true, itemName);
        isInit = true;
        counterText.gameObject.SetActive(true);
        buttonText.gameObject.SetActive(false);
        TimeChecker.Instance.InitTimedItems();

        switch (duration)
        {
            case ItemDuration.Minute:
                GlobalValue.SetItemDuration(itemName, 60);
                break;
            case ItemDuration.Second:
                GlobalValue.SetItemDuration(itemName, 1);
                break;
        }
    }

    public void ActivateDoubleXp(ItemDuration dur)
    {
        if (!GlobalValue.GetItemState(itemName) && !isInit)
        {
            isInit = true;
            buttonText.gameObject.SetActive(false);
            counterText.gameObject.SetActive(true);

            if (!GlobalValue.GetItemState(itemName))
            {
                GetTime(dur);
            }
        }
    }
}