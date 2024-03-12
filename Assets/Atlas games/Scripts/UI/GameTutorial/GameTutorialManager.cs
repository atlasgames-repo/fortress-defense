using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTutorialManager : MonoBehaviour
{
    public GameTutorialSetup setup;
    public bool inMenu = false;
    void Start()
    {
       
        if (!inMenu)
        {
            GameObject obj = Instantiate(setup.SceneTutorial().gameObject,
                FindObjectOfType<Canvas>().transform.position, Quaternion.identity,
                FindObjectOfType<Canvas>().transform);
            obj.SetActive(true);
            obj.transform.SetSiblingIndex(FindObjectOfType<Canvas>().transform.childCount - 1);
        } 
    }
    GameObject _tutorialObj;
    public void StartTutorialInMenu(string placing)
    {
        if (PlayerPrefs.GetInt("tutorial " + placing) == 0)
        {
            for (int a = 0; a < setup.tutorials.Length; a++)
            {
                if (setup.tutorials[a].menuPlacing == placing)
                {
                    _tutorialObj = setup.tutorials[a].gameObject;
                }
            }
            print(_tutorialObj.gameObject.name);
            GameObject spawnedObj = Instantiate(_tutorialObj,
                FindObjectOfType<Canvas>().transform.position, Quaternion.identity,
                FindObjectOfType<Canvas>().transform);
            spawnedObj.SetActive(true);
            spawnedObj.transform.SetSiblingIndex(FindObjectOfType<Canvas>().transform.childCount - 1);
            PlayerPrefs.SetInt("tutorial " + placing, 1);
        }
    }


}
