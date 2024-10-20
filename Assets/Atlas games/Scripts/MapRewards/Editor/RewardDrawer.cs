using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Reward))]
public class RewardDrawer : PropertyDrawer
{
    // Override the OnGUI method to customize the inspector
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Begin the property drawing
        EditorGUI.BeginProperty(position, label, property);

        // Calculate the positions for the fields
        Rect rewardLevelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        Rect typeRect = new Rect(position.x, position.y + 20, position.width, EditorGUIUtility.singleLineHeight);
        Rect amountRect = new Rect(position.x, position.y + 40, position.width, EditorGUIUtility.singleLineHeight);

        // Get the properties
        SerializedProperty rewardLevel = property.FindPropertyRelative("rewardLevel");
        SerializedProperty type = property.FindPropertyRelative("type");
        SerializedProperty shopItemName = property.FindPropertyRelative("shopItemName");
        SerializedProperty amount = property.FindPropertyRelative("amount");
        SerializedProperty icon = property.FindPropertyRelative("icon");

        // Draw the fields
        EditorGUI.PropertyField(rewardLevelRect, rewardLevel);
        EditorGUI.PropertyField(typeRect, type);
        EditorGUI.PropertyField(amountRect, amount);

        // Only show the shopItemName and icon fields if type is set to ShopItem
        if (type.enumValueIndex == (int)RewardType.ShopItem)
        {
            Rect shopItemNameRect = new Rect(position.x, position.y + 60, position.width, EditorGUIUtility.singleLineHeight);
            Rect iconRect = new Rect(position.x, position.y + 80, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(shopItemNameRect, shopItemName);
            EditorGUI.PropertyField(iconRect, icon);
        }

        // End the property drawing
        EditorGUI.EndProperty();
    }

    // Override GetPropertyHeight to calculate the correct height for the drawer
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty type = property.FindPropertyRelative("type");

        // If type is ShopItem, add extra height for shopItemName and icon
        if (type.enumValueIndex == (int)RewardType.ShopItem)
        {
            return EditorGUIUtility.singleLineHeight * 5.5f; // Adjust this to match the number of visible lines
        }

        return EditorGUIUtility.singleLineHeight * 3.2f; // Height for rewardLevel, type, and amount
    }
}
