using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hint : MonoBehaviour
{
    public TMP_Text hintText;
    
    private Animator _animator;
    private TutorialNew _tutorial;
    void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    public void Show(string text, Direction direction, TutorialNew tutorial)
    {
        _tutorial = tutorial;
        hintText.text = text;
        _animator.SetTrigger("Open");
    }

    public void Hide()
    {
        _animator.SetTrigger("Close");
    }

    public void OnAnimationFinished()
    {
        _tutorial.NextStep();
    }
}
