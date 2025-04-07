using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DeviceDependentAttribute))]
public class DeviceDependentAttributeDrawer: PropertyDrawer {
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return EditorGUIUtility.singleLineHeight * 2 + 4;
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position.height = EditorGUIUtility.singleLineHeight;
        EditorGUI.LabelField(position, label);

        // Calculate rects with proper spacing
        Rect mobileRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, EditorGUIUtility.singleLineHeight);
        Rect pcRect = new Rect(position.x, position.y + (EditorGUIUtility.singleLineHeight * 2) + 4, position.width, EditorGUIUtility.singleLineHeight);

        SerializedProperty mobile_prop = property.FindPropertyRelative("mobile_object");
        SerializedProperty pc_prop = property.FindPropertyRelative("pc_object");

        if (mobile_prop == null || pc_prop == null) {
            EditorGUI.LabelField(position, "Error: Ensure the field is a DeviceDependentReference.");
            return;
        }

        GUIStyle boxStyle = new GUIStyle()
        {
            normal = new GUIStyleState()
            {
                background = CreateTexture(2,6, new Color(0.3f, 0.3f, 0.3f, 0f)), 
            },
            padding = new RectOffset(20, 10, 10, 10) 
        };

        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.PropertyField(mobileRect, mobile_prop, new GUIContent("Mobile Object"));
        EditorGUI.PropertyField(pcRect, pc_prop, new GUIContent("PC Object"));
        EditorGUI.EndProperty();
        EditorGUILayout.EndVertical();
    }
    private Texture2D CreateTexture(int width, int height, Color color)
    {
        Texture2D texture = new Texture2D(width, height);
        Color[] pixels = new Color[width * height];

        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = color;
        }

        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }
}
