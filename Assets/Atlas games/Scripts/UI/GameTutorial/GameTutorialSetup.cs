using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTutorialSetup : MonoBehaviour
{
    public GameTutorial[] tutorials;
    public void Awake()
    {
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
}
