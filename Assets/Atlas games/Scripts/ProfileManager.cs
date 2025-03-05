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
    public TextMeshProUGUI username, gold, life, gem, level, uxp;
    public Slider Level;
    public float updateDelaySeconds = 60f;

    public void logout()
    {
        User.Token = "";
        APIManager.instance.LoadAsynchronously("Login");
    }
    public void Start()
    {
        StartCoroutine(UpdateCycle());
    }
    IEnumerator UpdateCycle()
    {
        while (true)
        {
            User.Get_User_Eeventually();
            yield return new WaitForSeconds(updateDelaySeconds);
            fetchData();
        }
    }
    public void fetchData()
    {
        fetchData(User.UserProfile);
    }
    public async void fetchData(UserResponse user)
    {
        if (user.name != "")
            username.text = user.name;
        else if (user.nickname != "")
            username.text = user.nickname;
        else if (user.email != "")
            username.text = user.email;
        else if (user.first_name != "" && user.last_name != "")
            username.text = $"{user.first_name}_{user.last_name}";
        else
            username.text = "user_287138";

        life.text = $"{LifeTTRSource.Life}/{LifeTTRSource.max_life}";
        gold.text = $"{User.Coin}";
        gem.text = $"{user.gem}";
        uxp.text = $"{user.uxp}";
        level.text = $"{User.Level}";
        Level.value = User.Next_Level_Progression;
        avatar.sprite = await APIManager.instance.Get_rofile_picture(user.avatar);
    }
}
