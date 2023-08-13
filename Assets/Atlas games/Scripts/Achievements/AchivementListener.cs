using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class AchivementListener : MonoBehaviour
{
    public static AchivementListener self;
    public float InitialDelaySeconds = 5;
    public float ListenerTickSeconds = 2;

    void Awake()
    {
        if (self == null)
        {
            self = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


        StartCoroutine(Listener());
    }

    IEnumerator Listener()
    {
        // Get all classes implementing the interface
        IEnumerable<IAchievement> classes = InterfaceReflection.GetClassesImplementingInterface<IAchievement>();
        yield return new WaitForSeconds(InitialDelaySeconds);
        while (true)
        {
            yield return new WaitForSeconds(ListenerTickSeconds);
            foreach (IAchievement item in classes)
            {
                Type type = item.GetType();
                bool check = (bool)type.GetProperty("Check", BindingFlags.Public | BindingFlags.Static).GetValue(null);
                string Id = (string)type.GetField("ID", BindingFlags.Public | BindingFlags.Static).GetValue(null);
                if (check)
                {
                    Trophy.STATUS_UP = Id;
                    // TODO: Give the price to the user
                }
            }
        }
    }

}

