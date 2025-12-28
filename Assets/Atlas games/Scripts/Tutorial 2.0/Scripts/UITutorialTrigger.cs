using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITutorialTrigger : MonoBehaviour
{
    public string tutorialName;
    public bool runAutomatically = false;
    public void OpenTutorial(string name)
    {
        FindFirstObjectByType<GameTutorialManager>().StartTutorialInMenu(name);

    }

    void Start()
    {
        if (runAutomatically)
        {
            FindFirstObjectByType<GameTutorialManager>().StartTutorialInMenu(tutorialName);
        }
    }
}
