using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class User : MonoBehaviour
{
    private delegate void callback(UserResponse response);
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
            Update_User(new UserUpdate(_gem: UserProfile.gem + value));
        }
    }
    public static int Uxp
    {
        get { return UserProfile.uxp; }
        set
        {
            Update_User(new UserUpdate(_uxp: UserProfile.uxp + value));
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
        UserResponse response = await APIManager.instance.UpdateUser(user);
        UserProfile = response;
    }
    public static void Get_User_Eeventually()
    {
        Task task = Task.Run(() => Get_user());
    }
    public static async Task<UserResponse> Get_user()
    {
        UserResponse response = await APIManager.instance.UpdateUser(new UserUpdate());
        UserProfile = response;
        return response;
    }

}
