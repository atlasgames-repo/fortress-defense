using UnityEngine;

public class uiElements : MonoBehaviour
{
    public static float aspectRatio = 0f;

    void Start()
    {
        if (Camera.main.aspect >= 1.7)
        {
            aspectRatio = 169f;
            Debug.Log("16:9");
        }
        else if (Camera.main.aspect >= 1.5)
        {
            aspectRatio = 32f;
            Debug.Log("3:2");
        }
        else
        {
            aspectRatio = 43f;
            Debug.Log("4:3");
        }
    }
}
