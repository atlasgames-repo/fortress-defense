using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTutorial : MonoBehaviour
{
    public Transform[] uiParts;
    public Transform mask;
    public GameObject[] Tips;
    public float[] scale;
    public GameObject darkBackground;
    private int _tipIndex;
    private int _tipOrder;
    public float speed = 60;

    void Start()
    {
        _tipIndex = uiParts.Length;
        _tipOrder = -1;

        if (PlayerPrefs.GetInt("GameTutorialOpened") != 1)
        {
            StartCoroutine(OpenTutorialAtStart());
        }
    }

    IEnumerator OpenTutorialAtStart()
    {
        yield return new WaitForSeconds(1.5f);
        StartTutorial();
        PlayerPrefs.SetInt("GameTutorialOpened", 1);
    }

    public void StartTutorial()
    {
        NextTip();
        Time.timeScale = 0;
        darkBackground.SetActive(true);
    }

    public void NextTip()
    {
        _tipOrder++;

          if (_tipOrder <= Tips.Length-1)
        {
            Tips[_tipOrder].GetComponent<Animator>().SetTrigger("Open");
        } else
          {
              print("end");
              darkBackground.SetActive(false);
              Time.timeScale = 1;
              _tipOrder = -1;
              Tips[Tips.Length-1].GetComponent<Animator>().SetTrigger("Close");

          }

      

        if (_tipOrder > 0)
        {
            Tips[_tipOrder - 1].GetComponent<Animator>().SetTrigger("Close");
        }
    }

    Vector3 _mask_position;

    void Update()
    {
        float _appliedScale = 0;
        
        if (_tipIndex != -1)
        {
            try
            {
                _mask_position = Vector3.Lerp(mask.position, uiParts[_tipOrder].position, Time.unscaledDeltaTime * speed);
                _appliedScale = Mathf.Lerp(mask.localScale.x, scale[_tipOrder], Time.unscaledDeltaTime * speed);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
               
            }
        }
        else
        {
            _mask_position = Vector3.Lerp(mask.position, uiParts[0].position, Time.unscaledDeltaTime * speed);

        }

        mask.localScale = new Vector3(_appliedScale, _appliedScale, _appliedScale);
        mask.position = _mask_position;
    }
}