using UnityEngine;

public class buttonCheck : MonoBehaviour
{
    public static bool press = false;
    public static bool press2 = false;
    public void buttonPress()
    {
        press = true;
        press2 = true;
        Debug.Log("pressed");
        Time.timeScale = 1;
    }


}
