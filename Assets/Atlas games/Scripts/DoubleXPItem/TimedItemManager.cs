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


    public void Init(string name, ItemDuration dur)
    {
        itemName = name;
        duration = dur;
        if (GlobalValue.GetItemState(itemName))
        {
            isInit = true;
            UpdateWithTimeTick();
            buttonText.gameObject.SetActive(false);
            counterText.gameObject.SetActive(true);
        }
    }
    
    public void UpdateWithTimeTick()
    {
        if (isInit)
        {
              if (GlobalValue.GetItemState(itemName))
            {
                if (ConvertedStringToDate(TimeChecker.Instance.GetCurrentDateTimeString()) >
                    ConvertedStringToDate(GlobalValue.ItemOpened(itemName))
                        .AddHours(itemDuration()))
                {
                    isInit = false;
                    GlobalValue.SetItemState(false, itemName);
                    buttonText.gameObject.SetActive(true);
                    counterText.gameObject.SetActive(false);
                }
                else
                {
                    buttonText.gameObject.SetActive(false);
                    counterText.gameObject.SetActive(true);
                    counterText.text = CountDownText();
                    isInit = true;  
                }
            }
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
        return (ConvertedStringToDate(GlobalValue.ItemOpened(itemName)).AddHours(GlobalValue.ItemDuration(itemName)) -
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

