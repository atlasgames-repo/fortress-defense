using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTutorialSetup : MonoBehaviour
{
    public GameTutorial[] tutorials;
   public void Awake()
   {
       foreach (GameTutorial tutorial in tutorials)
       {
           if (tutorial.inMenu)
           {
               tutorial.gameObject.SetActive(false);
           }
       }
       DontDestroyOnLoad(gameObject);
   }

    public GameTutorial SceneTutorial()
    {
        
        foreach (var obj in tutorials)
        {
            if (obj.level == GlobalValue.levelPlaying)
                return obj;
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
