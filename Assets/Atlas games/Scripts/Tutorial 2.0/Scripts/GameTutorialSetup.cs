using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTutorialSetup : MonoBehaviour
{
    public GameObject[] tutorials;
   public void Awake()
   {
       DontDestroyOnLoad(gameObject);
   }

    public GameObject SceneTutorial()
    {
        foreach (GameObject obj in tutorials)
        {
            if (obj.GetComponent<TutorialNew>().tutorialLevel == GlobalValue.levelPlaying && obj.GetComponent<TutorialNew>().placing == TutorialPlacing.Game)
            {
                return obj;
            }
        }

        return null;
    }
    
}
