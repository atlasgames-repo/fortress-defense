using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using Newtonsoft.Json;
using System.Linq;

[Serializable]
public class LifeTTR
{
    //public Dictionary<int, DateTime> times;
    public int minutes = 30;
    IEnumerator coroutine;

    public LifeTTR()
    {
        coroutine = Tickes();
        minutes = 30;
    }
    public LifeTTR(int _minutes)
    {
        coroutine = Tickes();
        minutes = _minutes;
    }
    public void Inintilize()
    {
        APIManager.instance.StartCoroutine(coroutine);
    }
    // This class must be somewhere that only destroies when the Game Quit
    ~LifeTTR()
    {
        APIManager.instance.StopCoroutine(coroutine);
    }
    public void addLifeTTR(int key)
    {
        DateTime lastTime;
        bool is_lastTime = LifeTTRSource.LifeTTRv2.TryGetValue(key + 1, out lastTime);
        if (is_lastTime)
            LifeTTRSource.Add(key, lastTime.AddMinutes(minutes));
        else
            LifeTTRSource.Add(key, DateTime.Now.AddMinutes(minutes));
    }
    public double TTL(int key)
    {
        DateTime Time;
        bool is_Time = LifeTTRSource.LifeTTRv2.TryGetValue(key, out Time);
        if (is_Time)
            return (Time - DateTime.Now).TotalMinutes;
        else
            return -1;
    }
    public double TTL()
    {
        int max_key = LifeTTRSource.LifeTTRv2.Select(f => f.Key).DefaultIfEmpty().Max();
        DateTime Time;
        bool is_Time = LifeTTRSource.LifeTTRv2.TryGetValue(max_key, out Time);
        if (is_Time)
            return (Time - DateTime.Now).TotalMinutes;
        else
            return -1;
    }
    public float TTLPercent
    {
        get { return (float)TTL() / (float)minutes; }
    }
    public void CheckTTL()
    {
        var ItemToRemove = LifeTTRSource.LifeTTRv2.Where(f => TTL(f.Key) < 0).ToArray();
        foreach (var item in ItemToRemove)
        {
            LifeTTRSource.Remove(item.Key);
            if (LifeTTRSource.Life < 6)
                LifeTTRSource.Life += 1;
        }
        if (ItemToRemove.Length > 0)
        {
            LifeTTRSource.IncrementKeys();
        }
    }
    IEnumerator Tickes()
    {
        yield return new WaitForSeconds(0.01f);
        while (true)
        {
            CheckTTL();
            yield return new WaitForSeconds(5);
        }

    }

}

public class LifeTTRSource : MonoBehaviour
{
    public static void Add(int key, DateTime value)
    {
        var dict = LifeTTRv2;
        if (!dict.ContainsKey(key))
        {
            dict.Add(key, value);
            LifeTTRv2 = dict;
        }
    }
    public static void IncrementKeys()
    {
        var dict = LifeTTRv2;
        var temp = new Dictionary<int, DateTime>();
        var ItemToRname = dict.ToArray();
        foreach (var item in ItemToRname)
        {
            temp[item.Key + 1] = item.Value;
        }
        LifeTTRv2 = temp;
    }
    public static void Remove(int key)
    {
        var dict = LifeTTRv2;
        if (!dict.ContainsKey(key))
        {
            dict.Remove(key);
            LifeTTRv2 = dict;
        }
    }
    public static string Json
    {
        get { return JsonConvert.SerializeObject(LifeTTRv2, Formatting.Indented); }
    }
    public static int Life
    {
        get { return PlayerPrefs.GetInt("life", 6); }
        set { PlayerPrefs.SetInt("life", value); }
    }

    public static Dictionary<int, DateTime> LifeTTRv2
    {
        get { return JsonConvert.DeserializeObject<Dictionary<int, DateTime>>(PlayerPrefs.GetString("lifettr", "{}")); }
        set { PlayerPrefs.SetString("lifettr", JsonConvert.SerializeObject(value, Formatting.Indented)); }
    }

}

