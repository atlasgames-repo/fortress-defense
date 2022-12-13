using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;


public class _Event : MonoBehaviour
{
    public static int CurrentEventPlaying = 0;

    public static Events GetCurrentEvent
    {
        get
        {
            
            var dict = _eventPrefs;
            bool is_event = dict.TryGetValue(CurrentEventPlaying, out Events events);
            if (is_event)
                return events;
            return null;
        }
    }
    public static Events GetEvent(int key)
    {
        var dict = _eventPrefs;
        bool is_event = dict.TryGetValue(key, out Events events);
        if (is_event)
            return events;
        return null;
    }
    public static void Add(int key, Events value)
    {
        var dict = _eventPrefs;
        if (!dict.ContainsKey(key))
        {
            dict.Add(key, value);
            _eventPrefs = dict;
        }
    }
    public static void Remove(int key)
    {
        var dict = _eventPrefs;
        dict.Remove(key);
        _eventPrefs = dict;
    }
    public static void CompleteLevel()
    {
        var dict = _eventPrefs;
        bool is_event = dict.TryGetValue(CurrentEventPlaying, out Events events);
        if (is_event)
            events.is_passed = true;
        if (events != null)
        {
            dict.Remove(CurrentEventPlaying);
            dict.Add(CurrentEventPlaying, events);
            _eventPrefs = dict;
        }

    }
    public static string Json
    {
        get { return JsonConvert.SerializeObject(_eventPrefs, Formatting.Indented); }
    }
    public static Dictionary<int, Events> _eventPrefs
    {
        get { return JsonConvert.DeserializeObject<Dictionary<int, Events>>(PlayerPrefs.GetString("events", "{}")); }
        set { PlayerPrefs.SetString("events", JsonConvert.SerializeObject(value)); }
    }

}

