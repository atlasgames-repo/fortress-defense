using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hint : MonoBehaviour
{
    public TMP_Text hintText;
    public Transform TL_Pos;
    public Transform TM_Pos;
    public Transform TR_Pos;
    public Transform L_Pos;
    public Transform R_Pos;
    public Transform BL_Pos;
    public Transform BR_Pos;
    public Transform BM_Pos;

    public void Show(string text, Direction direction)
    {
        hintText.text = text;
        switch (direction)
        {
            case Direction.Left:
                
                break;
            case Direction.Right:
                
                break;
            case Direction.Top:
                break;
            case Direction.BottomLeft:
                break;
            case Direction.BottomRight: 
                break;
            case Direction.UpperLeft:
                
                break;
            case Direction.UpperRight :
                break;
            case Direction.Bottom:
                
                break;
        }
    }

    public void Hide()
    {
        
    }
}
