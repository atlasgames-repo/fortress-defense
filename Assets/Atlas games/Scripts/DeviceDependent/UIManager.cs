using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject MobileUI, PCUI;
    public bool reverse_test;

    public static bool IsReversed {
        get {
            UIManager uIManager = FindFirstObjectByType<UIManager>();
            if (uIManager) return uIManager.reverse_test;
            else return false;
        }
    }

    private void Awake()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld || reverse_test)
        {
            MobileUI.SetActive(true);
            PCUI.SetActive(false);
        }
        else
        {
            MobileUI.SetActive(false);
            PCUI.SetActive(true);
        }
    }
}
