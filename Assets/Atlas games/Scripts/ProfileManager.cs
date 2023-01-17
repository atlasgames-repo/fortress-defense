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
    public TextMeshProUGUI username, gold, life, gem, uxp;
    public Slider Level;
    public float updateDelay = 1f;

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
            fetchData();
            yield return new WaitForSeconds(updateDelay);
        }
    }
    public void fetchData()
    {
        fetchData(User.UserProfile);
    }
    public async void fetchData(UserResponse user)
    {
        username.text = user.display_name;
        life.text = $"{LifeTTRSource.Life}/{LifeTTRSource.max_life}";
        gold.text = $"{GlobalValue.SavedCoins}";
        gem.text = $"{user.gem}";
        avatar.sprite = await APIManager.instance.get_rofile_picture(user.avatar);
        uxp.text = $"{User.Level}";
        Level.value = User.Next_Level_Progression;
    }
}
