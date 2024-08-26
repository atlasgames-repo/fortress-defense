using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
    public static DoubleXPManager self;
    async void Awake()
    {
        if (self == null){
            self = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(this);
        }

        await SetDate();
    }

    private static float newDifferenceSeconds = 0;
    private DateTime _currentGlobalDateTime;

    async Task SetDate()
    {
        _globalDate = await APIManager.instance.GetCurrentDateAndTime();
        dateTimeString = _globalDate.datetime;
        string time = dateTimeString.Substring(11, 8);
        string dateString = dateTimeString.Substring(0, 10);
        string finalDateTime = dateString + time;
        _currentGlobalDateTime = ConvertedStringToDate(finalDateTime);
        if (_currentGlobalDateTime > ConvertedStringToDate(GlobalValue.DoubleXpActivationTime)
                .AddHours(GlobalValue.DoubleXPDuration))
        {
            GlobalValue.DoubleXp = false;
        }
        else
        {
            GlobalValue.DoubleXp = true;
        }

        if (GlobalValue.MainTimeDifference == 0)
        {
            GlobalValue.MainTimeDifference = (float)(_currentGlobalDateTime - DateTime.Now).TotalSeconds;
        }

        TimeSpan newDifference = _currentGlobalDateTime -
                                 DateTime.Now.AddSeconds(GlobalValue.MainTimeDifference);
        if (Math.Abs(newDifference.TotalSeconds) > 100)
        {
            newDifferenceSeconds = (float)newDifference.TotalSeconds;
        }
    }

    static DateTime ConvertedStringToDate(string insertedTime)
    {
        DateTime parsedDate;
        DateTime.TryParseExact(insertedTime,"yyyy-MM-ddTHH:mm:ss",null,DateTimeStyles.None,out parsedDate);
        return parsedDate;
    }

    void Update()
    {
        DateTime dueDateTime = ConvertedStringToDate(GlobalValue.DoubleXpActivationTime)
            .AddHours(GlobalValue.DoubleXPDuration);
        TimeSpan timeDifference = dueDateTime -
                                  DateTime.Now.AddSeconds(GlobalValue.MainTimeDifference +
                                                          newDifferenceSeconds);
        if (GlobalValue.DoubleXp)
        {
            if (DateTime.Now.AddSeconds(GlobalValue.MainTimeDifference) >
                ConvertedStringToDate(GlobalValue.DoubleXpActivationTime)
                    .AddHours(GlobalValue.DoubleXPDuration))
            {
                GlobalValue.DoubleXp = false;
            }
        }
    }


   

    static string CountDownText(TimeSpan timeDifference)
    {
        int totalHours = (int)timeDifference.TotalHours; // Get the total hours as an integer
        return string.Format("{0:D2}:{1:D2}:{2:D2}",
            totalHours, timeDifference.Minutes, timeDifference.Seconds);
    }

    public static string CounterText()
    {
        if (GlobalValue.DoubleXp)
        {
            DateTime dueDateTime = ConvertedStringToDate(GlobalValue.DoubleXpActivationTime)
                .AddHours(GlobalValue.DoubleXPDuration);
            TimeSpan timeDifference = dueDateTime -
                                      DateTime.Now.AddSeconds(GlobalValue.MainTimeDifference +
                                                              newDifferenceSeconds);
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
        GlobalValue.DoubleXp = true;
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

