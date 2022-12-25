using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class AssetHandlerCharacter
{
    [OnOpenAsset()]
    public static bool OpenEditor(int instanceId, int line)
    {
        CharacterData obj = EditorUtility.InstanceIDToObject(instanceId) as CharacterData;
        if (obj != null)
        {
            CharacterDataEditorWindow.Open(obj);
            return true;
        }
        return false;
    }
}
[CustomEditor(typeof(CharacterData))]
public class CharacterDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Open Editor :)"))
            CharacterDataEditorWindow.Open((CharacterData)target);
    }
}
