using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class ShopItemReward : MonoBehaviour {
    public string itemName = "ITEM NAME";
	public enum ItemType{DoubleArrow, Posion, Freeze}
	public ItemType itemType;

	public int rewardedUnit = 1;

    public Text nameTxt;
	public Text rewardedAmountTxt;
	public Text currentAmountTxt;
	//public AudioClip sound;

	[ReadOnly] public int coinPrice = 1;
	public Text coinTxt;
	//public GameObject watchVideoBut;
	//ShowOptions options;

	void OnEnable(){
		UpdateAmount ();
	}

	void Start(){
        if (GameMode.Instance)
        {
            switch (itemType)
            {
                case ItemType.DoubleArrow:
                    coinPrice = GameMode.Instance.doubleArrowPrice;
                    break;
                case ItemType.Posion:
                    coinPrice = GameMode.Instance.poisonArrowPrice;
                    break;
                case ItemType.Freeze:
                    coinPrice = GameMode.Instance.freezeArrowPrice;
                    break;
                default:
                    break;
            }
        }

        UpdateAmount ();

		rewardedAmountTxt.text = "x" + rewardedUnit;
		coinTxt.text = coinPrice.ToString ();
        nameTxt.text = itemName;
        //options = new ShowOptions { resultCallback = HandleShowResult2 };
    }

	public void UseCoin(){
		var coins = GlobalValue.SavedCoins;
        if (coins >= coinPrice)
        {
            coins -= coinPrice;
            GlobalValue.SavedCoins = coins;

            DoReward();
        }
        else
        {
            SoundManager.PlaySfx(SoundManager.Instance.soundNotEnoughCoin);
            if (AdsManager.Instance && AdsManager.Instance.isRewardedAdReady())
                NotEnoughCoins.Instance.ShowUp();
        }
	}

    //public void ShowRewardAd()
    //{
    //    if (Advertisement.IsReady("rewardedVideo"))
    //    {
    //        Advertisement.Show("rewardedVideo", options);
    //    }
    //    SoundManager.Click();
    //}
	
	//private void ShowRewardedAd()
	//{

	//	if (Advertisement.IsReady ("rewardedVideo")) {
	//		Advertisement.Show ("rewardedVideo", options);
	//	}
	//}

	//private void HandleShowResult2(ShowResult result)
	//{
	//	switch (result) {
	//	case ShowResult.Finished:
	//		Debug.Log ("The ad was successfully shown.");
	//		DoReward ();
	//		break;
	//	case ShowResult.Skipped:
	//		Debug.Log ("The ad was skipped before reaching the end.");
	//		break;
	//	case ShowResult.Failed:
	//		Debug.LogError ("The ad failed to be shown.");
	//		break;
	//	}
	//}

	private void DoReward(){
        switch (itemType)
        {
            case ItemType.DoubleArrow:
                GlobalValue.ItemDoubleArrow += rewardedUnit;
                break;
            case ItemType.Posion:
                GlobalValue.ItemPoison += rewardedUnit;
                break;
            case ItemType.Freeze:
                GlobalValue.ItemFreeze += rewardedUnit;
                break;
            default:
                break;
        }

		UpdateAmount ();
        SoundManager.PlaySfx(SoundManager.Instance.soundPurchased);
		//Debug.LogWarning ("ITEM: " + sound.name);
		//SoundManager.PlaySfx (sound);
	}

    private void UpdateAmount()
    {
        switch (itemType)
        {
            case ItemType.DoubleArrow:
                currentAmountTxt.text = "current: " + GlobalValue.ItemDoubleArrow;
                break;
            case ItemType.Posion:
                currentAmountTxt.text = "current: " + GlobalValue.ItemPoison;
                break;
            case ItemType.Freeze:
                currentAmountTxt.text = "current: " + GlobalValue.ItemFreeze;
                break;
            default:
                break;
        }
    }
}
