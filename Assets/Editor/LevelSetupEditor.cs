using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(LevelWave))]
public class LevelSetupEditor : Editor
{
    private SerializedProperty type;


    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        SerializedProperty levelType = serializedObject.FindProperty("type");
        SerializedProperty level = serializedObject.FindProperty("level");
        SerializedProperty defaultExp = serializedObject.FindProperty("defaultExp");
        SerializedProperty backgroundSprite = serializedObject.FindProperty("backgroundSprite");
        SerializedProperty initialWaitAmount = serializedObject.FindProperty("initialWaitAmount");
        SerializedProperty increaseEnemyWaitDifficultyRate = serializedObject.FindProperty("increaseEnemyWaitDifficultyRate");
        SerializedProperty increaseEnemyAmountDifficultyRate = serializedObject.FindProperty("increaseEnemyAmountDifficultyRate");
        SerializedProperty increaseEnemyHealthDifficultyRate = serializedObject.FindProperty("increaseEnemyHealthDifficultyRate");
        SerializedProperty increaseEnemyAttackDifficultyRate = serializedObject.FindProperty("increaseEnemyAttackDifficultyRate");
        SerializedProperty increaseEnemySpeedDifficultyRate = serializedObject.FindProperty("increaseEnemySpeedDifficultyRate");
        SerializedProperty enemiesList = serializedObject.FindProperty("enemiesList");


        string levelTypeString;
        EditorGUILayout.PropertyField(levelType);

        if (levelType.enumValueIndex == 1)
        {
            levelTypeString = "Normal Level";
        }
        else
        {
            levelTypeString = "Endless Level";
        }
        EditorGUILayout.LabelField(levelTypeString);

        SerializedProperty waves = serializedObject.FindProperty("Waves");

        if (levelType.enumValueIndex == 1)
        {
            EditorGUILayout.PropertyField(waves);
            EditorGUILayout.PropertyField(defaultExp);
        }
        else
        {
            EditorGUILayout.PropertyField(enemiesList);
            EditorGUILayout.PropertyField(initialWaitAmount);
            EditorGUILayout.PropertyField(increaseEnemyWaitDifficultyRate);
            EditorGUILayout.PropertyField(increaseEnemyAmountDifficultyRate);
            EditorGUILayout.PropertyField(increaseEnemyHealthDifficultyRate);
            EditorGUILayout.PropertyField(increaseEnemyAttackDifficultyRate);
            EditorGUILayout.PropertyField(increaseEnemySpeedDifficultyRate);
        }
        EditorGUILayout.PropertyField(level);
        EditorGUILayout.PropertyField(backgroundSprite);

        serializedObject.ApplyModifiedProperties();
    }
}

