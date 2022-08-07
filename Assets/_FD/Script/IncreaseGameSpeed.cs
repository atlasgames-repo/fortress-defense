using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncreaseGameSpeed : MonoBehaviour
{
    public float timeSpeedUp = 2;
    public GameObject blinkingObj;
    public Text speedTxt;
    public GameObject helperObj;
    private void Start()
    {
        speedTxt.text = "Speed x1";
        helperObj.SetActive(false);
        Invoke("ShowHelper", 10);
    }

    void ShowHelper()
    {
         if (PlayerPrefs.GetInt("IncreaseGameSpeedDontShowAgain", 0) == 0)
            helperObj.SetActive(true);
    }

    public void ChangeSpeed()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = timeSpeedUp;
            StartCoroutine(BlinkingCo());
            speedTxt.text = "Speed x" + timeSpeedUp;
            SoundManager.PlaySfx(SoundManager.Instance.soundTimeUp);
        }
        else
        {
            blinkingObj.SetActive(true);
            Time.timeScale = 1;
            StopAllCoroutines();
            speedTxt.text = "Speed x1";
            SoundManager.PlaySfx(SoundManager.Instance.soundTimeDown);
        }

        PlayerPrefs.SetInt("IncreaseGameSpeedDontShowAgain", 1);
        helperObj.SetActive(false);
    }

    IEnumerator BlinkingCo()
    {
        while (true)
        {
            blinkingObj.SetActive(true);
            yield return new WaitForSeconds(0.8f);
            blinkingObj.SetActive(false);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
