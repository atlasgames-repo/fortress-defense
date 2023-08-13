using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ChildInParent : MonoBehaviour
{
    public static Transform GetChild(Transform parent, string route){
        int[] routes = Route(route,',');
        Transform child = parent.GetChild(routes[0]);
        for (int i = 1; i < routes.Length; i++)
        {
            child = child.GetChild(routes[i]);
        }
        return child;
    }
    static int[] Route(string route, char selector){
        string[] data = route.Split(selector);
        int[] result = new int[0];
        for (int i = 0; i < data.Length; i++)
        {
            Array.Resize(ref result,result.Length + 1);
            try
            {    
                result[result.Length - 1] = Convert.ToInt32(data[i]);
            }
            catch (System.Exception)
            {
                result[result.Length - 1] = 0;
            }
        }
        return result;
    }
}
