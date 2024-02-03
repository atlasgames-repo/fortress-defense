using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mopsicus.InfiniteScroll;
using Spine;
using UnityEngine;
using UnityEngine.Networking;

public class LeaderBoard : MonoBehaviour
{
    public string url = "https://run.mocky.io/v3/4f50ac8a-812c-4dd1-9a79-bf36e1f3ba12";
    public GameObject errorText;
    private LeaderboardData[] _dataArray;

    public InfiniteScroll scroll;

    // Start is called before the first frame update
    public void LoadLeaderboard()
    {
        StartCoroutine(GetData());
    }

    [Serializable]
    public class LeaderboardData
    {
        // data for leader board that has to change based on postman.
        public string userName;
        public string imageUrl;
        public int rxp;
    }

    // this is for converting a Json Array to array.
    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
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
        errorText.SetActive(false);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string json = webRequest.downloadHandler.text;
                json = fixJson(json);
                _dataArray = JsonHelper.FromJson<LeaderboardData>(json);
                Array.Sort(_dataArray, (x, y) => y.rxp.CompareTo(x.rxp));
                scroll.OnFill += OnFillItem;
                scroll.OnHeight += OnHeightItem;
                scroll.InitData(_dataArray.Length);
                
            }
            else
            {
                errorText.SetActive(true);
            }
        }
    }

    int OnHeightItem(int index)
    {
        return 150;
    }

    void OnFillItem(int index, GameObject item)
    {
        LeaderboardData data = _dataArray[index];
        // initialize the list item components.
        item.GetComponent<LeaderboardListItem>().SetData(data.rxp, data.userName, data.imageUrl);
    }

    public void ClearList()
    {
        scroll.RefreshViews();
    }
}