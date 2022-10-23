using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using Newtonsoft.Json;

public class BaseModel
{
    public string ToJson
    {
        get { return JsonUtility.ToJson(this); }
    }

    public string ToParams
    {
        get
        {
            string param = "?";
            Dictionary<string, string> dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(this.ToJson);
            foreach (KeyValuePair<string, string> item in dict)
            {
                param += $"{item.Key}={item.Value}&";
            }
            return param;
        }
    }

}
[Serializable]
public class AssetBundleUpdateResponse : BaseModel
{
    public string[] list;

}

[Serializable]
public class AssetBundleGenerator
{
    public string name;
    public Transform root;
    public bool is_root_world = false, has_depends = false;
    public MonoBehaviour depends_type;
}
[Serializable]
public class ErrorResponse : BaseModel
{
    public string message;
    public ErrorResponse(string _message)
    {
        message = _message;
    }
}

[Serializable]
public class AuthenticationResponse : BaseModel
{
    public string token;
    public string message;
    public bool result;
}
[Serializable]
public class Authentication : BaseModel
{
    public string username = null;
    public string password = null;

}
[Serializable]
public class UserResponse : BaseModel
{
    public string first_name, last_name, display_name, email, username, registered_date, avatar;
    public int gold;

}
[Serializable]
public class Message : BaseModel
{
    public string server = "";
    public string auth = "";

}


public class NetworkModels : BaseModel
{

}