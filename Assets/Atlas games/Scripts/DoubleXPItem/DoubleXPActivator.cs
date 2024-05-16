using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoubleXPActivator : MonoBehaviour
{
    public Text counterText;
    public Text buttonText;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckValue());
    }

    IEnumerator CheckValue()
    {
        while (true)
        {
            if (GlobalValue.DoubleXpActive == 0)
            {
                buttonText.gameObject.SetActive(true);
                counterText.gameObject.SetActive(false);
                break;
            }
            buttonText.gameObject.SetActive(false);
            counterText.gameObject.SetActive(true);
            counterText.text = DoubleXPManager.CounterText();
            yield return new WaitForSeconds(1f);
        }
    }

    public void ActivateDoubleXp(bool h24duration)
    {
        if (GlobalValue.DoubleXpActive == 0)
        {
            StartCoroutine(CheckValue());
            if (h24duration)
            {
                DoubleXPManager.GetTime(DoubleXPManager.DoubleXpDuration.Day);
            }
            else
            {
                DoubleXPManager.GetTime(DoubleXPManager.DoubleXpDuration.Hour);
            }
        }
    }
}
