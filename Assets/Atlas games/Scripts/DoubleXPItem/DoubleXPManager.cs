using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DoubleXPManager : MonoBehaviour
{
     static TimeAndDateResponseModel date;
     static string extractedDate;
     static string extractedTime;
     static string dateTimeString;

    public enum DoubleXpDuration
    {
        Day,
        Hour
    }

    TimeAndDateResponseModel _globalDate;
    static TimeSpan globalTimeDifference;
    async void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SetDate();
    }

    async Task SetDate()
    {
        _globalDate = await APIManager.instance.GetCurrentDateAndTime();
        dateTimeString = _globalDate.datetime;
        string time = dateTimeString.Substring(11, 8);
        string dateString = dateTimeString.Substring(0, 10);
        DateTime currentGlobalDateTime = ConvertedStringToDate(dateString + time);
        if (currentGlobalDateTime > ConvertedStringToDate(GlobalValue.DoubleXpActivationTime)
                .AddHours(GlobalValue.DoubleXPDuration))
        {
            GlobalValue.DoubleXpActive = 0;
        }
        else
        {
            GlobalValue.DoubleXpActive = 1;
        }
        globalTimeDifference = currentGlobalDateTime - DateTime.Now;
    }
    static DateTime ConvertedStringToDate(string insertedTime)
    {
        int year = int.Parse(insertedTime.Substring(0, 4));
        int month = int.Parse(insertedTime.Substring(5, 2));
        int day = int.Parse(insertedTime.Substring(8, 2));
        int hour = int.Parse(insertedTime.Substring(10, 2));
        int minute = int.Parse(insertedTime.Substring(13, 2));
        int second = int.Parse(insertedTime.Substring(16, 2));
        return new DateTime(year, month, day, hour, minute, second);
    }

    void Update()
    {
        if (GlobalValue.DoubleXpActive==1)
        {
            if (DateTime.Now.AddSeconds(globalTimeDifference.TotalSeconds) >
                ConvertedStringToDate(GlobalValue.DoubleXpActivationTime)
                    .AddHours(GlobalValue.DoubleXPDuration))
            {
                GlobalValue.DoubleXpActive = 0;
            }
        }
    }
    static string CountDownText(TimeSpan timeDifference)
    {
        return string.Format("{0:D2}:{1:D2}:{2:D2}",
            timeDifference.Hours, timeDifference.Minutes, timeDifference.Seconds);
    }

    public static string CounterText()
    {
        if (GlobalValue.DoubleXpActive == 1)
        {
            DateTime dueDateTime = ConvertedStringToDate(GlobalValue.DoubleXpActivationTime)
                .AddHours(GlobalValue.DoubleXPDuration);
            TimeSpan timeDifference = dueDateTime - DateTime.Now.AddSeconds(globalTimeDifference.TotalSeconds);
            string countdownText = CountDownText(timeDifference);
            return countdownText;
        }
        else
        {
            return null;
        }
    }

    public static async void GetTime(DoubleXpDuration duration)
    {
        date = await APIManager.instance.GetCurrentDateAndTime();
        dateTimeString = date.datetime;
        extractedDate = dateTimeString.Substring(0, 10);
        extractedTime = dateTimeString.Substring(11, 8);
        GlobalValue.DoubleXpActivationTime = extractedDate + extractedTime;
        GlobalValue.DoubleXpActive = 1;
        if (duration == DoubleXpDuration.Hour)
        {
            GlobalValue.DoubleXPDuration = 1;
        }

        if (duration == DoubleXpDuration.Day)
        {
            GlobalValue.DoubleXPDuration = 24;
        }
    }
}