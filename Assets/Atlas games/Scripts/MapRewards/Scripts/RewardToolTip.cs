using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardToolTip : MonoBehaviour
{
    public enum Direction
    {
        Left,Top,Right,Bottom
    }
    public Image rewardImage;
    public GameObject rewardTooltip;
   // public GameObject rewardIcon;
    public GameObject rewardTick;
    public GameObject mysteryGiftIcon;
    private int _levelsUnlocked;
    private int _levelToReward;
    private Vector2 _initialSize;
    public RewardList rewardList;
    private Reward _currentReward;
    private NativeAspectRatio _rewardImageScaler;
    public Text amountText;
    Animator _rewardAnimator;
    void Start()
    {
        _rewardAnimator = rewardTooltip.GetComponent<Animator>();
        rewardTooltip.SetActive(false);
        _rewardImageScaler = rewardImage.GetComponent<NativeAspectRatio>();
        List<int> levelsDefined = new List<int>();
        foreach (Reward reward in rewardList.rewards)
        {
            levelsDefined.Add(reward.rewardLevel);
        }
        _levelToReward = GetComponent<Level>().level;
        _levelsUnlocked = GlobalValue.LevelPass;
        if (levelsDefined.Contains(_levelToReward))
        {
            for (int i = 0; i < rewardList.rewards.Length; i++)
            {
                if (_levelToReward == rewardList.rewards[i].rewardLevel)
                {
                    _currentReward = rewardList.rewards[i];
                }
            }
        }

        if (GlobalValue.LevelPass > _levelsUnlocked)
        {
            _rewardImageScaler.ChangeImage(_currentReward.icon);
            mysteryGiftIcon.SetActive(false);
            amountText.text = "x" + _currentReward.amount;
        }
        else
        {
            rewardImage.gameObject.SetActive(false);
            rewardTick.SetActive(false);
            mysteryGiftIcon.SetActive(true);
        }
    }

    void OpenTooltip()
    {
        rewardTooltip.SetActive(true);
        _rewardAnimator.SetTrigger("Open");
        rewardImage.gameObject.SetActive(true);
        rewardTick.SetActive(true);
    }
}
