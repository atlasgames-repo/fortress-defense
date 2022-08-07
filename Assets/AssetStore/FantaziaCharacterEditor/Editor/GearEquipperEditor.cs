using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Spine;
using Spine.Unity;

[CustomEditor(typeof(GearEquipper))]
public class GearEquipperEditor : Editor
{
    //Editor script for the GearEquipper class. It changes how the class looks in the inspector
    GearEquipper GE;

    public void OnEnable()
    {
        GE = (GearEquipper)target;
        GE.ApplySkinChanges();
    }


    public override void OnInspectorGUI()
    {
        //Override the base OnInspectorGUI showing the range of possible choices

        //Records the object for Undo purposes
        Undo.RecordObject(GE, "Character Changes");

        EditorGUI.BeginChangeCheck();

        GE.Job = (Jobs)EditorGUILayout.EnumPopup("Job", GE.Job);

        if (GE.Job == Jobs.Warrior)
        {
            GE.Melee = EditorGUILayout.IntSlider("Melee", GE.Melee, 0, 55);
            GE.Shield = EditorGUILayout.IntSlider("Shield", GE.Shield, 0, 36); //max +1
        }
        else if (GE.Job == Jobs.Archer)
        {
            GE.Bow = EditorGUILayout.IntSlider("Bow", GE.Bow, 0, 35);
            GE.Quiver = EditorGUILayout.IntSlider("Quiver", GE.Quiver, 0, 35);
        }
        else if (GE.Job == Jobs.Duelist )
        {
            GE.Melee = EditorGUILayout.IntSlider("Melee", GE.Melee, 0, 55);
            GE.DuelistOffhand = EditorGUILayout.IntSlider("Offhand", GE.DuelistOffhand, 0, 55);
        }
        else if (GE.Job == Jobs.Elementalist)
        {
            GE.Staff = EditorGUILayout.IntSlider("Staff", GE.Staff, 0, 35);
        }
        GE.Armor = EditorGUILayout.IntSlider("Armor", GE.Armor, 0, 35);
        GE.Helmet = EditorGUILayout.IntSlider("Helmet", GE.Helmet, 0, 37);
        GE.Shoulder = EditorGUILayout.IntSlider("Shoulder", GE.Shoulder, 0, 35);
        GE.Arm = EditorGUILayout.IntSlider("Arm", GE.Arm, 0, 35);
        GE.Feet = EditorGUILayout.IntSlider("Feet", GE.Feet, 0, 35);
        GE.Hair = EditorGUILayout.IntSlider("Hair", GE.Hair, 0, 23);
        GE.Face = EditorGUILayout.IntSlider("Face", GE.Face, 0, 23);


        GUILayout.Space(15);

        //Toggling IsAutoUpdate refreshes the skins automaticaily but takes more resources

        GE.IsAutoUpdate = EditorGUILayout.Toggle("Auto Apply Gear", GE.IsAutoUpdate);

        if (GE.IsAutoUpdate == false)
        {
            GUILayout.BeginHorizontal();

            GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
            myButtonStyle.fontSize = 16;
            myButtonStyle.fontStyle = FontStyle.Bold;
            myButtonStyle.fixedHeight = 30;
            myButtonStyle.fixedWidth = Screen.width;
            myButtonStyle.alignment = TextAnchor.MiddleCenter;


            GUI.backgroundColor = new Color(0.9f, 1, 0.9f);
            if (GUILayout.Button("Apply Gear on Character", myButtonStyle))
            {
                GE.ApplySkinChanges();
            }
            GUILayout.EndHorizontal();

        }
        else
        {
            if (GUI.changed && GE != null)
            {
                GE.ApplySkinChanges();
            }
        }



        if (GUI.changed && GE != null)
        {
            EditorUtility.SetDirty(GE);
        }


        if (EditorGUI.EndChangeCheck())
        {
            Undo.FlushUndoRecordObjects();
        }

    }


}
