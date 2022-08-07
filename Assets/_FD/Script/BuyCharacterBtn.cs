using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyCharacterBtn : MonoBehaviour
{
    public UpgradedCharacterParameter characterID;
    public int price = 888;
    public int max = 5;
    [ReadOnly] public int current = 0;
    public GameObject character;

    public GameObject lockIcon;
    public Text priceTxt;
    //public Text unlockLevelTxt;
    public Text numberTxt;
    public AudioClip soundPurchase;
    Button ownBtn;
    bool isUnlocked = false;

    [ReadOnly] public List<GameObject> listCharacters;


    [Header("COOL DOWN")]
    public float delayOnStart = 2;
    public float coolDown = 3f;
    float coolDownCounter = 0;
    public Image image;
    bool allowWork = true;
    bool canUse = true;
    float holdCounter = 0;
    public CanvasGroup canvasGroup;

    public GameObject poisonFX, freezeFX;

    void Start()
    {
        ownBtn = GetComponent<Button>();
        ownBtn.onClick.AddListener(OnBtnClick);

        poisonFX.SetActive(characterID.weaponEffect.effectType == WEAPON_EFFECT.POISON);
        freezeFX.SetActive(characterID.weaponEffect.effectType == WEAPON_EFFECT.FREEZE);

        listCharacters = new List<GameObject>();

        //if (GlobalValue.levelPlaying >= characterID.unlockAtLevel)
        //{
            isUnlocked = true;
        //}

        if (isUnlocked)
        {
            priceTxt.text = price + "";
            //unlockLevelTxt.enabled = false;
            InvokeRepeating("CheckAvailable", 0, 0.1f);
        }
        else
        {
            priceTxt.text = "LOCKED";
            //unlockLevelTxt.text = "" + characterID.unlockAtLevel;
            ownBtn.interactable = false;
        }

        lockIcon.SetActive(!isUnlocked);

        if (image == null)
            image = GetComponent<Image>();
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        allowWork = false;
        coolDownCounter = delayOnStart;
    }

    int numberCharacterAlive()
    {
        int alives = 0;
        foreach (var cha in listCharacters)
        {
            if (cha.activeInHierarchy)
                alives++;
        }
        return alives;
    }

    void Update()
    {
        //update number 
        numberTxt.text = numberCharacterAlive() + "/" + max;
        if (!allowWork)
        {
            coolDownCounter -= Time.deltaTime;

            if (coolDownCounter <= 0)
                allowWork = true;
        }
        else
        {
            holdCounter -= Time.deltaTime;
        }

        image.fillAmount = Mathf.Clamp01((coolDown - coolDownCounter) / coolDown);

        canvasGroup.interactable = coolDownCounter <= 0;

        canUse = canvasGroup.interactable && canvasGroup.blocksRaycasts;
    }

    void CheckAvailable()
    {
        ownBtn.interactable = GlobalValue.SavedCoins >= price && numberCharacterAlive() < max;
    }

    private void OnBtnClick()
    {
        if (!canUse)
            return;

        if (!allowWork)
            return;

        if (GameManager.Instance.currentExp >= price)
        {
            //SoundManager.PlaySfx(SoundManager.Instance.buyCharacter);
            CharacterManager.Instance.SpawnCharacter(this);
            
        }
    }

    public void AddCharacter(GameObject character)
    {
        listCharacters.Add(character);
        allowWork = false;
        coolDownCounter = coolDown;
        GameManager.Instance.currentExp -= price;
    }
}
