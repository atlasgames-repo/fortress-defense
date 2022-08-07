using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour {

	public GameObject[] shopPanels;
    public Sprite buttonActiveImage, buttonInActiveImage;
    public Image upgradeBut, buyCoinBut;
	//public GameObject RemoveAdBut;
//	MMFade mmFade = new MMFade();
	// Use this for initialization
	void Start () {
        DisableObj();
		ActivePanel (shopPanels[0]);
        SetActiveBut(0);
    }
		
	void Update(){
		//if(RemoveAdBut)
		//RemoveAdBut.SetActive ((GlobalValue.RemoveAds ? false : true));
	}

	void DisableObj(){
		foreach (var obj in shopPanels) {
            obj.SetActive(false);
		}
	}

	void ActivePanel(GameObject obj){
        //		StartCoroutine (
        //			MMFade.FadeCanvasGroup (canv, 0.5f, 1));

        obj.SetActive(true);
    }
	
	public void SwichPanel(GameObject obj){
		for (int i = 0; i < shopPanels.Length; i++) {
			if (obj == shopPanels[i]) {
                DisableObj();
				ActivePanel (shopPanels[i]);
                SetActiveBut(i);

                break;
			}
		}
		SoundManager.Click ();
	}

    void SetActiveBut(int i)
    {
        upgradeBut.sprite = buttonInActiveImage;
        buyCoinBut.sprite = buttonInActiveImage;

        switch (i)
        {
            case 0:
                upgradeBut.sprite = buttonActiveImage;
                break;
            case 1:
                buyCoinBut.sprite = buttonActiveImage;
                break;
            default:

                break;
        }
    }

//	public IEnumerator FadeCanvasGroup(CanvasGroup target, float duration, float targetAlpha)
//	{
//		if (target==null)
//			yield break;
//
//		float currentAlpha = target.alpha;
//
//		float t=0f;
//		while (t<1.0f)
//		{
//			if (target==null)
//				yield break;
//
//			float newAlpha =Mathf.SmoothStep(currentAlpha,targetAlpha,t);
//			target.alpha=newAlpha;
//
//			t += Time.deltaTime / duration;
//
//			yield return null;
//
//		}
//		target.alpha=targetAlpha;
//	}
}
