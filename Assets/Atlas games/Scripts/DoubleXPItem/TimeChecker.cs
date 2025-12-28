using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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
    [HideInInspector]public TimedItemManager[] _items;
    public static TimeChecker Instance { get; private set; }
    private Coroutine _syncCoroutine;
    async void Awake()
    {
        _items = new TimedItemManager[0];
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            if (_syncCoroutine != null) 
                StopCoroutine(_syncCoroutine);
            Destroy(gameObject);
            return;
        }

        while (!_fetchedTime)
        {
            if (APIManager.self == null) {// If APIManager is not initialized, we can use a fallback
                Debug.LogWarning("APIManager not initialized, using local time as fallback.");
                TimeAndDateResponseModel time = new TimeAndDateResponseModel();
                time.datetime = DateTime.Now.ToString();
                _globalDate = time;
                break;
            }
            _globalDate = await APIManager.self.GetCurrentDateAndTime();
            if (_globalDate.datetime != null)
            {
                _fetchedTime = true;
            }
            _currentGlobalDateTime = DateTime.Parse(_globalDate.datetime, null, DateTimeStyles.RoundtripKind);
            _syncedGlobalDateTime = _currentGlobalDateTime;
        }
        extractedDate = _syncedGlobalDateTime.ToString("yyyy-MM-dd");
        extractedTime = _syncedGlobalDateTime.ToString("HH:mm:ss");
        _syncCoroutine = StartCoroutine(SyncTimeEverySecond());
        
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

    public void SyncTimers()
    {
        _items = FindObjectsByType<TimedItemManager>(FindObjectsSortMode.None);
       if (_items.Length > 0)
       {
           for (int i = 0; i < _items.Length; i++)
           {
               if (_items[i].isInit)
               {
                   _items[i].UpdateWithTimeTick();
               }
           }
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
                    .AddHours(currentData.duration == TimedItemManager.ItemDuration.Minute?60:1))
            {
                GlobalValue.SetItemState(false,currentData.itemName);
            }
        }
    }
    static DateTime ConvertedStringToDate(string insertedTime)
    {
        DateTime parsedDate;
        parsedDate=   DateTime.ParseExact(insertedTime, "M/dd/yyyy h:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture); 
        return parsedDate;
    }

    public void InitTimedItems()
    {
        _items = FindObjectsByType<TimedItemManager>(FindObjectsSortMode.None);
        for (int i = 0; i < _items.Length; i++)
        {
            _items[i].Init(_items[i].itemName,_items[i].duration,_items[i].allTimedItems,_items[i].purchaseWithCoin);
        }
        SyncTimers();
    }
}
