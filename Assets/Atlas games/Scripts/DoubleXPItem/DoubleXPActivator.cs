using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoubleXPActivator : MonoBehaviour
{
    public Text counterText;
    public Text buttonText;
    // Start is called before the first frame update
    void Update()
    {
        CheckValue();
    }
    void CheckValue()
    {
        if (GlobalValue.DoubleXpActive != 1)
        {
            buttonText.gameObject.SetActive(true);
            counterText.gameObject.SetActive(false);
        }
        else
        {
            buttonText.gameObject.SetActive(false);
            counterText.gameObject.SetActive(true);
            counterText.text = DoubleXPManager.CounterText();
        }
    }

    public void ActivateDoubleXp(bool h24duration)
    {
        buttonText.gameObject.SetActive(false);
        counterText.gameObject.SetActive(true);
        if (GlobalValue.DoubleXpActive == 0)
        {
          
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
