using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostItemUI : MonoBehaviour
{
    public static BoostItemUI Instance;
    [ReadOnly] public WEAPON_EFFECT currentEffect = WEAPON_EFFECT.NONE;
    [ReadOnly] public NumberArrow currentNumberOfArrows = NumberArrow.Single;

    [Header("Double Arrow")]
    public Text DA_remainTxt;
    public Button DA_Button;
    public GameObject DA_Icon;
    public Text DA_timerTxt;
    public int DA_Time = 25;
    [ReadOnly] public float DA_TimeCounter = 0;
    //[Header("Triple Arrow")]
    //public Text TA_remainTxt;
    //public Button TA_Button;
    //public GameObject TA_Icon;
    //public Text TA_timerTxt;
    //public int TA_Time = 20;
    //[ReadOnly] public float TA_TimeCounter = 0;
    [Header("Poison Arrow")]
    public Text PA_remainTxt;
    public Button PA_Button;
    public GameObject PA_Icon;
    public Text PA_timerTxt;
    public int PA_Time = 30;
    [ReadOnly] public float PA_TimeCounter = 0;
    [Header("Freeze Arrow")]
    public Text FA_remainTxt;
    public Button FA_Button;
    public GameObject FA_Icon;
    public Text FA_timerTxt;
    public int FA_Time = 30;
    [ReadOnly] public float FA_TimeCounter = 0;

    [Header("Boost Item")]
    public Animator boostItemAnim;
    public Animator boostButtonAnim;
    public float boostItemautoHide = 3;

    [Space]
    public GameObject activeIcons;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        DA_remainTxt.text = "x" + GlobalValue.ItemDoubleArrow;
        //TA_remainTxt.text = "x" + GlobalValue.ItemTripleArrow;
        PA_remainTxt.text = "x" + GlobalValue.ItemPoison;
        FA_remainTxt.text = "x" + GlobalValue.ItemFreeze;

        DA_Button.interactable = GlobalValue.ItemDoubleArrow > 0;
        //TA_Button.interactable = GlobalValue.ItemTripleArrow > 0;
        PA_Button.interactable = GlobalValue.ItemPoison > 0;
        FA_Button.interactable = GlobalValue.ItemFreeze > 0;

        DA_Icon.SetActive(false);
        //TA_Icon.SetActive(false);
        PA_Icon.SetActive(false);
        FA_Icon.SetActive(false);
    }

    private void Update()
    {
        activeIcons.SetActive(DA_Icon.activeSelf /*|| TA_Icon.activeSelf */|| PA_Icon.activeSelf || FA_Icon.activeSelf);
    }

    #region Double Arrow
    public void ActiveDoubleArror()
    {
        SoundManager.PlaySfx(SoundManager.Instance.BTsoundUseBoost);
        GlobalValue.ItemDoubleArrow--;
        DA_remainTxt.text = "x" + GlobalValue.ItemDoubleArrow;
        DA_Button.interactable = false;     //only active per game level

        currentNumberOfArrows = NumberArrow.Double;
        //DA_Icon.SetActive(true);
        RunTimerAutoHideBoostPanel();
        DoubleArrowTimerCoDo = DoubleArrowTimerCo();
        StartCoroutine(DoubleArrowTimerCoDo);
    }

    IEnumerator DoubleArrowTimerCoDo;
    IEnumerator DoubleArrowTimerCo()
    {
        DA_Icon.SetActive(true);

        DA_TimeCounter = (float)DA_Time;
        while (DA_TimeCounter > 0)
        {
            DA_TimeCounter -= Time.deltaTime;
            DA_timerTxt.text = (int)DA_TimeCounter + "";
            yield return null;
        }

        DA_Icon.SetActive(false);
        DA_Button.interactable = true && GlobalValue.ItemDoubleArrow > 0;
        currentNumberOfArrows = NumberArrow.Single;
    }
    #endregion

    //#region Triple Arrow
    //public void ActiveTripleArrow()
    //{
    //    SoundManager.PlaySfx(SoundManager.Instance.BTsoundUseBoost);
    //    GlobalValue.ItemTripleArrow--;
    //    TA_remainTxt.text = "x" + GlobalValue.ItemTripleArrow;
    //    TA_Button.interactable = false;     //only active per game level
    //    DA_Button.interactable = false;
    //    ArcherForceEffect.Instance.SetNumberArrow(ARCHER_FIRE_ARROWS.TRIPLE);
        
    //    RunTimerAutoHideBoostPanel();

    //    if (DoubleArrowTimerCoDo != null)
    //        StopCoroutine(DoubleArrowTimerCoDo);

    //    StartCoroutine(TripleArrowTimerCo());
    //}

    //IEnumerator TripleArrowTimerCo()
    //{
    //    DA_Icon.SetActive(false);
    //    TA_Icon.SetActive(true);

    //    TA_TimeCounter = (float)TA_Time;
    //    while (TA_TimeCounter > 0)
    //    {
    //        TA_TimeCounter -= Time.deltaTime;
    //        TA_timerTxt.text = (int)TA_TimeCounter + "";
    //        yield return null;
    //    }

    //    TA_Icon.SetActive(false);
    //    TA_Button.interactable = true && GlobalValue.ItemTripleArrow > 0;
    //    DA_Button.interactable = true && GlobalValue.ItemDoubleArrow > 0;
    //    ArcherForceEffect.Instance.SetNumberArrow(NumberArrow.ONE);
    //}
    //#endregion

    #region Poison Arrow
    public void ActivePoisonArrow() {
        SoundManager.PlaySfx(SoundManager.Instance.BTsoundUseBoost);
        GlobalValue.ItemPoison--;
        PA_remainTxt.text = "x" + GlobalValue.ItemPoison;
        PA_Button.interactable = false; 
        FA_Button.interactable = false;
        currentEffect = WEAPON_EFFECT.POISON;

        RunTimerAutoHideBoostPanel();
        StartCoroutine(PoisonArrowTimerCo());
    }

    IEnumerator PoisonArrowTimerCo()
    {
        PA_Icon.SetActive(true);
       
        PA_TimeCounter = (float)PA_Time;
        while (PA_TimeCounter > 0)
        {
            PA_TimeCounter -= Time.deltaTime;
            PA_timerTxt.text = (int)PA_TimeCounter + "";
           yield return null;
        }

        PA_Icon.SetActive(false);
        PA_Button.interactable = true && GlobalValue.ItemPoison > 0;
        FA_Button.interactable = true && GlobalValue.ItemFreeze > 0;
        currentEffect = WEAPON_EFFECT.NONE;
    }
    #endregion

    #region Freeze Arrow
    public void ActiveFrezzeArrow() {
        SoundManager.PlaySfx(SoundManager.Instance.BTsoundUseBoost);
        GlobalValue.ItemFreeze--;
        FA_remainTxt.text = "x" + GlobalValue.ItemFreeze;
        FA_Button.interactable = false;
        PA_Button.interactable = false;
        currentEffect = WEAPON_EFFECT.FREEZE;

        RunTimerAutoHideBoostPanel();

        StartCoroutine(FrezzeArrowTimerCo());
    }

    IEnumerator FrezzeArrowTimerCo()
    {
        FA_Icon.SetActive(true);
        FA_TimeCounter = (float)FA_Time;
        while(FA_TimeCounter > 0)
        {
            FA_TimeCounter -= Time.deltaTime;
            FA_timerTxt.text = (int)FA_TimeCounter + "";
            yield return null;
        }

        FA_Icon.SetActive(false);
        PA_Button.interactable = true && GlobalValue.ItemPoison > 0;
        FA_Button.interactable = true && GlobalValue.ItemFreeze > 0;
        currentEffect = WEAPON_EFFECT.NONE;
    }
    #endregion

    #region BOOST PANEL
    IEnumerator BoostItemHideCoDo;
    public void BoostItem()
    {
        if (boostItemAnim.GetBool("show"))
        {
            HideBoostPanel();
        }
        else
        {
            SoundManager.PlaySfx(SoundManager.Instance.BTsoundOpen);
            boostItemAnim.SetBool("show", true);
            boostButtonAnim.SetBool("on", true);
            RunTimerAutoHideBoostPanel();
        }
    }

    void RunTimerAutoHideBoostPanel()
    {
        if (BoostItemHideCoDo != null)
            StopCoroutine(BoostItemHideCoDo);

        BoostItemHideCoDo = BoostItemHideCo(boostItemautoHide);
        StartCoroutine(BoostItemHideCoDo);
    }

    IEnumerator BoostItemHideCo(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideBoostPanel();
    }

    void HideBoostPanel()
    {
        SoundManager.PlaySfx(SoundManager.Instance.BTsoundHide);
        boostItemAnim.SetBool("show", false);
        boostButtonAnim.SetBool("on", false);
    }

    #endregion
}
