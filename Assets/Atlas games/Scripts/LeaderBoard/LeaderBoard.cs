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
    public string url = "https://run.mocky.io/v3/4f50ac8a-812c-4dd1-9a79-bf36e1f3ba12";
    public GameObject errorText;
    public GameObject loadingText;
    private LeaderboardData[] _dataArray;
    public LeaderboardList listOfUsers;
    public InfiniteScroll scroll;

    public void LoadLeaderboard()
    {
        StartCoroutine(GetData());
    }

    [Serializable]
    public class LeaderboardData
    {
        // data for leader board that has to change based on postman.
        public int user_id;
        public string user_name;
        public string user_first_name;
        public string user_last_name;
        public string user_link;
        public string user_avatar;
        public int points;
        public int rank;
        //public string imageUrl;
        // public int rxp;
    }

    string fixJson(string value)
    {
        value = "{\"Items\":" + value + "}";
        return value;
    }
    // get data from the API mocked for the leader board list.
    IEnumerator GetData()
    {
        scroll.RefreshViews();
        scroll.gameObject.SetActive(false);
        errorText.SetActive(false);
        loadingText.SetActive(true);
        Get_user();
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                
                string json = webRequest.downloadHandler.text;
                json = fixJson(json);
//                _dataArray = JsonConvert.DeserializeObject<LeaderboardData[]>(json);
                // the next line is not necessary ...
          //      Array.Sort(_dataArray, (x, y) => y.rxp.CompareTo(x.rxp));
                listOfUsers.data = _dataArray;
                scroll.gameObject.SetActive(true);
                scroll.OnFill += OnFillItem;
                scroll.OnHeight += OnHeightItem;
                scroll.InitData(listOfUsers.data.Length);
                
            }
            else
            {
                errorText.SetActive(true);
            }
        }
        loadingText.SetActive(false);
    }

    int OnHeightItem(int index)
    {
        return 150;
    }

    void OnFillItem(int index, GameObject item)
    {
        LeaderboardData data = listOfUsers.data[index];
        // initialize the list item components.
        item.GetComponent<LeaderboardListItem>().SetData(data);
    }

    public void ClearList()
    {
        scroll.RefreshViews();
    }
    public static async void Get_user()
    {
        print("hello");
        
        LeaderBoardResponseModel[] leaderboardResponse = await APIManager.instance.Get_leader_board();
        print(leaderboardResponse);
    }
}
