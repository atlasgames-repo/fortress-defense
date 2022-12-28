using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System;
using System.Reflection;
using System.Linq;
using UnityEditor.Experimental.SceneManagement;

public class CharacterDataEditorWindow : ExtendetCharacterEditorWindow
{

    Vector2 scrollPos;
    GameObject enemy;
    static CharacterDataEditorWindow window;
    // Start is called before the first frame update
    public static void Open(CharacterData level)
    {
        window = GetWindow<CharacterDataEditorWindow>("Level editor");
        window.serializedObject = new SerializedObject(level);
    }
    private void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, true);
        serializedProperty = serializedObject.FindProperty("enemyName");
        EditorGUILayout.PropertyField(serializedProperty);
        serializedProperty = serializedObject.FindProperty("attakType");
        EditorGUILayout.PropertyField(serializedProperty);
        EditorGUILayout.BeginVertical("box");
        switch (serializedProperty.enumValueIndex)
        {
            case 1:
                DrawProperties(serializedObject.FindProperty("enemyMeleeAttack"), true);
                break;
            case 2:
                DrawProperties(serializedObject.FindProperty("enemyThrowAttack"), true);
                break;
            case 0:
                DrawProperties(serializedObject.FindProperty("enemyRangeAttack"), true);
                break;
            default:
                break;
        }

        EditorGUILayout.EndVertical();

        serializedProperty = serializedObject.FindProperty("smartEnemyGrounded");
        DrawProperties(serializedProperty, true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("expMin"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("expMax"));
        EditorGUILayout.EndScrollView();
        EditorGUILayout.LabelField("\n\n\n\t\t\t\t Edits and saves \n\n\n");
        EditorGUILayout.BeginHorizontal("box");
        enemy = (GameObject)EditorGUILayout.ObjectField(enemy, typeof(GameObject), true);
        if (GUILayout.Button("Load Enemy data"))
        {
            if (enemy != null)
                DrawProperties(enemy, serializedProperty, true);
            Apply();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal("box");
        if (GUILayout.Button("save"))
        {
            Debug.LogError("Saved the data");
            Apply();
        }
        if (GUILayout.Button("Apply changes"))
        {
            Apply();
            if (!enemy)
            {
                EditorUtility.DisplayDialog("Apply changes", "First load up a character", "Ok", "Exit");
                return;
            }
            bool changed = EditorUtility.DisplayDialog("Apply changes", "Are you sure you want to change this character?", "Ok", "Exit");
            if (changed)
            {
                Save(enemy);
                Debug.LogError("Applied changes to the Enemy data");
            }
        }
        EditorGUILayout.EndHorizontal();
    }

}
