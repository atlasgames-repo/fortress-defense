using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ANDROID || UNITY_IOS
using GoogleMobileAds.Api;
#endif
using System;

public class AdmobController : MonoBehaviour
{
    public static AdmobController Instance;
    //delegate   ()
    public delegate void RewardedAdResult(bool isWatched);

    //event  
    public static event RewardedAdResult AdResult;

    public bool useBanner = false;
#if UNITY_ANDROID || UNITY_IOS 
    public AdPosition bannerPosition = AdPosition.Bottom;
#endif
    //public bool useInterstitial = true;

    [Header("ANDROID")]
    public string androidID;
    public string androidBanner;
    public string androidInters;
    public string androidVideo;

    [Header("IOS")]
    public string iosID;
    public string iosBanner;
    public string iosInters;
    public string iosVideo;
#if UNITY_ANDROID || UNITY_IOS
    private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;
#endif

    private void Awake()
    {
        if (AdmobController.Instance != null)
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
#if UNITY_ANDROID
        string appId = androidID;
#elif UNITY_IPHONE
                    string appId = iosID;
#else
                    string appId = "unexpected_platform";
#endif
#if UNITY_ANDROID || UNITY_IOS
        //Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);
        MobileAds.Initialize(initStatus => { });
        if (useBanner)
            RequestBanner();
        RequestInterstitial();
        RequestRewardedVideo();
#endif
    }

    #region BANNER

    private void RequestBanner()
    {
#if UNITY_ANDROID
        string appId = androidBanner;
#elif UNITY_IPHONE
        string appId = iosBanner;
#else
        string appId = "unexpected_platform";
#endif

#if UNITY_ANDROID || UNITY_IOS
        // Create a 320x50 banner at the top of the screen.
        bannerView = new BannerView(appId, AdSize.SmartBanner, bannerPosition);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        bannerView.LoadAd(request);
        // Called when an ad request has successfully loaded.
        bannerView.OnAdLoaded += HandleOnAdLoaded;
#endif
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        ShowBanner(true);
    }

    public void ShowBanner(bool show)
    {
#if UNITY_ANDROID || UNITY_IOS
        if (show)
            bannerView.Show();
        else
            bannerView.Hide();
#endif
    }

    #endregion

    #region INTERSTITIAL
    private void RequestInterstitial()
    {
#if UNITY_ANDROID
        string appId = androidInters;
#elif UNITY_IPHONE
            string appId = iosInters;
#else
            string appId = "unexpected_platform";
#endif
#if UNITY_ANDROID || UNITY_IOS
        // Initialize an InterstitialAd.
        interstitial = new InterstitialAd(appId);

        interstitial.OnAdOpening += HandleOnAdOpening;
        interstitial.OnAdClosed += HandleOnAdClosed;

        LoadInterstitial();
        // Create an empty ad request.
        //AdRequest request = new AdRequest.Builder().Build();
        //// Load the interstitial with the request.
        //this.interstitial.LoadAd(request);
#endif
    }

    public void LoadInterstitial()
    {
#if UNITY_ANDROID || UNITY_IOS
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        interstitial.LoadAd(request);
#endif
    }

    public void HandleOnAdOpening(object sender, EventArgs args)
    {
        GameManager.Instance.isWatchingAd = true;
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        LoadInterstitial();
        GameManager.Instance.isWatchingAd = false;
    }


    public bool isInterstitialAdReady()
    {
#if UNITY_ANDROID || UNITY_IOS
        return interstitial.IsLoaded();
#else
        return false;
#endif
    }

    public bool ForceShowInterstitialAd()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
            return true;
        }
        else
            return false;

#else
        return false;
#endif
    }

    #endregion

    #region REWARDED VIDEO AD

    public bool isRewardedVideoAdReady()
    {
#if UNITY_ANDROID || UNITY_IOS
        return this.rewardedAd.IsLoaded();
#else
        return false;
#endif
    }

    public void WatchRewardedVideoAd()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
        }
#endif
    }

    private void RequestRewardedVideo()
    {
#if UNITY_ANDROID
        string appId = androidVideo;
#elif UNITY_IPHONE
            string appId = iosVideo;
#else
            string appId = "unexpected_platform";
#endif
#if UNITY_ANDROID || UNITY_IOS
        // Initialize an InterstitialAd.
        this.rewardedAd = new RewardedAd(appId);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);

        rewardedAd.OnAdOpening += HandleVideoOnAdOpening;
        rewardedAd.OnAdClosed += HandleVideoOnAdClosed;
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;

        LoadInterstitial();
        // Create an empty ad request.
        //AdRequest request = new AdRequest.Builder().Build();
        //// Load the interstitial with the request.
        //this.interstitial.LoadAd(request);
#endif
    }

#if UNITY_ANDROID || UNITY_IOS
    private void HandleUserEarnedReward(object sender, Reward e)
    {
        AdResult(true);
    }

    private void HandleVideoOnAdClosed(object sender, EventArgs e)
    {
        RequestRewardedVideo();
    }

    private void HandleVideoOnAdOpening(object sender, EventArgs e)
    {
    }
#endif
    #endregion
}
