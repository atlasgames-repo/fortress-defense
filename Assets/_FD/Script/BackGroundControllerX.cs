using UnityEngine;
using System.Collections;

public class BackGroundControllerX : MonoBehaviour {
	public bool autoScrolling = true;
    public float autoScrollingSpeed = 1;

    public enum Follow{FixedUpdate,Update}
	public Follow timeBase;
	public Renderer Background;
	public float speedBG = 0.1f;
	public Renderer Midground;
    public float speedMG = 0.2f;
	public Renderer Forceground;
    public float speedFG = 0.3f;

	Camera target;
	float startPosX;
    float x;

    // Use this for initialization
    void Start () {
		target = Camera.main;
		startPosX = target.transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		if (timeBase != Follow.Update)
			return;

        if (autoScrolling)
            x += Time.deltaTime * autoScrollingSpeed;
        else
            x = target.transform.position.x - startPosX;

		if (Background != null) {
			var offset = (x * speedBG) % 1;
			Background.material.mainTextureOffset = new Vector2 (offset, Background.material.mainTextureOffset.y);
		}
		if (Midground != null) {
			var offset = (x * speedMG) % 1;
			Midground.material.mainTextureOffset = new Vector2 (offset, Midground.material.mainTextureOffset.y);
		}
		if (Forceground != null) {
			var offset = (x * speedFG) % 1;
			Forceground.material.mainTextureOffset = new Vector2 (offset, Forceground.material.mainTextureOffset.y);
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (timeBase != Follow.FixedUpdate)
			return;

        if (autoScrolling)
            x += Time.fixedDeltaTime * autoScrollingSpeed;
        else
            x = target.transform.position.x - startPosX;

		if (Background != null) {
			var offset = (x * speedBG) % 1;
			Background.material.mainTextureOffset = new Vector2 (offset, Background.material.mainTextureOffset.y);
		}
		if (Midground != null) {
			var offset = (x * speedMG) % 1;
			Midground.material.mainTextureOffset = new Vector2 (offset, Midground.material.mainTextureOffset.y);
		}
		if (Forceground != null) {
			var offset = (x * speedFG) % 1;
			Forceground.material.mainTextureOffset = new Vector2 (offset, Forceground.material.mainTextureOffset.y);
		}
	}
}
