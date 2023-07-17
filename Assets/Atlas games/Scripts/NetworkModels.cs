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
public class Data : BaseModel
{
    public int status;

}
[Serializable]
public class GemResponseModel : CommonErrorResponse
{
    public int gem, rank;
}
[Serializable]
public class GemRequestModel : BaseModel
{
    public string amount = null;
    public string game_id = "442";
    public string date = null;
    public GemRequestModel() { }
    public GemRequestModel(string _game_id = null, string _amount = null, string _date = null)
    {
        amount = _amount;
        game_id = _game_id == null ? _game_id : "442";
        date = _date;
    }
}
[Serializable]
public class AuthenticationResponse : CommonErrorResponse
{
    public string token;
}
[Serializable]
public class CommonErrorResponse : BaseModel
{
    public string message;
    public string code;
    public bool result;
    public Data data;
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
    public int coin, gem, uxp;

}
[Serializable]
public class UserUpdate : BaseModel
{
    public int gem = User.UserProfile.gem, uxp = User.UserProfile.uxp, coin = User.UserProfile.coin;
    public UserUpdate()
    {
    }
    public UserUpdate(int _gem = 0, int _uxp = 0, int _coin = 0)
    {
        gem = _gem == 0 ? User.UserProfile.gem : _gem;
        uxp = _uxp == 0 ? User.UserProfile.uxp : _uxp;
        coin = _coin == 0 ? User.UserProfile.coin : _coin;
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