using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTutorialSetup : MonoBehaviour
{
    public static GameTutorialSetup self;
    public GameObject[] tutorials;
   public void Awake() {
       if (self == null) {
           self = this;
           DontDestroyOnLoad(gameObject);
       } else {
           Destroy(gameObject);
       }
   }

    public GameObject SceneTutorial() {
        foreach (GameObject obj in tutorials) {
            obj.TryGetComponent(out TutorialNew tutorial);
            if (tutorial == null) continue;
            if (
                tutorial.tutorialLevel == GlobalValue.levelPlaying &&
                tutorial.placing == TutorialPlacing.Game
            ) {
                return obj;
            }
        }

        return null;
    }
}
