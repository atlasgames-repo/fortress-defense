using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeController : MonoBehaviour
{

    public static ScreenShakeController instance;

    public float shakeDuration = 0.5f;
    public float shakeAmount = 0.1f;

    private Vector3 originalPosition;
    private float remainingTime;

    private void Start() {
        instance = this;

        originalPosition = transform.localPosition;
    }

    void Update() {
        if (remainingTime > 0)
        {
            float x = Random.Range(-1f, 1f) * shakeAmount;
            float y = Random.Range(-1f, 1f) * shakeAmount;

            transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            remainingTime -= Time.deltaTime;
        }
        else
        {
            transform.localPosition = originalPosition;
        }
    }

    public void StartShake() {
        remainingTime = shakeDuration;
    }
}
