using System;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine;

[CustomEditor(typeof(GameTutorial))]
public class TipEditor : Editor
{
    private SerializedProperty _type;

    // if tip type is dialog : 
    private GameObject _dialogDescription;

    //   private Animator _dialog;
    private string _openTrigger;
    private string _closeTrigger;
    private bool _isLast;

    // if tip type is task : 
    private float _delay;
    private Transform _pointerObject;
    private SerializedProperty _direction;
    private string _buttonName;
    
    // if tip type is just tip : 
    private Animator _tipAnimator;
    private string _uiPartName;
    private float _scale = 0;
    private bool _isUiInteractible;
    private bool _isGamePaused = false;

    // properties
    public List<GameTutorial.TipSetup> setups;
    private GameTutorial.TipSetup _setup;
    private GameTutorial _gt;

    private void OnEnable()
    {
        _setup = new GameTutorial.TipSetup();
        setups = new List<GameTutorial.TipSetup>();
        _gt = (GameTutorial)target;
        setups = _gt.tipsList;
        _gt.tipsList = setups;
        _type = serializedObject.FindProperty("type");
        _direction = serializedObject.FindProperty("direction");
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Space(10);

        EditorGUILayout.LabelField("Update script variables", EditorStyles.boldLabel);
        GUILayout.Space(10);

        serializedObject.Update();
        EditorGUILayout.PropertyField(_type);

        if (_gt.type == GameTutorial.TipType.Dialog)
        {
            _isLast = EditorGUILayout.Toggle("Is the last tip of dialog", _isLast);
            if (_isLast)
            {
                _closeTrigger = EditorGUILayout.TextField("Animation Close Trigger", _closeTrigger);
            }

            _openTrigger = EditorGUILayout.TextField("Animation Open Trigger", _openTrigger);
            _tipAnimator = (Animator)EditorGUILayout.ObjectField("Tip Animation", _tipAnimator, typeof(Animator), true);
        }
        else if (_gt.type == GameTutorial.TipType.Task)
        {
            _isUiInteractible = EditorGUILayout.Toggle("Can Interact with UI ", _isUiInteractible);
            _isGamePaused = EditorGUILayout.Toggle("Pause game showing task", _isGamePaused);
            EditorGUILayout.PropertyField(_direction);
            _delay = EditorGUILayout.FloatField("Start Pointing delay", _delay);
            _buttonName = EditorGUILayout.TextField("Target button name", _buttonName);
        }
        else if (_gt.type == GameTutorial.TipType.Tip)
        {
            _tipAnimator = (Animator)EditorGUILayout.ObjectField("Tip Animation", _tipAnimator, typeof(Animator), true);
            _uiPartName = EditorGUILayout.TextField("Target UI element name", _uiPartName);
            _scale = EditorGUILayout.FloatField("Mask Scale ", _scale);
            _closeTrigger = EditorGUILayout.TextField("Animation Close Trigger", _closeTrigger);
            _openTrigger = EditorGUILayout.TextField("Animation Open Trigger", _openTrigger);
        }

        GameTutorial script = (GameTutorial)target;
        GUILayout.Space(10);
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Add Tip", GUILayout.Height(30)))
        {
            Reset();
            if (_gt.type == GameTutorial.TipType.Dialog)
            {
                if ((_isLast && _openTrigger != null && _closeTrigger != null) || _openTrigger != null)
                {
                    _setup.openTrigger = _openTrigger;
                    _setup.tipAnimator = _tipAnimator;
                    _setup.closeTrigger = _closeTrigger;
                    _setup.type = _gt.type.ToString();
                    _setup.isLastDialog = _isLast;
                    setups.Add(_setup);
                }
            }
            else if (_gt.type == GameTutorial.TipType.Task)
            {
                if (_buttonName != null && _direction != null)
                {
                    _setup.delay = _delay;
                    _setup.pointerDirection = _gt.direction.ToString();
                    _setup.type = _gt.type.ToString();
                    _setup.isUiInteractible = _isUiInteractible;
                    _setup.pauseGame = _isGamePaused;
                    _setup.uiPartName = _buttonName;
                    setups.Add(_setup);
                }
            }
            else if (_gt.type == GameTutorial.TipType.Tip)
            {
                if (_openTrigger != null && _closeTrigger != null && _uiPartName !=null && _tipAnimator)
                {
                    _setup.tipAnimator = _tipAnimator;
                    _setup.uiPartName = _uiPartName;
                    _setup.scale = _scale;
                    _setup.openTrigger = _openTrigger;
                    _setup.closeTrigger = _closeTrigger;
                    _setup.type = _gt.type.ToString();
                    setups.Add(_setup);
                }
            }
        }

        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("Reset", GUILayout.Height(30)))
        {
            Reset();
            setups.Clear();
        }

        GUI.backgroundColor = Color.white;
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Current tutorial steps", EditorStyles.boldLabel);
        GUILayout.Space(10);
        serializedObject.ApplyModifiedProperties();
        _gt.tipsList = setups;
        DrawDefaultInspector();
    }

    private void Reset()
    {
        try
        {
            _setup.openTrigger = null;
            _setup.closeTrigger = null;
            _setup.isLastDialog = false;
            _setup.pointerDirection = null;
            _setup.delay = 0.5f;
            _setup.tipAnimator = null;
            _setup.uiPartName= null;
            _setup.scale = 0;
            _setup.isUiInteractible = false;
            _setup.type = null;
            _setup.pauseGame = false;
        }
        catch (Exception e)
        {
        }
    }
}