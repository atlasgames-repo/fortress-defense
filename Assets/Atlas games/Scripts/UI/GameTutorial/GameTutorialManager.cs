using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTutorialManager : MonoBehaviour
{
    private GameTutorialSetup _setup;
    public bool inMenu = false;
    void Start()
    {
        StartToturial();
    }
    public void StartToturial(){
       _setup =  FindObjectOfType<GameTutorialSetup>();
       if (!inMenu)
       {
           GameObject obj = Instantiate(_setup.SceneTutorial().gameObject,
               FindObjectOfType<Canvas>().transform.position, Quaternion.identity,
               FindObjectOfType<Canvas>().transform);
           obj.SetActive(true);
           obj.transform.SetSiblingIndex(FindObjectOfType<Canvas>().transform.childCount - 1);
       } 
    }

    public void StartTutorialInMenu(string placing)
    {
        if (PlayerPrefs.GetInt("tutorial " + placing) == 0)
        {

            _setup = FindObjectOfType<GameTutorialSetup>();
            GameObject obj;
            for (int a = 0; a < _setup.tutorials.Length; a++)
            {
                if (_setup.tutorials[a].menuPlacing == placing)
                {
                    obj = _setup.tutorials[a].gameObject;
                }
            }

            GameObject spawnedObj = Instantiate(_setup.SceneTutorial().gameObject,
                FindObjectOfType<Canvas>().transform.position, Quaternion.identity,
                FindObjectOfType<Canvas>().transform);
            spawnedObj.SetActive(true);
            spawnedObj.transform.SetSiblingIndex(FindObjectOfType<Canvas>().transform.childCount - 1);
            PlayerPrefs.SetInt("tutorial " + placing, 1);
        }
    }


}
