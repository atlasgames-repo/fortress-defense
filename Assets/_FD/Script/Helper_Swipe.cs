using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper_Swipe : MonoBehaviour
{
    float cameraLastPos;
    public float showHelperIfCameraIdle = 5;
    Transform cameraMain;
    float lastMoveTime = 0;
    public GameObject helperObj;
    bool isShown = false;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("DontShowAgain", 0) == 1)
        {
            helperObj.SetActive(false);
            Destroy(this);
        }
    }
    private void Start()
    {
        cameraMain = Camera.main.transform;
        cameraLastPos = cameraMain.position.x;
        InvokeRepeating("CheckingIdle", 5, 0.1f);
        lastMoveTime = Time.time;
        helperObj.SetActive(false);
    }
    void CheckingIdle()
    {
        if (GameManager.Instance.State == GameManager.GameState.Playing)
        {
            if (cameraLastPos != cameraMain.position.x)
            {
                cameraLastPos = cameraMain.position.x;
                lastMoveTime = Time.time;
                helperObj.SetActive(false);
            }
            else if (Time.time - lastMoveTime > showHelperIfCameraIdle)
            {
                if (!isShown)
                    helperObj.SetActive(true);
                isShown = true;
            }
        }else
            helperObj.SetActive(false);
    }

    public void DontShowAgain()
    {
        PlayerPrefs.SetInt("DontShowAgain", 1);
        helperObj.SetActive(false);
        Destroy(this);
    }
}
