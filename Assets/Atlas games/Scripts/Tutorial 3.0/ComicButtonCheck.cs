using UnityEngine;

public class ComicbuttonCheck : MonoBehaviour
{
    public static bool press = false;
    public static bool press2 = false;
    public void ComicButtonPress()
    {
        press = true;
        press2 = true;
        Debug.Log("pressed");
        Time.timeScale = 1;
    }
}
