using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class TimeChecker : MonoBehaviour
{
    static TimeAndDateResponseModel date;
    static string extractedDate;
    static string extractedTime;
    TimeAndDateResponseModel _globalDate;
    private DateTime _currentGlobalDateTime;
    private DateTime _syncedGlobalDateTime;
    [HideInInspector] public static float newDifferenceSeconds = 0;
    private bool _fetchedTime;
    public ShopItemData data;
    private List<ShopItemData.ShopItem>  _timedItems = new List<ShopItemData.ShopItem>();
    public TimedItemManager[] _items;
    public static TimeChecker Instance { get; private set; }
    async void Awake()
    {
        _items = new TimedItemManager[0];
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        while (!_fetchedTime)
        {
            APIManager apiManager = FindObjectOfType<APIManager>();
            _globalDate = await apiManager.GetCurrentDateAndTime();
            if (_globalDate.datetime != null)
            {
                _fetchedTime = true;
            }
            _currentGlobalDateTime = DateTime.Parse(_globalDate.datetime, null, DateTimeStyles.RoundtripKind);
            _syncedGlobalDateTime = _currentGlobalDateTime;
        }
        Debug.Log("Fetched time !" + _syncedGlobalDateTime);
        extractedDate = _syncedGlobalDateTime.ToString("yyyy-MM-dd");
        extractedTime = _syncedGlobalDateTime.ToString("HH:mm:ss");
        StartCoroutine(SyncTimeEverySecond());
        
        for (int i = 0; i < data.ShopData.Length; i++)
        {
            if (data.ShopData[i].isTimed)
            {
                _timedItems.Add(data.ShopData[i]);
                CheckItem(data.ShopData[i]);
            }
        }
    }

    IEnumerator SyncTimeEverySecond()
    {
            yield return new WaitForSeconds(1f);
            _syncedGlobalDateTime = _syncedGlobalDateTime.AddSeconds(1);
            extractedDate = _syncedGlobalDateTime.ToString("yyyy-MM-dd");
            extractedTime = _syncedGlobalDateTime.ToString("HH:mm:ss");
            SyncTimers();
            StartCoroutine(SyncTimeEverySecond());
    }

    void SyncTimers()
    {
        _items = new TimedItemManager[0];
         _items = FindObjectsOfType<TimedItemManager>();
        for (int i = 0; i < _items.Length; i++)
        {
            _items[i].UpdateWithTimeTick();
        }
    }

    public string GetCurrentDateTimeString()
    {
        return _syncedGlobalDateTime.ToString("MM/dd/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
    }


    public float TimeDifference()
    {
        return newDifferenceSeconds;
    }
    
    void CheckItem(ShopItemData.ShopItem currentData)
    {
        if (GlobalValue.GetItemState(currentData.itemName))
        {
            if (ConvertedStringToDate(GetCurrentDateTimeString()) >
                ConvertedStringToDate(GlobalValue.ItemOpened(currentData.itemName))
                    .AddHours(currentData.duration == TimedItemManager.ItemDuration.Day?24:1))
            {
                print("deactivate bruh" + currentData.itemName);
                GlobalValue.SetItemState(false,currentData.itemName);
            }
        }
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

}
