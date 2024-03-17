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
                GameObject obj = Instantiate(setup.SceneTutorial().gameObject,
                    FindObjectOfType<Canvas>().transform.position, Quaternion.identity,
                    FindObjectOfType<Canvas>().transform);
                obj.GetComponent<GameTutorial>().enabled = true;
                obj.transform.SetSiblingIndex(FindObjectOfType<Canvas>().transform.childCount - 1);
                GlobalValue.SetTutorialState(GlobalValue.levelPlaying.ToString(),1);
            }
        }
    }
    GameObject _tutorialObj;
    public void StartTutorialInMenu(string placing)
    {
    //   if (GlobalValue.GetTutorialState(placing) == 0)
    //   {
            for (int a = 0; a < setup.tutorials.Length; a++)
            {
                if (setup.tutorials[a].GetComponent<GameTutorial>().menuPlacing == placing)
                {
                    _tutorialObj = setup.tutorials[a].gameObject;
                }
            }
            GameObject spawnedObj = Instantiate(_tutorialObj,
                FindObjectOfType<Canvas>().transform.position, Quaternion.identity,
                FindObjectOfType<Canvas>().transform);
            spawnedObj.GetComponent<GameTutorial>().enabled = true;
            spawnedObj.transform.SetSiblingIndex(FindObjectOfType<Canvas>().transform.childCount - 1);
            GlobalValue.SetTutorialState(placing,1);
            
       // }
    }


}
