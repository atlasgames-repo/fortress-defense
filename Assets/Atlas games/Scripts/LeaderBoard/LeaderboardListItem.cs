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
    public void SetData(LeaderboardData data)
    {
       rxpText.text = data.rank.ToString();
       userNameText.text = data.user_name;
       SetImage(data.user_avatar);
    }
    
    async void SetImage(string url)
    {
        
    }
}
