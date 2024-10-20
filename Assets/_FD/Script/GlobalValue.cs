﻿using UnityEngine;
using System;
public class GlobalValue : MonoBehaviour
{
    public static bool isFirstOpenMainMenu = true;
    public static int worldPlaying = 1;
    public static int levelPlaying = 1;
    public static Level.LeveType levelType = Level.LeveType.MISSION;
    //public static int finishGameAtLevel = 50;

    public static string WorldReached = "WorldReached";
    public static bool isSound = true;
    public static bool isMusic = true;

    public static void Debug(object message)
    {
        var textobj = GameObject.FindGameObjectWithTag("DebugText");
        if (textobj == null)
        {
            UnityEngine.Debug.LogError($"[{DateTime.Now}]: {message}\n");
            return;
        }
        textobj.TryGetComponent(out UnityEngine.UI.Text text);
        if (text != null)
            text.text += $"[{DateTime.Now}]: {message}\n";
        else
            UnityEngine.Debug.LogError($"[{DateTime.Now}]: {message}\n");
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
    public static int GameTutorialOpened
    {
        get { return PlayerPrefs.GetInt("GameTutorialOpened", 0); }
        set { PlayerPrefs.SetInt("GameTutorialOpened", value); }
    }
    // Achievements
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
    public static int KillCount
    {
        get { return PlayerPrefs.GetInt("killcount", 0); }
        set { PlayerPrefs.SetInt("killcount", value); }
    }
    public static int UserGold
    {
        get { return User.Coin; }
    }
    public static int BoughtGroundUnit
    {
        get { return PlayerPrefs.GetInt("BoughtGroundUnit", 0); }
        set { PlayerPrefs.SetInt("BoughtGroundUnit", value); }
    }
    public static int PoisionUsage
    {
        get { return PlayerPrefs.GetInt("PoisionUsage", 0); }
        set { PlayerPrefs.SetInt("PoisionUsage", value); }
    }
    public static int FreezeUsage
    {
        get { return PlayerPrefs.GetInt("FreezeUsage", 0); }
        set { PlayerPrefs.SetInt("FreezeUsage", value); }
    }
    public static int LightningUsage
    {
        get { return PlayerPrefs.GetInt("LightningUsage", 0); }
        set { PlayerPrefs.SetInt("LightningUsage", value); }
    }

    public static int GameStartTimerMinutes
    {
        get
        {
            string strDateTime = PlayerPrefs.GetString("GameStartTimerMinutes", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            return (DateTime.Now - DateTime.ParseExact(strDateTime, "yyyy-MM-dd HH:mm:ss", null)).Minutes;
        }
        set { PlayerPrefs.SetString("GameStartTimerMinutes", DateTime.Now.AddMinutes(value).ToString("yyyy-MM-dd HH:mm:ss")); }
    }
}
