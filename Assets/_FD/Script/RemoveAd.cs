using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoveAd : MonoBehaviour
{
    public Text priceTxt;
    public Text rewardedTxt;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(GameMode.Instance && !GlobalValue.RemoveAds);
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalValue.RemoveAds)
        {

            priceTxt.text = "$" + GameMode.Instance.purchase.removeAdsPrice;
            Debug.LogWarning("Ads Remove");
            gameObject.SetActive(false);
        }
    }

    public void Buy()
    {
        SoundManager.Click();
        GameMode.Instance.BuyRemoveAds();
    }
}
