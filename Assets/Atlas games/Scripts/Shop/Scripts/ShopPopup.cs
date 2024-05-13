using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPopup : MonoBehaviour
{
    public GameObject notEnoughCoinText;
    public GameObject maxAmountText;
    private Animator _anim;


    public void OpenDialog(bool notEnoughCoins)
    {
        _anim = GetComponent<Animator>();
        _anim.SetTrigger("Open");
        if (notEnoughCoins)
        {
            notEnoughCoinText.SetActive(true);
            maxAmountText.SetActive(false);
        }
        else
        {
            maxAmountText.SetActive(true);
            notEnoughCoinText.SetActive(false);
        }
    }

    public void CloseDialog()
    {
        _anim.SetTrigger("Close");
    }
    
    public void DisablePopup()
    {
        Invoke("Deactivate", 0.1f);
    }
    void Deactivate()
    {
        // Deactivate the GameObject this script is attached to
        gameObject.SetActive(false);
    }
}
