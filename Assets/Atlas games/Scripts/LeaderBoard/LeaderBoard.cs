using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mopsicus.InfiniteScroll;
using Newtonsoft.Json;
using Spine;
using UnityEngine;
using UnityEngine.Networking;

public class LeaderBoard : MonoBehaviour
{
    public GameObject errorText;
    public GameObject loadingText;
    public LeaderboardList listOfUsers;
    public  InfiniteScroll scroll;
    private LeaderBoardResponseModel _leaderboardResponse;
    public void LoadLeaderboard()
    {
        GetData();
    }


    public void GetData()
    {
        scroll.RefreshViews();
        scroll.gameObject.SetActive(false);
        errorText.SetActive(false);
        loadingText.SetActive(true);
        Get_user();
    }

     int OnHeightItem(int index)
    {
        return 150;
    }

     void OnFillItem(int index, GameObject item)
    {
        // This cach system dosn't actually work but due to short time left to release we ignore this for now
       LeaderboardData data = listOfUsers.data[index]; 
        item.GetComponent<LeaderboardListItem>().SetData(data);
    }

    public void ClearList()
    {
        scroll.RefreshViews();
    }
    public async void Get_user()
    {
        if (listOfUsers.data.Length > 0)ShowData();
        _leaderboardResponse = await APIManager.instance.Get_leader_board();
        listOfUsers.data = _leaderboardResponse.results;
        ShowData();
    }

    void ShowData()
    {
        loadingText.SetActive(false);
        scroll.gameObject.SetActive(true);
        scroll.OnFill += OnFillItem;
        scroll.OnHeight += OnHeightItem;
        scroll.InitData(listOfUsers.data.Length);
    }
}
