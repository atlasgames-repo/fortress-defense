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
    public string[] list = new string[0];

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
    public int gem, uxp;

}
[Serializable]
public class UserUpdate : BaseModel
{
    public string token = "null";
    public int gem = User.UserProfile.gem, uxp = User.UserProfile.uxp;
    public UserUpdate()
    {

    }
    public UserUpdate(string _token, int _gem = 0, int _uxp = 0)
    {
        token = _token;
        gem = _gem == 0 ? User.UserProfile.gem : _gem;
        uxp = _uxp == 0 ? User.UserProfile.uxp : _uxp; ;
    }
    public UserUpdate(int _gem = 0, int _uxp = 0)
    {
        token = User.Token;
        gem = _gem == 0 ? User.UserProfile.gem : _gem;
        uxp = _uxp == 0 ? User.UserProfile.uxp : _uxp;
    }

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