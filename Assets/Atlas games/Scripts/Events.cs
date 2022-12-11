using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Events
{

    public string name;
    public int price;
    [ReadOnly] public int world = 0;
    public int level;
    private string _id;
    public string ID { get { return _id; } }
    [ReadOnly] public bool is_passed;

    public Events()
    {

    }
    public Events(string id)
    {
        _id = id;
    }
}
