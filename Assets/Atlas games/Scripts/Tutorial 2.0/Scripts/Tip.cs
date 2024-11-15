using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Direction
{
    Top,
    Bottom,
    Left,
    Right,
    UpperLeft,
    UpperRight,
    BottomLeft,
    BottomRight
}

public enum TutorialPlacing
{
    Menu,Game   
}
public enum TipType
{
    Dialog,
    Tip,
    Task
}
public class Tip
{
    public Direction tipDirection;
    public TipType tipType;
    public Direction pointerDirection;
    public float delay;
    public bool isLastDialog;
    public Dialog dialog;
    public string uiPartName;
    public float circleMaskScale;
    public bool isUiInteractible;
    public bool pauseGame;
    public Hint hint;
}
