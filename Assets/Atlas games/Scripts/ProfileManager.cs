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
    public TextMeshProUGUI username, email, gold, gem;


    public void logout()
    {
        GlobalValue.token = "";
        APIManager.instance.LoadAsynchronously("Login");
    }

    // Start is called before the first frame update
    void Start()
    {
        //fetchData(GlobalValue.user);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void fetchData()
    {
        fetchData(GlobalValue.user);
    }
    public async void fetchData(UserResponse user)
    {
        username.text = user.display_name;
        email.text = user.email;
        gold.text = $"{GlobalValue.SavedCoins}";
        gem.text = $"{user.gem}";
        avatar.sprite = await APIManager.instance.get_rofile_picture(user.avatar);
    }
}
