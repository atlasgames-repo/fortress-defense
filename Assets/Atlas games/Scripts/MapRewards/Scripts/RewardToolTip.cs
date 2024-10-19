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
    private RectTransform _rect;
    private Canvas _canvas;
    public float padding = 100;
    public Sprite rxpIcon;
    public Sprite goldIcon;
    void Start()
    {
        _canvas = FindObjectOfType<Canvas>();
        _rect = rewardTooltip.GetComponent<RectTransform>();
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
    
    private void OnRectTransformDimensionsChange()
    {
        if (IsElementVisible())
        {
            OnElementEnteredCanvas(); 
        }
    }

    private bool IsElementVisible()
    {
        RectTransform canvasRect = _canvas.GetComponent<RectTransform>();
        Vector3[] canvasCorners = new Vector3[4];
        canvasRect.GetWorldCorners(canvasCorners);
        float leftLimit = canvasCorners[0].x + padding;
        float rightLimit = canvasCorners[3].x - padding;
        Vector3[] elementCorners = new Vector3[4];
        _rect.GetWorldCorners(elementCorners);
        bool isWithinHorizontalBounds = elementCorners[0].x >= leftLimit && elementCorners[3].x <= rightLimit;
        return isWithinHorizontalBounds ;
    }

    private void OnElementEnteredCanvas()
    {
        OpenTooltip();
    }
}
