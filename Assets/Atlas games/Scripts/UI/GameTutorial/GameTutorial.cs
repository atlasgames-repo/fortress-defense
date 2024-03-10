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

    public bool inMenu = false;
    public string menuPlacing;
    [HideInInspector] public TipType type;
    [HideInInspector] public Direction direction;


    public int level;
    public List<TipSetup> tipsList;
    [HideInInspector] public Transform[] uiParts;
    public Transform mask;
    public GameObject darkBackground;
    public GameObject dialogBackground;
    public Transform pointerObject;
    public Transform pointerIcon;
    public GameObject clickPreventer;
    private int _tipOrder;
    public float speed = 30f;
    private float _maxDistance = 0.01f;
    private int _childIndex;
    private Transform _buttonParent;
    private GameObject _uiPartClone;
    [Serializable]
    public class TipSetup
    {
        public string openTrigger;
        public string closeTrigger;
        public string pointerDirection;
        public float delay;
        public bool isLastDialog;
        public Animator tipAnimator;
        public string uiPartName;
        public float scale;
        public string type;
        public bool isUiInteractible;
        public bool pauseGame;
    }

    public float initialWait = 0.5f;

    [HideInInspector]public string tutorialName;


    // start game and open tutorial automatically if never watched 
    void Start()
    {
        clickPreventer.SetActive(false);
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
        yield return new WaitForSeconds(initialWait);
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

    private GameObject _prevUiPart;
    void CloseTip()
    {
        
        if (_tipOrder > 0)
        {
            TipSetup prevSetup = tipsList[_tipOrder - 1];
            if (prevSetup.uiPartName != "")
            {
                TutorialFinder[] uiParts = FindObjectsOfType<TutorialFinder>();
                foreach (var uiPart in uiParts)
                {
                    if (uiPart.GetComponent<TutorialFinder>().uiPartName == prevSetup.uiPartName)
                    {
                        _prevUiPart = uiPart.gameObject;
                    }
                }
              //  _prevUiPart = GameObject.Find(prevSetup.uiPartName);
            }
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
                        _prevUiPart.transform.SetParent(_buttonParent);
                        _prevUiPart.transform.SetSiblingIndex(_childIndex);
                     Destroy(_uiPartClone);
                        clickPreventer.SetActive(false);
                    }

                    if (prevSetup.pauseGame)
                    {
                        Time.timeScale = 1;
                    }
                    
                    _prevUiPart.GetComponent<Button>().onClick.RemoveListener(NextTip);
                    break;
            }
        }
    }


    private GameObject _nextUiPart;
    public void OpenTip()
    {
        if (_tipOrder <= tipsList.Count - 1)
        {
            TipSetup nextSetup = tipsList[_tipOrder];
            if (nextSetup.uiPartName != "")
            {
                TutorialFinder[] uiParts = FindObjectsOfType<TutorialFinder>();
                foreach (var uiPart in uiParts)
                {
                    if (uiPart.GetComponent<TutorialFinder>().uiPartName ==  nextSetup.uiPartName)
                    {
                        _nextUiPart = uiPart.gameObject;
                    }
                }
            }
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
                        _buttonParent = _nextUiPart.transform.parent;
                        _childIndex = _nextUiPart.transform.GetSiblingIndex();
                        clickPreventer.SetActive(true);
                        _uiPartClone = Instantiate(_nextUiPart, _nextUiPart.transform.position, _nextUiPart.transform.rotation,
                            _nextUiPart.transform.parent);
                        _nextUiPart.transform.transform.SetParent(clickPreventer.transform);
                    }

                    if (_nextUiPart.GetComponent<Button>())
                    {
                        _nextUiPart.transform.GetComponent<Button>().onClick.AddListener(NextTip);
                    }

                    Thread.Sleep(Mathf.RoundToInt(nextSetup.delay * 1000));
                    pointerObject.gameObject.SetActive(true);
                    pointerIcon.gameObject.SetActive(true);
                    pointerObject.transform.position = _nextUiPart.transform.position;
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
                    if (nextSetup.pauseGame)
                    {
                        Time.timeScale = 0;
                    }
                    else
                    {
                        Time.timeScale = 1;
                    }
                    break;
                case "Tip":
                    nextSetup.tipAnimator.SetTrigger(nextSetup.openTrigger);
                    Time.timeScale = 0;
                    darkBackground.SetActive(true);
                    mask.gameObject.SetActive(true);
                    dialogBackground.SetActive(false);
                    StartCoroutine(SmoothTransition(_nextUiPart.transform.position, nextSetup.scale));
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

    public void SkipTutorial()
    {
        _tipOrder++;
        TipSetup prevSetup = tipsList[_tipOrder - 1];
        if (prevSetup.uiPartName != "")
        {
            TutorialFinder[] uiParts = FindObjectsOfType<TutorialFinder>();
            foreach (var uiPart in uiParts)
            {
                if (uiPart.name == prevSetup.uiPartName)
                {
                    _prevUiPart = uiPart.gameObject;
                }
            }
            //  _prevUiPart = GameObject.Find(prevSetup.uiPartName);
        }
        
        switch (prevSetup.type)
        {
            case "Dialog":
                if (prevSetup.isLastDialog)
                {
                    prevSetup.tipAnimator.SetTrigger(prevSetup.closeTrigger);
                }
                else
                {
                    prevSetup.tipAnimator.gameObject.SetActive(false);
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
                    _prevUiPart.transform.SetParent(_buttonParent);
                    _prevUiPart.transform.SetSiblingIndex(_childIndex);
                    Destroy(_uiPartClone);
                    clickPreventer.SetActive(false);
                }

                if (prevSetup.pauseGame)
                {
                    Time.timeScale = 1;
                }

                if (_prevUiPart.GetComponent<Button>())
                {
                    _prevUiPart.GetComponent<Button>().onClick.RemoveListener(NextTip);
                }
                break;
        }

        darkBackground.SetActive(false);
        dialogBackground.SetActive(false);
        _tipOrder = -1;
    }
}