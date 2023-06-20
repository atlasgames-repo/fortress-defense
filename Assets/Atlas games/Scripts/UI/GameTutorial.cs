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
    public float speed = 60;


    // start game and open tutorial automatically if never watched 
    void Start()
    {
        _tipIndex = uiParts.Length;
        _tipOrder = -1;

        if (PlayerPrefs.GetInt("GameTutorialOpened") != 1)
        {
            StartCoroutine(OpenTutorialAtStart());
        }
    }

// open tutorial
    IEnumerator OpenTutorialAtStart()
    {
        yield return new WaitForSeconds(1.5f);
        StartTutorial();
        PlayerPrefs.SetInt("GameTutorialOpened", 1);
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
            tips[_tipOrder].GetComponent<Animator>().SetTrigger("Open");
        }
        else
        {
            print("end");
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

    Vector3 _maskPosition;

    void Update()
    {
        // set mask position on UI element 
        // scale circle mask based on UI component for tutorial 
        float appliedScale = 0;

        if (_tipIndex != -1)
        {
            try
            {
                _maskPosition = Vector3.Lerp(mask.position, uiParts[_tipOrder].position,
                    Time.unscaledDeltaTime * speed);
                appliedScale = Mathf.Lerp(mask.localScale.x, scale[_tipOrder], Time.unscaledDeltaTime * speed);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        else
        {
            _maskPosition = Vector3.Lerp(mask.position, uiParts[0].position, Time.unscaledDeltaTime * speed);
        }

        mask.localScale = new Vector3(appliedScale, appliedScale, appliedScale);
        mask.position = _maskPosition;
    }
}