using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hint : MonoBehaviour
{
    public TMP_Text hintText;
    
    private Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    public void Show(string text, Direction direction)
    {
        hintText.text = text;
        _animator.SetTrigger("Open");
        switch (direction)
        {
         //   case Direction.Left:
         //       
         //       break;
         //   case Direction.Right:
         //       
         //       break;
         //   case Direction.Top:
         //       break;
         //   case Direction.BottomLeft:
         //       break;
         //   case Direction.BottomRight: 
         //       break;
         //   case Direction.UpperLeft:
         //       
         //       break;
         //   case Direction.UpperRight :
         //       break;
         //   case Direction.Bottom:
         //       
         //       break;
        }
    }

    public void Hide()
    {
        _animator.SetTrigger("Close");
    }
}
