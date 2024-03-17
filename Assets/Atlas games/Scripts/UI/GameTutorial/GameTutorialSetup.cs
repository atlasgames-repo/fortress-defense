using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTutorialSetup : MonoBehaviour
{
    public GameObject[] tutorials;
   public void Awake()
   {
     //  foreach (GameObject tutorial in tutorials)
     //  {
     //      if (tutorial.GetComponent<GameTutorial>().inMenu)
     //      {
     //          tutorial.gameObject.SetActive(false);
     //      }
   //    }
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

 // public GameTutorial MenuTutorial()
 // {
 //     foreach (var obj in  tutorials)
 //     {
 //         if (obj.menuPlacing == GlobalValue.menuTutorial)
 //         {
 //             return obj;
 //         }        
 //     }

 //     return null;
 // }
}
