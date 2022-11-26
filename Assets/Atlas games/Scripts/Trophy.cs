using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using Newtonsoft.Json;
using System.Linq;
using UnityEngine.Events;

public class Trophy : MonoBehaviour
{
    public static Trophy self;
    public Dictionary<string, _Trophy> Trophies = new Dictionary<string, _Trophy>();
    public _Trophy[] _Trophies;
    void Awake()
    {
        self = this;
        foreach (var item in _Trophies)
        {
            _Trophy trphy = item;
            if (Achived.ContainsKey(item._id))
                trphy.is_achived = Achived[item._id];
            else
                Add_Achived(trphy._id, false);
            if (Served.ContainsKey(item._id))
                trphy.is_served = Served[item._id];
            else
                Add_Served(trphy._id, false);
            Trophies.Add(item._id, trphy);
        }
    }
    public static void Add(string key, _Trophy value)
    {
        if (!self.Trophies.ContainsKey(key))
        {
            self.Trophies.Add(key, value);
        }
    }
    void Add_Achived(string key, bool value)
    {
        var dict = Achived;
        if (!dict.ContainsKey(key))
        {
            dict.Add(key, value);
            Achived = dict;

        }
    }
    void Add_Served(string key, bool value)
    {
        var dict = Served;
        if (!dict.ContainsKey(key))
        {
            dict.Add(key, value);
            Served = dict;
        }
    }
    public static void Achive(string key)
    {
        var dict = Achived;
        if (!dict.ContainsKey(key))
        {
            dict.Add(key, true);
            Achived = dict;
        }
        else
        {
            dict[key] = true;
            Achived = dict;
        }
    }
    public static void Serve(string key)
    {
        var dict = Served;
        if (!dict.ContainsKey(key))
        {
            dict.Add(key, true);
            Served = dict;
        }
        else
        {
            dict[key] = true;
            Served = dict;
        }
    }
    public static void Remove(string key)
    {
        if (!self.Trophies.ContainsKey(key))
        {
            self.Trophies.Remove(key);
        }
    }
    public static string Json
    {
        get { return JsonConvert.SerializeObject(self.Trophies, Formatting.Indented); }
    }
    static Dictionary<string, bool> Achived
    {
        get { return JsonConvert.DeserializeObject<Dictionary<string, bool>>(PlayerPrefs.GetString("TrophyAchived", "{}")); }
        set { PlayerPrefs.SetString("TrophyAchived", JsonConvert.SerializeObject(value)); }
    }
    static Dictionary<string, bool> Served
    {
        get { return JsonConvert.DeserializeObject<Dictionary<string, bool>>(PlayerPrefs.GetString("TrophyServed", "{}")); }
        set { PlayerPrefs.SetString("TrophyServed", JsonConvert.SerializeObject(value)); }
    }

}


[Serializable]
public class _Trophy
{
    public string name = "null";
    public string _id = "null";
    public Sprite image = null;
    public string details = "null";
    [ReadOnly] public bool is_achived = false;
    [ReadOnly] public bool is_served = false;


}

