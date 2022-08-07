using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarEnemyNew : MonoBehaviour {
	public Transform healthBar;
	public float showTime = 1f;
	public float hideSpeed = 0.5f;

	public SpriteRenderer backgroundImage;
	public SpriteRenderer barImage;

	Color oriBGImage, oriBarImage;

    Transform target;
    Vector3 offset;

	// Use this for initialization
	void Start () {
		healthBar.localScale = new Vector2 (1, healthBar.localScale.y);
		oriBGImage = backgroundImage.color;
		oriBarImage = barImage.color;

		//hide all
		backgroundImage.color = new Color (oriBGImage.r, oriBGImage.g, oriBGImage.b, 0);
		barImage.color = new Color (oriBarImage.r, oriBarImage.g, oriBarImage.b, 0);
	}

    public void Init(Transform _target, Vector3 _offset)
    {
        target = _target;
        offset = _offset;
    }

    private void Update()
    {
        if (target)
        {
            transform.position = target.position + offset;
        }
    }

    public void UpdateValue(float value){
		StopAllCoroutines ();
		CancelInvoke ();

		backgroundImage.color = oriBGImage;
		barImage.color = oriBarImage;

		value = Mathf.Max (0, value);
		healthBar.localScale = new Vector2 (value, healthBar.localScale.y);
		if (value > 0)
			Invoke ("HideBar", showTime);
		else
			gameObject.SetActive (false);
	}

	private void HideBar(){
		StartCoroutine (MMFade.FadeSpriteRenderer (backgroundImage, hideSpeed, new Color (oriBGImage.r, oriBGImage.g, oriBGImage.b, 0)));
		StartCoroutine (MMFade.FadeSpriteRenderer (barImage, hideSpeed, new Color (oriBarImage.r, oriBarImage.g, oriBarImage.b, 0)));
	}
}
