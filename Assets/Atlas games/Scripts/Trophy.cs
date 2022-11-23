using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using Newtonsoft.Json;
using System.Linq;

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
            Trophies.Add(item._id, trphy);
        }
    }
    public void Add(string key, _Trophy value)
    {
        if (!Trophies.ContainsKey(key))
        {
            Trophies.Add(key, value);
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
    public void Achive(string key)
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
        }
    }
    public void Remove(string key)
    {
        if (!Trophies.ContainsKey(key))
        {
            Trophies.Remove(key);
        }
    }
    public string Json
    {
        get { return JsonConvert.SerializeObject(Trophies, Formatting.Indented); }
    }
    static Dictionary<string, bool> Achived
    {
        get { return JsonConvert.DeserializeObject<Dictionary<string, bool>>(PlayerPrefs.GetString("TrophyAchived", "{}")); }
        set { PlayerPrefs.SetString("TrophyAchived", JsonConvert.SerializeObject(value)); }
    }

}


[Serializable]
public class _Trophy
{
    public string name = "null";
    public string _id = "null";
    public Sprite image = null;
    public bool is_achived = false;
}
