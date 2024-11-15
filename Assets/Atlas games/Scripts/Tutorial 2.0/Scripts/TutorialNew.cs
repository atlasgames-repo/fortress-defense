using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialNew : MonoBehaviour
{
    public Tip[] tutorialStep;
    public RectTransform circleMask;
    public Transform pointer;
    public float maskSpeed;
    public Dialog dialog;
    public Hint hint;
    public int tutorialLevel;
    public string tutorialPlacing;
    public Transform pointerObject;
    public Transform pointerIcon;
    public GameObject clickPreventer;
    public Color transparent;
    public Color darkBackground;
    private int _tipOrder;
    public float speed = 30f;
    public float initialWait = 0.5f;
    [HideInInspector] public string tutorialName;
    Camera _main;
}
