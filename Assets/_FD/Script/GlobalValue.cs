using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using Newtonsoft.Json;
using System.Linq;

public class GlobalValue : MonoBehaviour
{
    public static bool isFirstOpenMainMenu = true;
    public static int worldPlaying = 1;
    public static int levelPlaying = 1;
    //public static int finishGameAtLevel = 50;

    public static string WorldReached = "WorldReached";
    public static bool isSound = true;
    public static bool isMusic = true;


    public static UserResponse user
    {
        get { return JsonUtility.FromJson<UserResponse>(PlayerPrefs.GetString("user", "{}")); }
        set { PlayerPrefs.SetString("user", JsonUtility.ToJson(value)); }
    }

    public static bool isNewGame
    {
        get { return PlayerPrefs.GetInt("isNewGame", 0) == 0; }
        set { PlayerPrefs.SetInt("isNewGame", value ? 0 : 1); }
    }

    public static int lastDayShowNativeAd1
    {
        get { return PlayerPrefs.GetInt("lastDayShowNativeAd1", 0); }
        set { PlayerPrefs.SetInt("lastDayShowNativeAd1", value); }
    }
    public static string token
    {
        get { return PlayerPrefs.GetString("token", null); }
        set { PlayerPrefs.SetString("token", value); }
    }
    public static int lastDayShowNativeAd2
    {
        get { return PlayerPrefs.GetInt("lastDayShowNativeAd2", 0); }
        set { PlayerPrefs.SetInt("lastDayShowNativeAd2", value); }
    }

    public static int lastDayShowNativeAd3
    {
        get { return PlayerPrefs.GetInt("lastDayShowNativeAd3", 0); }
        set { PlayerPrefs.SetInt("lastDayShowNativeAd3", value); }
    }

    public static int SavedCoins
    {
        get { return PlayerPrefs.GetInt("Coins", 200); }
        set { PlayerPrefs.SetInt("Coins", value); }
    }

    public static int LevelPass
    {
        get { return PlayerPrefs.GetInt("LevelReached", 0); }
        set { PlayerPrefs.SetInt("LevelReached", value); }
    }
    public static int WorldPass
    {
        get { return PlayerPrefs.GetInt("WorldReached", 1); }
        set { PlayerPrefs.SetInt("WorldReached", value); }
    }

    public static void LevelStar(int level, int stars)
    {
        PlayerPrefs.SetInt("LevelStars" + level, stars);
    }

    public static int LevelStar(int level)
    {
        return PlayerPrefs.GetInt("LevelStars" + level, 0);
    }

    public static bool RemoveAds
    {
        get { return PlayerPrefs.GetInt("RemoveAds", 0) == 1 ? true : false; }
        set { PlayerPrefs.SetInt("RemoveAds", value ? 1 : 0); }
    }

    public static int ItemDoubleArrow
    {
        get { return PlayerPrefs.GetInt("ItemDoubleArrow", 3); }
        set { PlayerPrefs.SetInt("ItemDoubleArrow", value); }
    }

    public static int ItemTripleArrow
    {
        get { return PlayerPrefs.GetInt("ItemTripleArrow", 1); }
        set { PlayerPrefs.SetInt("ItemTripleArrow", value); }
    }

    public static int ItemPoison
    {
        get { return PlayerPrefs.GetInt("ItemPoison", 3); }
        set { PlayerPrefs.SetInt("ItemPoison", value); }
    }

    public static int ItemFreeze
    {
        get { return PlayerPrefs.GetInt("ItemFreeze", 3); }
        set { PlayerPrefs.SetInt("ItemFreeze", value); }
    }



    public static int UpgradeStrongWall
    {
        get { return PlayerPrefs.GetInt("UpgradeStrongWall", 0); }
        set { PlayerPrefs.SetInt("UpgradeStrongWall", value); }
    }
    public static float StrongWallExtra
    {
        get { return PlayerPrefs.GetFloat("StrongWallExtra", 0); }
        set { PlayerPrefs.SetFloat("StrongWallExtra", value); }
    }
}


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
        dict.Add(key, value);
        LifeTTRv2 = dict;
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
        dict.Remove(key);
        LifeTTRv2 = dict;
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
