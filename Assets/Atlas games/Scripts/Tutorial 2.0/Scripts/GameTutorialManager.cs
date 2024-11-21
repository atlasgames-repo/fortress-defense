using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTutorialManager : MonoBehaviour
{
    public GameTutorialSetup setup;
    public float time;
    void Start() {
            setup = FindObjectOfType<GameTutorialSetup>();
            print(GlobalValue.levelPlaying);
            if (GlobalValue.GetTutorialState(GlobalValue.levelPlaying.ToString()) == 0) {
                GameObject setupObj = setup.SceneTutorial();
                if (!setupObj) return;
                GameObject obj = Instantiate(setup.SceneTutorial(), FindObjectOfType<MenuManager>().transform.position, Quaternion.identity, FindObjectOfType<MenuManager>().transform);
                obj.transform.SetSiblingIndex(FindObjectOfType<MenuManager>().transform.childCount - 1);
                obj.GetComponent<TutorialNew>().InitTutorial();
                GlobalValue.SetTutorialState(GlobalValue.levelPlaying.ToString(),1);
            }
    }

    void Update()
    {
        time = Time.timeScale;
    }

    GameObject _tutorialObj;
    public void StartTutorialInMenu(string placing)
    {
       if (GlobalValue.GetTutorialState(placing) == 0)
       {
            for (int a = 0; a < setup.tutorials.Length; a++)
            {
                if (setup.tutorials[a].GetComponent<TutorialNew>().placing == TutorialPlacing.Menu && setup.tutorials[a].GetComponent<TutorialNew>().tutorialName == placing)
                {
                    _tutorialObj = setup.tutorials[a].gameObject;
                }
            }
            GameObject spawnedObj = Instantiate(_tutorialObj,
            FindObjectOfType<MainMenuHomeScene>().transform.position, Quaternion.identity,
            FindObjectOfType<MainMenuHomeScene>().transform);
            spawnedObj.transform.SetSiblingIndex(FindObjectOfType<MainMenuHomeScene>().transform.childCount - 1);
            spawnedObj.GetComponent<TutorialNew>().InitTutorial();
            GlobalValue.SetTutorialState(placing,1);
            
        }
    }
}
