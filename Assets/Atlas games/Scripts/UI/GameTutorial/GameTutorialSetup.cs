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
            if (obj.GetComponent<GameTutorial>().level == GlobalValue.levelPlaying)
            {
                return obj;
            }
        }

        return null;
    }
}
