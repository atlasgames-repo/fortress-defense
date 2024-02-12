using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LeaderboardListItem : MonoBehaviour
{
    public TMP_Text userNameText;
    public TMP_Text rxpText;
    public Image image;
    // this function sets the data recived from leaderboard.cs to the list item component 
    public void SetData(LeaderBoard.LeaderboardData data)
    {
        rxpText.text = data.rxp.ToString();
        userNameText.text = data.userName;
        SetImage(data.imageUrl);
    }
    
    async void SetImage(string url)
    {
        image.sprite = await APIManager.instance.Get_rofile_picture(url);
    }
}
