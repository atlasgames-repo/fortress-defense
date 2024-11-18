using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    public Transform TL_Pos_env;
    public Transform TM_Pos_env;
    public Transform TR_Pos_env;
    public Transform L_Pos_env;
    public Transform R_Pos_env;
    public Transform BL_Pos_env;
    public Transform BR_Pos_env;
    public Transform BM_Pos_env;
    
    public Transform pointerPlacerEnvironment;


    public Transform environmentPointer;
    private float _maxDistance = 0.01f;
    void Start()
    {
        _main = Camera.main;
        InitTutorial();
    }

    void InitTutorial()
    {
        _main = Camera.main;
        _tipOrder = -1;
        Invoke("NextStep",tutorialStep[0].delay*1000f);
    }

    public void ApplyNewStep()
    {
      
            GameObject nextUIPart = new GameObject();
            Transform buttonParent;
            int childIndex = 0;
            if (tutorialStep[_tipOrder].tipType == TipType.Task)
            {
                TutorialFinder[] uiParts = FindObjectsOfType<TutorialFinder>();
                foreach (var uiPart in uiParts)
                {
                    if (uiPart.GetComponent<TutorialFinder>().uiPartName == tutorialStep[_tipOrder].uiPartName)
                    {
                        nextUIPart = uiPart.gameObject;
                    }
                }
            }
            clickPreventer.SetActive(tutorialStep[_tipOrder].tipType!= TipType.Task);
            clickPreventer.GetComponent<CanvasGroup>().blocksRaycasts = !tutorialStep[_tipOrder].isUiInteractible;
            Time.timeScale = tutorialStep[_tipOrder].pauseGame ? 0f : 1f;
            switch (tutorialStep[_tipOrder].tipType)
            {
                case TipType.Dialog:
                    dialog.DialogChange(tutorialStep[_tipOrder], DialogAction.Next, tutorialStep[_tipOrder], this);
                    clickPreventer.GetComponent<Image>().enabled = true;
                    circleMask.gameObject.SetActive(false);
                    Time.timeScale = 0;
                    break;
                case TipType.Hint:
                    hint.Show(tutorialStep[_tipOrder].tipText, tutorialStep[_tipOrder].tipDirection,this, circleMask.rect.position, tutorialStep[_tipOrder].circleMaskScale, _tipOrder);
                    Time.timeScale = 0;
                    clickPreventer.GetComponent<Image>().enabled = false;
                    clickPreventer.GetComponent<CanvasGroup>().blocksRaycasts = true;
                    circleMask.gameObject.SetActive(true);
                    StartCoroutine(SmoothTransition(nextUIPart.transform.position, tutorialStep[_tipOrder].circleMaskScale));
                    break;
                case TipType.Task:
                    
                                        if (!tutorialStep[_tipOrder].isUiInteractible && nextUIPart.GetComponent<Button>())
                    {
                        buttonParent = nextUIPart.transform.parent;
                        childIndex = nextUIPart.transform.GetSiblingIndex();
                        clickPreventer.GetComponent<CanvasGroup>().blocksRaycasts =
                            !tutorialStep[_tipOrder].isUiInteractible;
                        GameObject uiPartClone = Instantiate(nextUIPart, nextUIPart.transform.position,
                            nextUIPart.transform.rotation,
                            nextUIPart.transform.parent);
                        nextUIPart.transform.transform.SetParent(clickPreventer.transform);
                        pointerObject.gameObject.SetActive(true);
                        pointerIcon.gameObject.SetActive(true);
                    }
                    Thread.Sleep(Mathf.RoundToInt(tutorialStep[_tipOrder].delay * 1000));
                    nextUIPart.GetComponent<TutorialFinder>().isClickable = true;
                    if (nextUIPart.GetComponent<Button>())
                    {
                        nextUIPart.transform.GetComponent<Button>().onClick.AddListener(NextStep);
                        pointerObject.transform.position = nextUIPart.transform.position;
                        pointerIcon.gameObject.SetActive(true);
                        float rotationAngle=0f;
                        Vector3 targetPos = new Vector3();
                        switch (tutorialStep[_tipOrder].pointerDirection)
                        {
                            case Direction.Bottom:
                                targetPos= BM_Pos.position;
                                rotationAngle = 180f;
                                break;
                            case Direction.Left :
                                targetPos = L_Pos.position;
                                rotationAngle = 90f;
                                break;
                            case Direction.Right : 
                                targetPos = R_Pos.position;
                                rotationAngle = -90f;
                                break;
                            case Direction.Top:
                                targetPos = TM_Pos.position;
                                rotationAngle = 0f;
                                break;
                            case Direction.BottomLeft:
                                targetPos = BL_Pos.position;
                                rotationAngle = -135f;
                                break;
                            case Direction.BottomRight:
                                targetPos = BR_Pos.position;
                                rotationAngle = 135f;
                                break;
                            case Direction.UpperLeft:
                                targetPos = TL_Pos.position;
                                rotationAngle = -45f;
                                break;
                            case Direction.UpperRight:
                                targetPos = TR_Pos.position;
                                rotationAngle = 45f;
                                break;
                        }
                        Quaternion pointerRotation = Quaternion.Euler(0, 0, rotationAngle);
                        pointerIcon.transform.rotation = pointerRotation;
                        pointerIcon.transform.position = targetPos;
                    }
                    else
                    {
                        environmentPointer.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
                        pointerPlacerEnvironment.transform.position = nextUIPart.transform.position;
                             float rotationAngle = 0f;
                             Vector3 targetPosition = new Vector3();
                             switch (tutorialStep[_tipOrder].pointerDirection)
                             {
                                 case Direction.Top:
                                     rotationAngle = 0f;
                                     targetPosition = TM_Pos_env.position;
                                     break;
                                 case Direction.Bottom:
                                     rotationAngle = 180f;
                                     targetPosition = BM_Pos_env.position;
                                     break;
                                 case Direction.Left:
                                     rotationAngle = 90f;
                                     targetPosition = L_Pos_env.position;
                                     break;
                                 case Direction.Right:
                                     rotationAngle = -90f;
                                     targetPosition = R_Pos_env.position;
                                     break;
                                 case Direction.UpperLeft:
                                     rotationAngle = -45f;
                                     targetPosition = TL_Pos_env.position;
                                     break;
                                 case Direction.UpperRight:
                                     targetPosition = TR_Pos_env.position;
                                     rotationAngle = 45f;
                                     break;
                                 case Direction.BottomLeft:
                                     rotationAngle = -135f;
                                     targetPosition = BL_Pos_env.position;
                                     break;
                                 case Direction.BottomRight:
                                     rotationAngle = 135f;
                                     targetPosition = BR_Pos_env.position;
                                     break;
                             }
                             Quaternion pointerRotation = Quaternion.Euler(0, 0, rotationAngle);
                             pointerIcon.transform.rotation = pointerRotation;
                             pointerIcon.transform.position = targetPosition;
                    }

                    clickPreventer.GetComponent<Image>().enabled = false;
                    circleMask.gameObject.SetActive(false);
                    break;
        }
    }

    public void NextStep()
    {
        if (_tipOrder < tutorialStep.Length - 1)
        {
            _tipOrder++;
            ApplyNewStep();
        }
        else
        {
            clickPreventer.GetComponent<Image>().enabled = false;
            clickPreventer.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public void PreviousStep()
    {
        if (_tipOrder > -1)
        {
            _tipOrder--;
            ApplyNewStep();
        }   
    }
    
    IEnumerator SmoothTransition(Vector3 targetPosition, float targetScale)
    {
        float appliedScale = circleMask.localScale.x;
        Vector3 maskPosition = circleMask.position;
        while (Vector3.Distance(circleMask.position, targetPosition) > _maxDistance)
        {
            maskPosition = Vector3.Lerp(circleMask.position, targetPosition, Time.unscaledDeltaTime * speed);
            appliedScale = Mathf.Lerp(circleMask.localScale.x, targetScale, Time.unscaledDeltaTime * speed);
            circleMask.localScale = new Vector3(appliedScale, appliedScale, appliedScale);
            circleMask.position = maskPosition;
            yield return null;
        }
    }
}