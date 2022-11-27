using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class AssetHandler
{
    [OnOpenAsset()]
    public static bool OpenEditor(int instanceId, int line)
    {
        LevelData obj = EditorUtility.InstanceIDToObject(instanceId) as LevelData;
        if (obj != null)
        {
            LevelDataEditorWindow.Open(obj);
            return true;
        }
        return false;
    }
}

[CustomEditor(typeof(LevelData))]
public class LevelDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Open Editor :)"))
            LevelDataEditorWindow.Open((LevelData)target);
    }
}
