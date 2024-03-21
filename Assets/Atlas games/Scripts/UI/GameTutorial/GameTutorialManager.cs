using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTutorialManager : MonoBehaviour
{
    public GameTutorialSetup setup;
    public bool inMenu = false;
    
    void Start()
    {

        setup = FindObjectOfType<GameTutorialSetup>();
       
        if (!inMenu)
        {
         if (GlobalValue.GetTutorialState(GlobalValue.levelPlaying.ToString()) == 0)
         {
             GameObject obj = Instantiate(setup.SceneTutorial(),
                 FindObjectOfType<MenuManager>().transform.position, Quaternion.identity,
                 FindObjectOfType<MenuManager>().transform);
             obj.GetComponent<GameTutorial>().enabled = true;
             obj.transform.SetSiblingIndex(FindObjectOfType<MenuManager>().transform.childCount - 1);
             GlobalValue.SetTutorialState(GlobalValue.levelPlaying.ToString(),1);
         }
        GameObject.FindWithTag("Pointer").transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;

        }
    }



    GameObject _tutorialObj;
    public void StartTutorialInMenu(string placing)
    {
       if (GlobalValue.GetTutorialState(placing) == 0)
       {
            for (int a = 0; a < setup.tutorials.Length; a++)
            {
                if (setup.tutorials[a].GetComponent<GameTutorial>().menuPlacing == placing)
                {
                    _tutorialObj = setup.tutorials[a].gameObject;
                }
            }
            GameObject spawnedObj = Instantiate(_tutorialObj,
                FindObjectOfType<MainMenuHomeScene>().transform.position, Quaternion.identity,
                FindObjectOfType<MainMenuHomeScene>().transform);
            spawnedObj.GetComponent<GameTutorial>().enabled = true;
            spawnedObj.transform.SetSiblingIndex(FindObjectOfType<MainMenuHomeScene>().transform.childCount - 1);
            GlobalValue.SetTutorialState(placing,1);
            
        }
    }


}
