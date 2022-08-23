using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseModel
{
    public string ToJson
    {
        get { return JsonUtility.ToJson(this); }
    }

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
    public UserResponse user;
}
[Serializable]
public class Authentication : BaseModel
{
    public string username = null;
    public string password = null;
    public string token = null;

}
[Serializable]
public class UserResponse : BaseModel
{
    public string username;
    public string profile_url;
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