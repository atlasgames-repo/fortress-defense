using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

[CustomEditor(typeof(TutorialNew))]
public class TutorialNewInspector : Editor
{
    private Tip newTip;
    private bool showAddTipSection = false;
    private string errorMessage = "";

    private void OnEnable()
    {
        // Initialize newTip with a fresh Tip instance
        newTip = new Tip();
    }

  public override void OnInspectorGUI()
{
    TutorialNew tutorial = (TutorialNew)target;

    // Display existing tutorial steps as read-only
    EditorGUILayout.LabelField("Tutorial Steps", EditorStyles.boldLabel);
    if (tutorial.tutorialStep != null && tutorial.tutorialStep.Length > 0)
    {
        for (int i = 0; i < tutorial.tutorialStep.Length; i++)
        {
            // Ensure stepText is properly initialized and doesn't show as "Empty" if not necessary
            string stepText = string.IsNullOrEmpty(tutorial.tutorialStep[i]?.tipText) 
                ? $"Step {i + 1} " + tutorial.tutorialStep[i].tipType
                : tutorial.tutorialStep[i].tipType.ToString();

            EditorGUILayout.LabelField($"Step {i + 1}", stepText);
        }
    }
    else
    {
        EditorGUILayout.LabelField("No tutorial steps available.");
    }

    GUILayout.Space(10);

    // Toggle section for adding a new tip
    showAddTipSection = EditorGUILayout.Foldout(showAddTipSection, "Add New Tip");
    if (showAddTipSection)
    {
        // Render newTip fields based on TipType
        EditorGUILayout.LabelField("New Tip Details", EditorStyles.boldLabel);
        newTip.tipType = (TipType)EditorGUILayout.EnumPopup("Tip Type", newTip.tipType);

        switch (newTip.tipType)
        {
            case TipType.Dialog:
                newTip.dialogContentType = (DialogContent)EditorGUILayout.EnumPopup("Dialog Content Type", newTip.dialogContentType);
                newTip.tipTitle = EditorGUILayout.TextField("Tip Title", newTip.tipTitle);
                newTip.tipText = EditorGUILayout.TextField("Tip Text", newTip.tipText);
                newTip.dialog = (Dialog)EditorGUILayout.ObjectField("Dialog", newTip.dialog, typeof(Dialog), false);
                newTip.isLastDialog = EditorGUILayout.Toggle("Is Last Dialog", newTip.isLastDialog);
                newTip.isUiInteractible = EditorGUILayout.Toggle("Is UI Interactible", newTip.isUiInteractible);
                newTip.pauseGame = EditorGUILayout.Toggle("Pause Game", newTip.pauseGame);

                if (newTip.dialogContentType == DialogContent.Video)
                {
                    newTip.dialogVideo = (VideoClip)EditorGUILayout.ObjectField("Dialog Video", newTip.dialogVideo, typeof(VideoClip), false);
                }
                else if (newTip.dialogContentType == DialogContent.Image)
                {
                    newTip.dialogImage = (Sprite)EditorGUILayout.ObjectField("Dialog Image", newTip.dialogImage, typeof(Sprite), false);
                }
                break;

            case TipType.Hint:
                newTip.tipText = EditorGUILayout.TextField("Tip Text", newTip.tipText);
                newTip.tipDirection = (Direction)EditorGUILayout.EnumPopup("Tip Direction", newTip.tipDirection);
                newTip.uiPartName = EditorGUILayout.TextField("UI Part Name", newTip.uiPartName);
                newTip.circleMaskScale = EditorGUILayout.FloatField("Circle Mask Scale", newTip.circleMaskScale);
                newTip.pauseGame = EditorGUILayout.Toggle("Pause Game", newTip.pauseGame);
                break;

            case TipType.Task:
                newTip.uiPartName = EditorGUILayout.TextField("UI Part Name", newTip.uiPartName);
                newTip.isUiInteractible = EditorGUILayout.Toggle("Is UI Interactible", newTip.isUiInteractible);
                newTip.delay = EditorGUILayout.FloatField("Delay", newTip.delay);
                newTip.pointerDirection = (Direction)EditorGUILayout.EnumPopup("Pointer Direction", newTip.pointerDirection);
                break;
        }

        // Error Message Section - Display above the Add Tip Button
        if (!string.IsNullOrEmpty(errorMessage))
        {
            EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
            GUILayout.Space(10);  // Add some space after the error message
        }

        // Add Tip Button (Green)
        GUIStyle greenButtonStyle = new GUIStyle(GUI.skin.button);
        greenButtonStyle.normal.textColor = Color.white;
        greenButtonStyle.normal.background = MakeTex(2, 2, Color.green);

        if (GUILayout.Button("Add Tip", greenButtonStyle, GUILayout.Height(30)))
        {
            if (ValidateNewTip(newTip))
            {
                // Clone and add the tip to avoid reference issues
                ArrayUtility.Add(ref tutorial.tutorialStep, CloneTip(newTip));
                newTip = new Tip(); // Reset the new tip
                errorMessage = ""; // Clear error message

                // Ensure the array is updated and the inspector is refreshed
                EditorUtility.SetDirty(tutorial);
            }
            else
            {
                errorMessage = "Please fill all required fields for the selected Tip Type.";
            }
        }

        GUILayout.Space(10); // Add some space before the Reset button

        // Reset Button (Red)
        GUIStyle redButtonStyle = new GUIStyle(GUI.skin.button);
        redButtonStyle.normal.textColor = Color.white;
        redButtonStyle.normal.background = MakeTex(2, 2, Color.red);

        if (GUILayout.Button("Reset Tips", redButtonStyle, GUILayout.Height(30)))
        {
            // Clear the array of tutorial steps
            tutorial.tutorialStep = new Tip[0];

            // Refresh the editor
            EditorUtility.SetDirty(tutorial);
        }
    }

    // Save changes to the TutorialNew object
    if (GUI.changed)
    {
        EditorUtility.SetDirty(tutorial);
    }
}

// Utility function to create a colored button texture
    private Texture2D MakeTex(int width, int height, Color color)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; i++) pix[i] = color;
        Texture2D texture = new Texture2D(width, height);
        texture.SetPixels(pix);
        texture.Apply();
        return texture;
    }
    private bool ValidateNewTip(Tip tip)
    {
        // Validation logic for required fields
        switch (tip.tipType)
        {
            case TipType.Dialog:
                return !string.IsNullOrEmpty(tip.tipText) && !string.IsNullOrEmpty(tip.tipTitle) &&
                       tip.dialog != null && ((tip.dialogContentType == DialogContent.Video && tip.dialogVideo != null) ||
                                              (tip.dialogContentType == DialogContent.Image && tip.dialogImage != null));
            case TipType.Hint:
                return !string.IsNullOrEmpty(tip.tipText) && !string.IsNullOrEmpty(tip.uiPartName);
            case TipType.Task:
                return !string.IsNullOrEmpty(tip.uiPartName);
            default:
                return false;
        }
    }

    private Tip CloneTip(Tip original)
    {
        // Create a new instance of Tip and copy properties to avoid reference issues
        return new Tip
        {
            tipType = original.tipType,
            tipText = original.tipText,
            tipTitle = original.tipTitle,
            dialog = original.dialog,
            dialogContentType = original.dialogContentType,
            dialogImage = original.dialogImage,
            dialogVideo = original.dialogVideo,
            isLastDialog = original.isLastDialog,
            isUiInteractible = original.isUiInteractible,
            pauseGame = original.pauseGame,
            tipDirection = original.tipDirection,
            pointerDirection = original.pointerDirection,
            uiPartName = original.uiPartName,
            circleMaskScale = original.circleMaskScale,
            delay = original.delay
        };
    }
}
