using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UnityAdsitem : MonoBehaviour
{
    public GameObject but;
    public Text rewardedTxt;

    private void Update()
    {
        //but.SetActive(GameMode.Instance && GameMode.Instance.isRewardedAdReady());
        but.SetActive(AdsManager.Instance && AdsManager.Instance.isRewardedAdReady());
        if (AdsManager.Instance)
        {
            if (!AdsManager.Instance.isRewardedAdReady())
                rewardedTxt.text = "NO AD AVAILABLE NOW!";
            else
                rewardedTxt.text = "+" + AdsManager.Instance.getRewarded;
        }else
            rewardedTxt.text = "NO AD AVAILABLE NOW!";
    }

    public void WatchVideoAd()
    {
        if (AdsManager.Instance)
        {
            SoundManager.Click();
            AdsManager.AdResult += AdsManager_AdResult;
            AdsManager.Instance.ShowRewardedAds();
        }
    }

    private void AdsManager_AdResult(bool isSuccess, int rewarded)
    {
        AdsManager.AdResult -= AdsManager_AdResult;
        if (isSuccess)
        {
            GlobalValue.SavedCoins += rewarded;
            SoundManager.PlaySfx(SoundManager.Instance.soundPurchased);
        }
    }
}
