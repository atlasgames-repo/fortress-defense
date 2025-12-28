using UnityEditor;
using UnityEngine;

namespace Assets.FantasyMonsters.Scripts.Editor
{
    /// <summary>
    /// Add action buttons to LayerManager script
    /// </summary>
    [CustomEditor(typeof(Demo))]
    public class DemoEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var script = (Demo) target;

            if (GUILayout.Button("Refresh"))
            {
                script.OnValidate();
            }
        }
    }
}