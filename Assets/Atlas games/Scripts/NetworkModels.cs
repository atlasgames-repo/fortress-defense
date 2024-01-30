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
                if (!string.IsNullOrEmpty(item.Value) || !APIManager.instance.IS_DEBUG)
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
    public int coin, gem, uxp, rxp, rxpTotal;

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
        gem = _gem;
        uxp = _uxp;
        coin = _coin;
    }

}
[Serializable]
public class Message : BaseModel
{
    public string server = "";
    public string auth = "";

}

public static class NetworkStatusError
{
    public static readonly string TOKEN_LOGIN_FAIL = "ابتدا باید وارد شوید";
    public static readonly string LOGIN_FAIL = "یوزرنیم یا پسوردو اشتباه وارد کردی، حواس پرت -__-";
    public static readonly string UNKNOWN_ERROR = "مشکلی پیش اومده دوباره امتحان کن :(";
    public static readonly string COULDNT_GET_UPDATES = "آپدیت ها پیدا نشد، دوباره تلاش کن، باشه ؟";
    public static readonly string SUCCESSFUL_LOGIN = "ورود موفقیت آمیز، الان بازی لود میشه.";
    public static readonly string FAIL_LOGIN = "مشکلی در ورود پیش اومده دوباره امتحان کن. :/";
    public static readonly string USER_UPDATE_FAIL = "نتونستیم اطلاعتت رو آپدیت کنیم، گر صبر کنی یه ترشی حلوا سازی.";

}
public class NetworkModels : BaseModel
{

}