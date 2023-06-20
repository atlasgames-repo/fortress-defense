using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTutorial : MonoBehaviour
{
    public Transform[] uiParts;
    public Transform mask;
    public GameObject[] tips;
    public float[] scale;
    public GameObject darkBackground;
    private int _tipIndex;
    private int _tipOrder;
    public float speed = 30f;

    // start game and open tutorial automatically if never watched 
    void Start()
    {
        _tipIndex = uiParts.Length;
        _tipOrder = -1;

        if (GlobalValue.GameTutorialOpened != 1)
        {
            StartCoroutine(OpenTutorialAtStart());
        }
    }

// open tutorial if never watched
    IEnumerator OpenTutorialAtStart()
    {
        yield return new WaitForSeconds(1.5f);
        StartTutorial();
        GlobalValue.GameTutorialOpened = 1;
    }

// start tutorial with first tip and pause game 
    public void StartTutorial()
    {
        NextTip();
        Time.timeScale = 0;
        darkBackground.SetActive(true);
    }

// go to next tip and animate enter/exit states of tips 
    public void NextTip()
    {
        _tipOrder++;

        if (_tipOrder <= tips.Length - 1)
        {
            StartCoroutine(SmoothTransition(uiParts[_tipOrder].transform.position, scale[_tipOrder]));
            tips[_tipOrder].GetComponent<Animator>().SetTrigger("Open");
        }
        else
        {
            darkBackground.SetActive(false);
            Time.timeScale = 1;
            _tipOrder = -1;
            tips[tips.Length - 1].GetComponent<Animator>().SetTrigger("Close");
        }


        if (_tipOrder > 0)
        {
            tips[_tipOrder - 1].GetComponent<Animator>().SetTrigger("Close");
        }
    }


    IEnumerator SmoothTransition(Vector3 targetPosition, float targetScale)
    {
        float appliedScale = mask.localScale.x;
        Vector3 maskPosition = mask.position;
        while (Vector3.Distance(mask.position,uiParts[_tipOrder].position)>0.01f)
        {
            maskPosition = Vector3.Lerp(mask.position, uiParts[_tipOrder].position, Time.unscaledDeltaTime * speed);
            appliedScale = Mathf.Lerp(mask.localScale.x, scale[_tipOrder], Time.unscaledDeltaTime * speed);
            mask.localScale = new Vector3(appliedScale, appliedScale, appliedScale);
            mask.position = maskPosition;
            yield return null;
        }
    }

}