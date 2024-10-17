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
    
    bool _isInit;
   string[] _otherDependantItems;
    public enum ItemDuration
    {
        Day,
        Hour
    }


    ItemDuration _duration;
    private string _itemName;
    private string _itemToShowTime;
    public void Init(string name, ItemDuration dur,string[] dependantItems = null)
    {
        _otherDependantItems = dependantItems;
        _itemName = name;
        _duration = dur;
        if (GlobalValue.GetItemState(_itemName))
        {
            _isInit = true;
            UpdateWithTimeTick();
            buttonText.gameObject.SetActive(false);
            counterText.gameObject.SetActive(true);
        }
    }
    
    public void UpdateWithTimeTick()
    {
        if (_isInit)
        {
            if (ConvertedStringToDate(TimeChecker.Instance.GetCurrentDateTimeString()) >
                    ConvertedStringToDate(GlobalValue.ItemOpened(_itemToShowTime))
                        .AddHours(itemDuration()) && _itemToShowTime== _itemName)
                {
                    _isInit = false;
                    GlobalValue.SetItemState(false, _itemToShowTime);
                    buttonText.gameObject.SetActive(true);
                    counterText.gameObject.SetActive(false);
                }
                else
                {
                    buttonText.gameObject.SetActive(false);
                    counterText.gameObject.SetActive(true);
                    counterText.text = CountDownText();
                    _isInit = true;  
                }
         
            for (int i = 0; i < _otherDependantItems.Length; i++)
            {
                if (GlobalValue.GetItemState(_otherDependantItems[i]))
                {
                    _itemToShowTime = _otherDependantItems[i];
                    _isInit = true;
                    buttonText.gameObject.SetActive(false);
                    counterText.gameObject.SetActive(true);
                    counterText.text = CountDownText();
                }
            }
        }
    }
    
     double itemDuration()
    {
        return _duration == ItemDuration.Day ? 24 : 1;
    }


     DateTime ConvertedStringToDate(string insertedTime)
    {
        string dateString = "10/17/2024 9:46:18 AM";

        // Extract the components of the date string using Substring
        int month = int.Parse(dateString.Substring(0, 2));
        int day = int.Parse(dateString.Substring(3, 2));
        int year = int.Parse(dateString.Substring(6, 4));
        
        // Handle hour extraction: account for 1 or 2 digit hour values
        int hourStartIndex = 11;
        int hourLength = dateString[hourStartIndex + 1] == ':' ? 1 : 2;
        int hour = int.Parse(dateString.Substring(hourStartIndex, hourLength));
        
        int minute = int.Parse(dateString.Substring(hourStartIndex + hourLength + 1, 2));
        int second = int.Parse(dateString.Substring(hourStartIndex + hourLength + 4, 2));
        
        // Extract the AM/PM part
        string amPm = dateString.Substring(hourStartIndex + hourLength + 7, 2);

        // Convert the hour based on AM/PM
        if (amPm == "PM" && hour != 12)
        {
            hour += 12;  // Convert to 24-hour time for PM
        }
        else if (amPm == "AM" && hour == 12)
        {
            hour = 0;  // Midnight case
        }

        DateTime dateTime = new DateTime(year, month, day, hour, minute, second);
        return dateTime;
    }



     string CountDownText()
    {
        return (ConvertedStringToDate(GlobalValue.ItemOpened(_itemToShowTime)).AddHours(GlobalValue.ItemDuration(_itemToShowTime)) -
                ConvertedStringToDate(TimeChecker.Instance.GetCurrentDateTimeString())).ToString();
    }
    
    public void GetTime(ItemDuration duration)
    {
        GlobalValue.SetItemActivationTime(_itemName, TimeChecker.Instance.GetCurrentDateTimeString());
        GlobalValue.SetItemState(true, _itemName);
        _isInit = true;
        counterText.gameObject.SetActive(true);
        buttonText.gameObject.SetActive(false);
        UpdateWithTimeTick();
        GlobalValue.SetItemDuration(_itemName, duration == ItemDuration.Day ? 24 : 1);
    }


    public void ActivateDoubleXp(bool h24duration)
    {
        if (!GlobalValue.GetItemState(_itemName) && !_isInit)
        {
            _isInit = true;
            buttonText.gameObject.SetActive(false);
            counterText.gameObject.SetActive(true);
            if (!GlobalValue.GetItemState(_itemName))
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

