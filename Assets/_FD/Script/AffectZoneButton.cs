using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class AffectZoneButton : MonoBehaviour
{
    public AffectZoneType affectType;
    public Button ownBtn;

    [Header("COOL DOWN")]
    public float delayOnStart = 2;
    public float coolDown = 3f;
    float coolDownCounter = 0;
    public Image image;
    public Text timerTxt;
    bool allowWork = true;
    bool allowCounting = false;
    bool canUse = true;
    float holdCounter = 0;
    public CanvasGroup canvasGroup;

    void Start()
    {
        ownBtn = GetComponent<Button>();
        ownBtn.onClick.AddListener(OnBtnClick);

        if (image == null)
            image = GetComponent<Image>();
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        allowWork = false;
        allowCounting = true;
        coolDownCounter = delayOnStart;
    }

    void Update()
    {
        if (!allowWork)
        {
            if (allowCounting)
            {
                coolDownCounter -= Time.deltaTime;

                if (coolDownCounter <= 0)
                    allowWork = true;
            }
        }
        else
        {
            holdCounter -= Time.deltaTime;
        }

        image.fillAmount = Mathf.Clamp01((coolDown - coolDownCounter) / coolDown);

        timerTxt.text = (int)coolDownCounter + "";
        if ((int)coolDownCounter == 0)
            timerTxt.text = "";

        canUse = coolDownCounter <= 0 && canvasGroup.blocksRaycasts && !AffectZoneManager.Instance.isAffectZoneWorking && !AffectZoneManager.Instance.isChecking;

        canvasGroup.interactable = canUse;
    }

     void ActiveLighting()
    {
        AffectZoneManager.Instance.ActiveZone(AffectZoneType.Lighting, this);
        SoundManager.Click();
    }

     void ActiveFrozen()
    {
        AffectZoneManager.Instance.ActiveZone(AffectZoneType.Frozen, this);
        SoundManager.Click();
    }

     void ActivePoison()
    {
        AffectZoneManager.Instance.ActiveZone(AffectZoneType.Poison, this);
        SoundManager.Click();
    }



    public void StartCountingDown()
    {
        allowCounting = true;
        coolDownCounter = coolDown;
    }

    private void OnBtnClick()
    {
        if (!canUse)
            return;

        if (!allowWork)
            return;

        switch (affectType)
        {
            case AffectZoneType.Lighting:
                ActiveLighting();
                break;
            case AffectZoneType.Frozen:
                ActiveFrozen();
                break;
            case AffectZoneType.Poison:
                ActivePoison();
                break;
        }

        allowWork = false;
        allowCounting = false;
        
    }
}
