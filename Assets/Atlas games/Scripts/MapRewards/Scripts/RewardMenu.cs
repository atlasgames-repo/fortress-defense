using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardMenu : MonoBehaviour
{
    private Animator _anim;
    private MenuManager _menuManager;
    private int _rewardAmount;
    public Image rewardImage;
    public float rewardAmountLerpingTime = 0.3f;
    public Text rewardAmountText;
    private string _itemName;
    public Text rewardItemName;
    private Reward _currentReward;
    public Animator claimButtonAnimator;
    public float prizeClaimDelay = 1.5f;
    IEnumerator DelayBeforeClaimButton()
    {
        yield return new WaitForSeconds(prizeClaimDelay);
        claimButtonAnimator.SetTrigger("Grow");
    }
    public void Init(Reward reward,MenuManager menuManager)
    {
        StartCoroutine(DelayBeforeClaimButton());
        _currentReward = reward;
        _itemName = reward.shopItemName;
        rewardItemName.text = _itemName;
        _menuManager = menuManager;
     _anim = GetComponent<Animator>();
     _anim.SetTrigger("Open");
     rewardImage.GetComponent<NativeAspectRatio>().ChangeImage(reward.icon);
     _rewardAmount = reward.amount;
     StartCoroutine(CountRewardAmount(_rewardAmount));
    }

    public void ClaimReward()
    {
        for (int i = 0; i < _rewardAmount; i++)
        {
            switch (_currentReward.type)
            {
                case RewardType.Coin:
                    User.Coin = 1;
                    break;
                case RewardType.Exp:
                    User.Rxp = 1;
                    break;
                case RewardType.ShopItem:
                   // this is present in the shop branch, remove the comments after merging with Update !
                    // GlobalValue.incrementShopItem(_itemName);
                    break;
            }
        }
        _menuManager.OpenVictoryMenu();
    }

    IEnumerator CountRewardAmount(int targetAmount)
    {
        float addedAmount = 0f;
        float elapsedTime = 0f;
        while (elapsedTime<= rewardAmountLerpingTime)
        {
            elapsedTime+= Time.deltaTime;
            float progress = elapsedTime/rewardAmountLerpingTime;
            addedAmount = Mathf.Lerp(0f, targetAmount, progress);
            rewardAmountText.text = "x" + addedAmount;
            yield return null;
        }
        rewardAmountText.text = "x" + targetAmount;
    }
}
