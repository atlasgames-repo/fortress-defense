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
    public GameObject Number_one_rank;
    public Image image;
    public Sprite _default;
    // this function sets the data recived from leaderboard.cs to the list item component 
    public async void SetData(LeaderboardData data, bool show_avatar = true)
    {
        if (data.rank == 1 && Number_one_rank) Number_one_rank.SetActive(true);
        rxpText.text = data.total.ToString();
        rank_text.text = data.rank.ToString();
        userNameText.text = data.username;
        if (!show_avatar && !image) {
            return;
        };
        if (data.avatar_url != "")
            image.sprite = await APIManager.instance.Get_rofile_picture(data.avatar_url);
        else if (data.avatar != "")
            image.sprite = await APIManager.instance.Get_rofile_picture(data.avatar);
        else
            image.sprite = _default;
    }
}
