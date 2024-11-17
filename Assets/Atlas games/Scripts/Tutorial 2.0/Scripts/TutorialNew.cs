using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialNew : MonoBehaviour
{
    public Tip[] tutorialStep;

    [Header("Tutorial Placing")] 
    [Space(3)]
    public TutorialPlacing placing;
    [SerializeField] public string tutorialName;
    [SerializeField] public int tutorialLevel;

    [Header("UI Elements")]
    [Space(3)]
    public RectTransform circleMask;
    public Transform pointer;
    public float maskSpeed;
    public Transform pointerObject;
    public Transform pointerIcon;
    public GameObject clickPreventer;
    public Color transparent;
    public Color darkBackground;
    private int _tipOrder;
    [SerializeField]public float speed = 30f;
    Camera _main;
    
    [Header("Prefabs")]
    [Space(3)]
    public Hint hint;
    public Dialog dialog;

    
    [Header("Positions")]
    [Space(3)]
    public Transform TL_Pos;
    public Transform TM_Pos;
    public Transform TR_Pos;
    public Transform L_Pos;
    public Transform R_Pos;
    public Transform BL_Pos;
    public Transform BR_Pos;
    public Transform BM_Pos;

    void Start()
    {
        _main = Camera.main;
    }

    void InitTutorial()
    {
        _main = Camera.main;
        _tipOrder = -1;
        Invoke("NextStep",tutorialStep[0].delay);
    }

    public void NextStep()
    {
        if (_tipOrder < tutorialStep.Length - 1)
        {
            _tipOrder++;
            clickPreventer.SetActive(tutorialStep[_tipOrder].tipType!= TipType.Task);
            clickPreventer.GetComponent<CanvasGroup>().blocksRaycasts = !tutorialStep[_tipOrder].isUiInteractible;
            switch (tutorialStep[_tipOrder].tipType)
            {
                case TipType.Dialog:
                    
                    break;
            }
        }
        else
        {
            clickPreventer.GetComponent<Image>().enabled = false;
            clickPreventer.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        switch (tutorialStep[_tipOrder].tipType)
        {
            case TipType.Dialog:
                dialog.DialogChange(tutorialStep[_tipOrder], DialogAction.Next, this);
                break;
            case TipType.Hint:
                hint.Show(tutorialStep[_tipOrder].tipText,tutorialStep[_tipOrder].tipDirection,this);
                break;
            case TipType.Task:
                
                break;
        }
    }

    public void PreviousStep()
    {
        
    }
}
