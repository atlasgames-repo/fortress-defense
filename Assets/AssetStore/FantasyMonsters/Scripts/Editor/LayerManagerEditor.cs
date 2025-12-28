using UnityEditor;
using UnityEngine;

namespace Assets.FantasyMonsters.Scripts.Editor
{
    /// <summary>
    /// Add action buttons to LayerManager script
    /// </summary>
    [CustomEditor(typeof(LayerManager))]
    public class LayerManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var script = (LayerManager) target;

            if (GUILayout.Button("Read Sorting Order"))
            {
                script.GetSpritesBySortingOrder();
            }

            if (GUILayout.Button("Set Sorting Order"))
            {
                script.SetSpritesBySortingOrder();
            }
        }
    }
}