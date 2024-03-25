using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LeaderboardListItem : MonoBehaviour
{
    public TMP_Text userNameText;
    public TMP_Text rxpText;
    public TMP_Text rank_text;
    public Image image;
    // this function sets the data recived from leaderboard.cs to the list item component 
    public async void SetData(LeaderboardData data)
    {
        rxpText.text = data.points.ToString();
        rank_text.text = data.rank.ToString();
        userNameText.text = data.user_name;
        image.sprite = await APIManager.instance.Get_rofile_picture(data.user_avatar);
    }
}
