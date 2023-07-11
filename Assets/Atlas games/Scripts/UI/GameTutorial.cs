using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;

public class GameTutorial : MonoBehaviour
{
    public enum TipType
    {
        Dialog,
        Tip,
        Task
    }

    public enum Direction
    {
        Top,
        Bottom,
        Left,
        Right,
        UpperLeft,
        UpperRight,
        BottomLeft,
        BottomRight
    }


    [HideInInspector] public TipType type;
    [HideInInspector] public Direction direction;


    public List<TipSetup> tipsList;
    [HideInInspector] public Transform[] uiParts;
    public Transform mask;
    public GameObject darkBackground;
    public GameObject dialogBackground;
    public Transform pointerObject;
    public Transform pointerIcon;
    private int _tipOrder;
    public float speed = 30f;
    private float _maxDistance = 0.01f;


    [Serializable]
    public class TipSetup
    {
        public string openTrigger;
        public string closeTrigger;
        public string pointerDirection;
        public float delay;
        public bool isLastDialog;
        public Animator tipAnimator;
        public Transform uiPart;
        public float scale;
        public string type;
        public bool isUiInteractible;
    }

    public string tutorialName;

   public List<Button> buttons = new List<Button>();

    // start game and open tutorial automatically if never watched 
    void Start()
    {
        _tipOrder = -1;
        StartCoroutine(OpenTutorialAtStart());
        pointerIcon.gameObject.SetActive(false);
        if (PlayerPrefs.GetInt("GameTutorial" + tutorialName) != 1)
        {
            //play tutorial
        }
    }

// open tutorial if never watched
    IEnumerator OpenTutorialAtStart()
    {
        yield return new WaitForSeconds(1.5f);
        StartTutorial();
        
        //makes sure that tutorial doesnt play anymore at the start of the scene 
        PlayerPrefs.SetInt("GameTutorial" + tutorialName, 1);
    }

// start tutorial with first tip and pause game 
    public void StartTutorial()
    {
        NextTip();
    }

// go to next tip and animate enter/exit states of tips 
    public void NextTip()
    {
        _tipOrder++;
        // close last one 
        if (_tipOrder > 0)
        {
            CloseTip();
            OpenTip();
        }
        else
        {
            OpenTip();
        }
    }

    void CloseTip()
    {
        if (_tipOrder > 0)
        {
            TipSetup prevSetup = tipsList[_tipOrder - 1];
            switch (prevSetup.type)
            {
                case "Dialog":
                    if (prevSetup.isLastDialog)
                    {
                        prevSetup.tipAnimator.SetTrigger(prevSetup.closeTrigger);
                    }

                    break;
                case "Tip":
                    prevSetup.tipAnimator.SetTrigger(prevSetup.closeTrigger);
                    break;
                case "Task":
                    pointerObject.gameObject.SetActive(false);
                    pointerIcon.gameObject.SetActive(false);
                    
                    if (!prevSetup.isUiInteractible)
                    {
                        foreach (Button button in buttons)
                        { 
                            button.interactable = true;
                        }
                        buttons.Clear();
                    }
                    prevSetup.uiPart.GetComponent<Button>().onClick.RemoveListener(NextTip);
                    break;
            }
        }
    }


    public void OpenTip()
    {
        if (_tipOrder <= tipsList.Count - 1)
        {
            TipSetup nextSetup = tipsList[_tipOrder];
            switch (nextSetup.type)
            {
                case "Dialog":
                    nextSetup.tipAnimator.SetTrigger(nextSetup.openTrigger);
                    darkBackground.SetActive(false);
                    dialogBackground.SetActive(true);
                    mask.gameObject.SetActive(false);
                    dialogBackground.SetActive(true);
                    Time.timeScale = 0;
                    break;
                case "Task":
                    if (!nextSetup.isUiInteractible)
                    {
                        Button[] buttonsActive = FindObjectsOfType<Button>();
                        foreach (Button button in buttonsActive)
                        {
                            if (button.IsInteractable())
                            {
                                button.interactable = false;
                                buttons.Add(button);
                            }
                        }   
                    }
                    nextSetup.uiPart.GetComponent<Button>().interactable = true;
                    nextSetup.uiPart.GetComponent<Button>().onClick.AddListener(NextTip);
                    Thread.Sleep(Mathf.RoundToInt(nextSetup.delay * 1000));
                    pointerObject.gameObject.SetActive(true);
                    pointerIcon.gameObject.SetActive(true);
                    pointerObject.transform.position = nextSetup.uiPart.position;
                    for (int a = 0; a < pointerObject.childCount; a++)
                    {
                        if (pointerObject.GetChild(a).name == nextSetup.pointerDirection)
                        {
                            pointerIcon.transform.position = pointerObject.GetChild(a).position;
                            switch (nextSetup.pointerDirection)
                            {
                                case "Top":
                                    pointerIcon.transform.rotation = new Quaternion(0, 0, 0, 0);
                                    break;
                                case "Bottom":
                                    pointerIcon.transform.rotation = new Quaternion(0, 0, 180, 0);
                                    break;
                                case "Left":
                                    pointerIcon.transform.rotation = new Quaternion(0, 0, -90, 0);
                                    break;
                                case "Right":
                                    pointerIcon.transform.rotation = new Quaternion(0, 0, 90, 0);
                                    break;
                                case "TopLeft":
                                    pointerIcon.transform.rotation = new Quaternion(0, 0, -45, 0);
                                    break;
                                case "TopRight":
                                    pointerIcon.transform.rotation = new Quaternion(0, 0, 45, 0);
                                    break;
                                case "BottomLeft":
                                    pointerIcon.transform.rotation = new Quaternion(0, 0, -135, 0);
                                    break;
                                case "BottomRight":
                                    pointerIcon.transform.rotation = new Quaternion(0, 0, 135, 0);
                                    break;
                            }
                        }
                    }

                    darkBackground.SetActive(false);
                    mask.gameObject.SetActive(false);
                    dialogBackground.SetActive(false);
                    Time.timeScale = 1;
                    break;
                case "Tip":
                    nextSetup.tipAnimator.SetTrigger(nextSetup.openTrigger);
                    Time.timeScale = 0;
                    darkBackground.SetActive(true);
                    mask.gameObject.SetActive(true);
                    dialogBackground.SetActive(false);
                    StartCoroutine(SmoothTransition(nextSetup.uiPart.position, nextSetup.scale));
                    break;
            }
        }
        else
        {
            darkBackground.SetActive(false);
            dialogBackground.SetActive(false);
            Time.timeScale = 1;
            _tipOrder = -1;
        }
    }


    IEnumerator SmoothTransition(Vector3 targetPosition, float targetScale)
    {
        float appliedScale = mask.localScale.x;
        Vector3 maskPosition = mask.position;
        while (Vector3.Distance(mask.position, targetPosition) > _maxDistance)
        {
            maskPosition = Vector3.Lerp(mask.position, targetPosition, Time.unscaledDeltaTime * speed);
            appliedScale = Mathf.Lerp(mask.localScale.x, targetScale, Time.unscaledDeltaTime * speed);
            mask.localScale = new Vector3(appliedScale, appliedScale, appliedScale);
            mask.position = maskPosition;
            yield return null;
        }
    }
}