using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class RewardMenu : MonoBehaviour
{
    private Animator _anim;
    private MenuManager _menuManager;
    private int _rewardAmount;
    private string _itemName;
    private Reward _currentReward;

    [Header("UI")]
    public Image rewardImage;
    public Text rewardAmountText;
    public Text rewardItemName;

    [Header("Animation")]
    public Animator claimButtonAnimator;
    public float rewardAmountLerpingTime = 0.3f;
    public float prizeClaimDelay = 1.5f;
    public float delayForTextLerp = 0.53f;

    [Header("Effects")]
    public ParticleSystem[] confettiFx;
    public AudioClip prizeSource;

    private AudioSource _audioSource;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void Init(Reward reward, MenuManager menuManager)
    {
        _currentReward = reward;
        _menuManager = menuManager;

        _itemName = reward.shopItemName;
        _rewardAmount = reward.amount;

        rewardItemName.text = _itemName;

        // ✅ FETCH REWARD IMAGE
        rewardImage.sprite = reward.icon;
        rewardImage.preserveAspect = true;

        _anim.SetTrigger("Open");

        foreach (ParticleSystem confetti in confettiFx)
        {
            confetti.Play();
        }

        _audioSource.clip = prizeSource;
        _audioSource.Play();

        StartCoroutine(DelayBeforeClaimButton());
        StartCoroutine(CountRewardAmount(_rewardAmount));
    }

    IEnumerator DelayBeforeClaimButton()
    {
        yield return new WaitForSeconds(prizeClaimDelay);
        claimButtonAnimator.SetTrigger("Grow");
    }

    public void ClaimReward()
    {
        switch (_currentReward.type)
        {
            case RewardType.Coin:
                User.Coin += _rewardAmount;
                break;

            case RewardType.Exp:
                User.Rxp += _rewardAmount;
                break;

            case RewardType.ShopItem:
                GlobalValue.IncrementChosenShopItem(_itemName);
                GlobalValue.ItemFreeze = 5;
                break;
        }

        SoundManager.Instance.ClickBut();
        _menuManager.OpenVictoryMenu();
    }

    IEnumerator CountRewardAmount(int targetAmount)
    {
        yield return new WaitForSeconds(delayForTextLerp);

        float elapsedTime = 0f;

        while (elapsedTime < rewardAmountLerpingTime)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / rewardAmountLerpingTime;
            int displayedAmount = Mathf.RoundToInt(Mathf.Lerp(0, targetAmount, progress));
            rewardAmountText.text = "x" + displayedAmount;
            yield return null;
        }

        rewardAmountText.text = "x" + targetAmount;
    }
}
