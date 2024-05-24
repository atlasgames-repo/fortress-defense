using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    public static CameraShake instance { get; private set; }

    private void Awake() {
        instance = this;
    }

    // Original position of the camera to reset after shaking
    private Vector3 originalPosition;

    // Internal timer to keep track of shake duration
    private float shakeTimeRemaining;
    // Magnitude of the shake effect
    private float shakeMagnitude;

    void Start() {
        // Store the original position of the camera
        originalPosition = transform.localPosition;
    }

    void Update() {
        // If there's remaining shake time, apply the shake effect
        if (shakeTimeRemaining > 0)
        {
            transform.localPosition = originalPosition + (Vector3)Random.insideUnitCircle * shakeMagnitude;
            shakeTimeRemaining -= Time.deltaTime;
        }
        else
        {
            // Reset to the original position once the shake effect is over
            transform.localPosition = originalPosition;
        }
    }

    // Method to start the camera shake with specified duration and magnitude
    public void StartShake(float duration, float magnitude) {
        shakeTimeRemaining = duration;
        shakeMagnitude = magnitude;
    }

}
