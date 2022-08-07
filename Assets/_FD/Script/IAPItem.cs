using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IAPItem : MonoBehaviour
{
    public int ID = 1;
    public Text priceTxt;
    public Text rewardedTxt;

    private void Start()
    {
#if UNITY_PURCHASING
        if (GameMode.Instance)
        {
            switch (ID)
            {
                case 1:
                    priceTxt.text = "$" + GameMode.Instance.purchase.price1;
                    rewardedTxt.text = "+" + GameMode.Instance.purchase.reward1 + "";
                    break;
                case 2:
                    priceTxt.text = "$" + GameMode.Instance.purchase.price2;
                    rewardedTxt.text = "+" + GameMode.Instance.purchase.reward2;
                    break;
                case 3:
                    priceTxt.text = "$" + GameMode.Instance.purchase.price3;
                    rewardedTxt.text = "+" + GameMode.Instance.purchase.reward3 + "";
                    break;
                default:
                    break;
            }
        }
#endif
    }

    public void Buy()
    {
        SoundManager.Click();
        GameMode.Instance.BuyItem(ID);
    }
}
