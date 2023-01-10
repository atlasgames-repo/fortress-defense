using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using System.Threading;
using System;

public class ProfileManager : MonoBehaviour
{
    public Image avatar;
    public TextMeshProUGUI username, gold, life, gem;


    public void logout()
    {
        User.Token = "";
        APIManager.instance.LoadAsynchronously("Login");
    }

    public void fetchData()
    {
        fetchData(User.UserProfile);
    }
    public async void fetchData(UserResponse user)
    {
        username.text = user.display_name;
        life.text = LifeTTRSource.Life.ToString();
        gold.text = $"{GlobalValue.SavedCoins}";
        gem.text = $"{user.gem}";
        avatar.sprite = await APIManager.instance.get_rofile_picture(user.avatar);
    }
}
