using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ScreenShake : MonoBehaviour {
    public static ScreenShake instance;

    private float shakeTimeRemaining, shakePower, shakeFadeTime, shakeRotation;

    public float rotationMultiplier = 15f;


    // Start is called before the first frame update
    void Start() {
        instance = this;
    }

    private IEnumerator ShakeCoroutine() {
        while (shakeTimeRemaining > 0f)
        {
            shakeTimeRemaining -= Time.deltaTime;

            float xAmount = Random.Range(-1f, 1f) * shakePower;
            float yAmount = Random.Range(-1f, 1f) * shakePower;

            transform.position += new Vector3(xAmount, yAmount, 0);

            shakePower = Mathf.MoveTowards(shakePower, 0f, shakeFadeTime * Time.deltaTime);

            shakeRotation = Mathf.MoveTowards(shakeRotation, 0f, shakeFadeTime * rotationMultiplier * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 0, shakeRotation * Random.Range(-1f, 1f));

            yield return null;
        }


    }

    public void StartShake(float length, float power) {
        shakeTimeRemaining = length;
        shakePower = power;

        shakeFadeTime = power / length;

        shakeRotation = power * rotationMultiplier;

        StartCoroutine(ShakeCoroutine());
    }

}