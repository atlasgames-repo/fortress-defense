using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public enum WatchAdResult { Finished, Failed, Skipped}
public class UnityAds : MonoBehaviour
{
    //delegate   ()
    public delegate void RewardedAdResult(WatchAdResult result);

    //event  
    public static event RewardedAdResult AdResult;

    public static UnityAds Instance;

    [Header("UNITY AD SETUP")]
    public string UNITY_ANDROID_ID = "1486550";
    public string UNITY_IOS_ID = "1486551";
    public bool isTestMode = true;

    private void Awake()
    {
        if (UnityAds.Instance != null)
        {
            Destroy(gameObject);
            return; 
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        string gameId = "";
#if UNITY_IOS
		gameId = UNITY_IOS_ID;
#elif UNITY_ANDROID
        gameId = UNITY_ANDROID_ID;
#endif
        if (Advertisement.isSupported)
        {
            Advertisement.Initialize(gameId, isTestMode);
        }
    }



    #region NORMAL AD
    public void ShowNormalAd()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
    }

    public bool ForceShowNormalAd()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
            return true;
        }
        else
            return false;
    }

    #endregion

    #region REWARD AD
    public bool isRewardedAdReady()
    {
        return Advertisement.IsReady("rewardedVideo");
    }

    public void ShowRewardVideo()
    {
        ShowRewardedAd();
    }

    private void ShowRewardedAd()
    {
        if (!allowWatch)
            return;

        if (Advertisement.IsReady("rewardedVideo"))
        {
                var options = new ShowOptions { resultCallback = HandleShowResult };
                if (!Advertisement.isShowing)
                    Advertisement.Show("rewardedVideo", options);

                allowWatch = false;
            
        }
    }

    bool allowWatch = true;
    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                AdResult(WatchAdResult.Finished);
                ; break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                AdResult(WatchAdResult.Skipped);
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                AdResult(WatchAdResult.Failed);
                break;
        }

        allowWatch = true;
    }
    #endregion
}
