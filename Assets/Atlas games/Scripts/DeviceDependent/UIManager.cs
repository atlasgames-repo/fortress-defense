using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject MobileUI, PCUI;

    private void Awake() {
        if (SystemInfo.deviceType == DeviceType.Handheld) {
            MobileUI.SetActive(true);
            PCUI.SetActive(false);
        } else {
            MobileUI.SetActive(false);
            PCUI.SetActive(true);
        }
    }
}
