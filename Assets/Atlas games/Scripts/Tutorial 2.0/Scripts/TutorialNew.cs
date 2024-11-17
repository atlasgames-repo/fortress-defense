using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialNew : MonoBehaviour
{
    public Tip[] tutorialStep;
    
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
    public float speed = 30f;
    [HideInInspector] public string tutorialName;
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
        _tipOrder = 0;
        Invoke("NextStep",tutorialStep[0].delay);
    }

    public void NextStep()
    {
        switch (tutorialStep[_tipOrder].tipType)
        {
            case TipType.Dialog:
                
                break;
            case TipType.Hint:
                
                break;
            case TipType.Task:
                
                break;
        }
    }

    public void PreviousStep()
    {
        
    }
}
