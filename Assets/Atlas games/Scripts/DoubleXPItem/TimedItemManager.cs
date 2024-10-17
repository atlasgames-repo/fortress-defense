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
    [HideInInspector]public string itemName;

  [HideInInspector]  public bool isInit;

    public enum ItemDuration
    {
        Day,
        Hour
    }


    [HideInInspector]public ItemDuration duration;
    private string[] _allTimedItems;
    private bool _activeItem;
    private string _activeItemName;

    public void Init(string name, ItemDuration dur,string[] otherItems)
    {
        _allTimedItems = otherItems;
        itemName = name;
        duration = dur;
        _activeItem = false;
        for (int i = 0; i < _allTimedItems.Length; i++)
        {
            if (GlobalValue.GetItemState(_allTimedItems[i]))
            {
                _activeItemName = _allTimedItems[i];
                _activeItem = true;
                break;
            }
        }
        if (_activeItem)
        {
            isInit = true;
            UpdateWithTimeTick();
            buttonText.gameObject.SetActive(false);
            counterText.gameObject.SetActive(true);
        }
    }
    
    public void UpdateWithTimeTick()
    {
        print(isInit);
        _activeItem = false;
        for (int i = 0; i < _allTimedItems.Length; i++)
        {
            if (GlobalValue.GetItemState(_allTimedItems[i]))
            {
                _activeItemName = _allTimedItems[i];
                _activeItem = true;
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
                        .AddHours(itemDuration()) && _activeItemName == itemName)
                {
                    isInit = false;
                    GlobalValue.SetItemState(false, itemName);
                    buttonText.gameObject.SetActive(true);
                    counterText.gameObject.SetActive(false);
                }
            //   else
            //   {
            //       buttonText.gameObject.SetActive(false);
            //       counterText.gameObject.SetActive(true);
            //       isInit = true;  
            //   }
            
        }
    }
    
     double itemDuration()
    {
        return duration == ItemDuration.Day ? 24 : 1;
    }


    static DateTime ConvertedStringToDate(string insertedTime)
    {
        DateTime parsedDate;
      parsedDate=   DateTime.ParseExact(insertedTime, "M/dd/yyyy h:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture); 
        return parsedDate;
    }

     string CountDownText()
    {
        return (ConvertedStringToDate(GlobalValue.ItemOpened(_activeItemName)).AddHours(GlobalValue.ItemDuration(_activeItemName)) -
                ConvertedStringToDate(TimeChecker.Instance.GetCurrentDateTimeString())).ToString();
    }
    
    public void GetTime(ItemDuration duration)
    {
        GlobalValue.SetItemActivationTime(itemName, TimeChecker.Instance.GetCurrentDateTimeString());
        GlobalValue.SetItemState(true, itemName);
        print(GlobalValue.GetItemState(itemName));
        isInit = true;
        counterText.gameObject.SetActive(true);
        buttonText.gameObject.SetActive(false);
        UpdateWithTimeTick();
        GlobalValue.SetItemDuration(itemName, duration == ItemDuration.Day ? 24 : 1);
    }


    public void ActivateDoubleXp(bool h24duration)
    {
        if (!GlobalValue.GetItemState(itemName) && !isInit)
        {
            isInit = true;
            buttonText.gameObject.SetActive(false);
            counterText.gameObject.SetActive(true);
            if (!GlobalValue.GetItemState(itemName))
            {
                if (h24duration)
                {
                    GetTime(ItemDuration.Day);
                }
                else
                {
                    GetTime(ItemDuration.Hour);
                }
            }
        }
    }
}

