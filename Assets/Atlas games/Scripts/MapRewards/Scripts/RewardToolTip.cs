using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class RewardToolTip : MonoBehaviour
{
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
     RectTransform _rect;
    public Canvas canvas;
    public float padding = 100;
    public Sprite rxpIcon;
    public Sprite goldIcon;
    void Start()
    {
        _rect = rewardTooltip.GetComponent<RectTransform>();
        _rewardAnimator = rewardTooltip.GetComponent<Animator>();
    //    rewardTooltip.SetActive(false);
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
    }
    
    private void OnEnable()
    {
        MapControllerUI.OnMapChange += OnElementEnteredCanvas; // Subscribe to the event with int parameter
    }

    private void OnDisable()
    {
        MapControllerUI.OnMapChange -= OnElementEnteredCanvas; // Unsubscribe to avoid memory leaks
    }


    void OpenTooltip()
    {
        if (GlobalValue.LevelPass > _levelsUnlocked)
        {
            switch (_currentReward.type)
            {
                case RewardType.Coin:
                    _rewardImageScaler.ChangeImage(goldIcon);
                    break;
                case RewardType.Exp:
                    _rewardImageScaler.ChangeImage(rxpIcon);
                    break;
                case RewardType.ShopItem:
                    _rewardImageScaler.ChangeImage(_currentReward.icon);
                    break;
            }
            mysteryGiftIcon.SetActive(false);
            amountText.gameObject.SetActive(true);
            amountText.text = "x" + _currentReward.amount;
            rewardTooltip.SetActive(true);
            rewardImage.gameObject.SetActive(true);
            rewardTick.SetActive(true);
        }
        else
        {
            amountText.gameObject.SetActive(false);
            rewardImage.gameObject.SetActive(false);
            rewardTick.SetActive(false);
            mysteryGiftIcon.SetActive(true);
        }
        _rewardAnimator.SetTrigger("Open");
    }
    

    private void OnElementEnteredCanvas(int enteredMap)
    {
        if (enteredMap == _levelToReward/11)
        {
            OpenTooltip();
        }
    }
}
