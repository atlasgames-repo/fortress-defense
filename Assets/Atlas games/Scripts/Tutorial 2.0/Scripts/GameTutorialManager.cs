using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTutorialManager : MonoBehaviour
{
    public GameTutorialSetup setup;
    void Start()
    {
        StartTutorial();
    }

    public void StartTutorial()
    {
        if (GameTutorialSetup.self == null) return;
        setup = GameTutorialSetup.self;
        if (GlobalValue.GetTutorialState(GlobalValue.levelPlaying.ToString()) != 0) return;
        GameObject setupObj = setup.SceneTutorial();
        if (!setupObj) return;
        if (MenuManager.Instance == null) return;
        MenuManager manager = MenuManager.Instance;
        GameObject obj = Instantiate(setupObj, manager.transform.position, Quaternion.identity, manager.transform);
        obj.SetActive(true);
        obj.transform.SetSiblingIndex(manager.transform.childCount - 1);
        obj.GetComponent<TutorialNew>().InitTutorial();

        GlobalValue.SetTutorialState(GlobalValue.levelPlaying.ToString(), 1);
        return;
    }

    GameObject _tutorialObj;
    public void StartTutorialInMenu(string placing)
    {
       if (GlobalValue.GetTutorialState(placing) == 0)
       {
            for (int a = 0; a < setup.tutorials.Length; a++)
            {
                var tutorial = setup.tutorials[a].GetComponent<TutorialNew>();
                if (tutorial.placing == TutorialPlacing.Menu && tutorial.tutorialName == placing && tutorial.AfterLevel < GlobalValue.LevelPass)
                {
                    _tutorialObj = setup.tutorials[a].gameObject;
                }
            }
            if (_tutorialObj == null || MainMenuHomeScene.Instance == null)
                return;
            GameObject spawnedObj = Instantiate(_tutorialObj,
            MainMenuHomeScene.Instance.transform.position, Quaternion.identity,
            MainMenuHomeScene.Instance.transform);
            spawnedObj.SetActive(true);
            spawnedObj.transform.SetSiblingIndex(MainMenuHomeScene.Instance.transform.childCount - 1);
            TutorialNew tutorialNew;
            spawnedObj.TryGetComponent(out tutorialNew);
            if (tutorialNew == null)
                return;
            tutorialNew.InitTutorial();
            GlobalValue.SetTutorialState(placing,1);
            
        }
    }
}
