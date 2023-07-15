using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class BasePlayerPrefs<T> : MonoBehaviour
{
    public static string PrefsName;
    // Start is called before the first frame update
    public static void Add(string index, T value)
    {
        Dictionary<string, T> dict = Dict;
        if (!dict.ContainsKey(index))
        {
            dict.Add(index, value);
            Dict = dict;
        }
    }
    public static void Update(string index, T value)
    {
            bool is_exist = TryGetValue(index, out T outvalue);
            if (is_exist)
            {
                Remove(index);
                Add(index,value);
            }
    }
    public static void Remove(string index)
    {
            var dict = Dict;
            dict.Remove(index);
            Dict = dict;
    }
    public static string Json
    {
        get { return JsonConvert.SerializeObject(Dict, Formatting.Indented); }
    }
    public static string[] Keys
    {
        get
        {
            Dictionary<string, T> dict = Dict;
            return dict.Keys.ToArray();
        }
    }
    public static bool TryGetValue(string index, out T value)
    {
        Dictionary<string, T> dict = Dict;
        dict.TryGetValue(index, out T found_value);
        if (found_value != null) { value = found_value; return true; }
        else { value = default; return false; }
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override bool Equals(object other)
    {
        return base.Equals(other);
    }

    public override string ToString()
    {
        return base.ToString();
    }

    public static T[] DictArray
    {
        get
        {
            Dictionary<string, T> dict = Dict;
            return dict.Values.ToArray();
        }
    }
    public static Dictionary<string, T> Dict
    {
        get { return JsonConvert.DeserializeObject<Dictionary<string, T>>(PlayerPrefs.GetString(typeof(T).Name,"{}")); }
        set { PlayerPrefs.SetString(typeof(T).Name, JsonConvert.SerializeObject(value)); }
    }
}
