using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine.Events;

public class User : MonoBehaviour
{
    public static UnityEvent UserUpdateEvent; 
    public static UserResponse UserProfile
    {
        get { return JsonUtility.FromJson<UserResponse>(PlayerPrefs.GetString("user", "{}")); }
        set { PlayerPrefs.SetString("user", JsonUtility.ToJson(value)); }
    }
    public static string Token
    {
        get { return PlayerPrefs.GetString("token", null); }
        set { PlayerPrefs.SetString("token", value); }
    }
    public static int Gem
    {
        get { return UserProfile.gem; }
        set
        {
            Update_User(new UserUpdate(_gem: value));
        }
    }
    public static int Coin
    {
        get { return UserProfile.coin; }
        set
        {
            Update_User(new UserUpdate(_coin: value));
        }
    }

    public static int Uxp
    {
        get { return UserProfile.uxp; }
        set
        {
            Update_User(new UserUpdate(_uxp: value));
        }
    }
    public static int Level
    {
        get { return (int)Mathf.Pow(UserProfile.uxp, 0.5f); }
    }
    public static float Next_Level_Progression
    {
        get
        {
            float next_level = Level + 1f;
            float this_level_uxp = Mathf.Pow(Level, 2f);
            float next_level_uxp = Mathf.Pow(next_level, 2);
            return (UserProfile.uxp - this_level_uxp) / (next_level_uxp - this_level_uxp);
        }
    }
    private static async void Update_User(UserUpdate user)
    {
        await APIManager.instance.UpdateUser(user);
        Get_User_Eeventually();
    }
    public static void Get_User_Eeventually()
    {
        Get_user();
    }
    public static async void Get_user()
    {
        UserResponse response = await APIManager.instance.Check_token();
        UserProfile = response;
    }

}
