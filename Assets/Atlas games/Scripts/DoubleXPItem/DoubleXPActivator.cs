using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoubleXPActivator : MonoBehaviour
{
    public Text counterText;
    public Text buttonText;
    public string itemName;
    // Start is called before the first frame update
    void Update()
    {
        CheckValue();
    }
    void CheckValue()
    {
        if (GlobalValue.DoubleXp)
        {
            buttonText.gameObject.SetActive(false);
            counterText.gameObject.SetActive(true);
            counterText.text = TimedItemManager.CounterText();
        }
        else
        {
            buttonText.gameObject.SetActive(true);
            counterText.gameObject.SetActive(false);
           
        }
    }

    public void ActivateDoubleXp(bool h24duration)
    {
        buttonText.gameObject.SetActive(false);
        counterText.gameObject.SetActive(true);
        if (!GlobalValue.DoubleXp)
        {
            if (h24duration)
            {
                TimedItemManager.GetTime(TimedItemManager.ItemDuration.Day);
            }
            else
            {
                TimedItemManager.GetTime(TimedItemManager.ItemDuration.Hour);
            }
        }
    }
}
