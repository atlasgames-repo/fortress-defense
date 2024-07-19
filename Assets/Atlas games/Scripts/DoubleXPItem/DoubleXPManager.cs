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

        await SetDate();
    }

    private static float newDifferenceSeconds = 0;
    private DateTime _currentGlobalDateTime;
    private bool _doubleXPStatus = false;

    async Task SetDate()
    {
        _globalDate = await APIManager.instance.GetCurrentDateAndTime();
        dateTimeString = _globalDate.datetime;
        string time = dateTimeString.Substring(11, 8);
        string dateString = dateTimeString.Substring(0, 10);
        print(dateString+time);
        _currentGlobalDateTime = ConvertedStringToDate(dateString + time);
        if (_currentGlobalDateTime > ConvertedStringToDate(GlobalValue.DoubleXpActivationTime)
                .AddHours(GlobalValue.DoubleXPDuration))
        {
            GlobalValue.DoubleXp = false;
        }
        else
        {
            GlobalValue.DoubleXp = true;
        }

        if (PlayerPrefs.GetFloat("MainTimeDifference") == 0)
        {
            PlayerPrefs.SetFloat("MainTimeDifference", (float)(_currentGlobalDateTime - DateTime.Now).TotalSeconds);
        }

        TimeSpan newDifference = _currentGlobalDateTime -
                                 DateTime.Now.AddSeconds(PlayerPrefs.GetFloat("MainTimeDifference"));
        if (Math.Abs(newDifference.TotalSeconds) > 100)
        {
            newDifferenceSeconds = (float)newDifference.TotalSeconds;
        }
    }

    static DateTime ConvertedStringToDate(string insertedTime)
    {
        try
        {
        //    int year = int.Parse(insertedTime.Substring(0, 4));
        //    int month = int.Parse(insertedTime.Substring(5, 2));
        //    int day = int.Parse(insertedTime.Substring(8, 2));
        //    int hour = int.Parse(insertedTime.Substring(10, 2));
        //    int minute = int.Parse(insertedTime.Substring(13, 2));
        //    int second = int.Parse(insertedTime.Substring(16, 2));
        //    return new DateTime(year, month, day, hour, minute, second);
        string format = "yyyy-MM-ddHH:mm:ss";
        return DateTime.ParseExact(insertedTime, format, null);
        }
        catch (Exception e)
        {
            Debug.Log(e);
                         return DateTime.MinValue;
        }
    }

    void Update()
    {
        CheckDoubleXpStatus();
        if (_doubleXPStatus)
        {
            if (DateTime.Now.AddSeconds(PlayerPrefs.GetFloat("MainTimeDifference")) >
                ConvertedStringToDate(GlobalValue.DoubleXpActivationTime)
                    .AddHours(GlobalValue.DoubleXPDuration))
            {
                GlobalValue.DoubleXp = false;
            }
        }
    }


    private async void CheckDoubleXpStatus()
    {
        _doubleXPStatus = await Task.Run(() =>
        {
            try
            {
                return GlobalValue.DoubleXp;
            }
            catch (Exception e)
            {
                return false;
            }
        });
    }

    static string CountDownText(TimeSpan timeDifference)
    {
        return string.Format("{0:D2}:{1:D2}:{2:D2}",
            timeDifference.Hours, timeDifference.Minutes, timeDifference.Seconds);
    }

    public static string CounterText()
    {
        if (GlobalValue.DoubleXp)
        {
            DateTime dueDateTime = ConvertedStringToDate(GlobalValue.DoubleXpActivationTime)
                .AddHours(GlobalValue.DoubleXPDuration);
            TimeSpan timeDifference = dueDateTime -
                                      DateTime.Now.AddSeconds(PlayerPrefs.GetFloat("MainTimeDifference") +
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
