using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hint : MonoBehaviour
{
    public TMP_Text hintText;
    public TMP_Text titleText;
    public float distanceFromMask = 140f;
    public float mainMaskRadius = 50f;
    private Animator _animator;
    private TutorialNew _tutorial;
    private bool _isOpen;
    void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    public void Show(string text, Direction direction, TutorialNew tutorial, Vector3 maskPosition, float maskScale, int tipIndex)
    {
        titleText.text = "#Tip #" + (tipIndex + 1).ToString();
        Vector3 newHintPosition = Vector3.zero;
        _tutorial = tutorial;
        hintText.text = text;
        _isOpen = true;
        _animator.SetTrigger("Open");
        float maskDistance = mainMaskRadius * maskScale + distanceFromMask;
        newHintPosition = maskPosition + new Vector3( (direction == Direction.Left || direction == Direction.BottomLeft || direction== Direction.UpperLeft ? - maskDistance : 0f) + (direction==Direction.BottomRight || direction == Direction.UpperRight || direction == Direction.Right ? maskDistance : 0f ) ,((direction== Direction.BottomRight|| direction== Direction.BottomLeft || direction== Direction.Bottom? -maskDistance : 0f)+(direction== Direction.UpperRight || direction== Direction.UpperLeft || direction == Direction.Top ? maskDistance : 0f)) ,0f);
        transform.position = newHintPosition;
    }

    public void Hide()
    {
        _isOpen = false;
        _animator.SetTrigger("Close");
    }

    public void OnAnimationFinished()
    {
        if (!_isOpen)
        {
            _tutorial.NextStep();
        }
    }
}
