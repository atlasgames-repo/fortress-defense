using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreenUI : MonoBehaviour {
	public static BlackScreenUI instance;
	CanvasGroup canvas;
	Image image;
	// Use this for initialization
	void Start () {
		instance = this;
		canvas = GetComponent<CanvasGroup> ();
		image = GetComponent<Image> ();
	}
	
	// Update is called once per frame
	public void Show (float timer, Color _color) {
		image.color = _color;
		canvas.alpha = 0;
		StartCoroutine (MMFade.FadeCanvasGroup (GetComponent<CanvasGroup> (), timer, 1));
	}

	public void Show (float timer) {
		image.color = Color.black;
		canvas.alpha = 0;
		StartCoroutine (MMFade.FadeCanvasGroup (GetComponent<CanvasGroup> (), timer, 1));
	}

	public void Hide (float timer) {
		canvas.alpha = 1;
		StartCoroutine (MMFade.FadeCanvasGroup (GetComponent<CanvasGroup> (), timer, 0));
	}
}
