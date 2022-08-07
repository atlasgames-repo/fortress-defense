using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public float delayShow = 3;
    public CanvasGroup canvasG;

    // Start is called before the first frame update
    void Start()
    {
        canvasG.alpha = 0;

        InvokeRepeating("CheckFireOrNot", 0, 0.1f);
        Invoke("ShowTutorial", delayShow);
    }

    void CheckFireOrNot()
    {
        if (FindObjectOfType<ArrowProjectile>())
        {
            CancelInvoke();
            gameObject.SetActive(false);
        }
    }

    void ShowTutorial()
    {
        canvasG.alpha = 1;
    }
}
