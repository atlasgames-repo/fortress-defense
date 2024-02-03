using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LeaderboardListItem : MonoBehaviour
{
    public TMP_Text userNameText;
    public TMP_Text rxpText;
    public DownloadImage image;
    // this function sets the data recived from leaderboard.cs to the list item component 
    public void SetData(int rxp, string userName,string imageUrl)
    {
        rxpText.text = rxp.ToString();
        userNameText.text = userName;
        image.Initialize(imageUrl);
    }
}
