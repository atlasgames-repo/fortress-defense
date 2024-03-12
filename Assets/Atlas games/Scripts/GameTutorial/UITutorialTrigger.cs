using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITutorialTrigger : MonoBehaviour
{
    public string tutorialName;
    public bool runAutomatically = false;
    public void OpenTutorial(string name)
    {
        FindObjectOfType<GameTutorialManager>().StartTutorialInMenu(name);

    }

    void Start()
    {
        if (runAutomatically)
        {
            print("step 1");
            FindObjectOfType<GameTutorialManager>().StartTutorialInMenu(tutorialName);
        }
    }
}
