using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTutorialManager : MonoBehaviour
{
    private GameTutorialSetup _setup;
    void Start()
    {
        StartToturial(false);
    }
    public void StartToturial(bool inMenu){
       _setup =  FindObjectOfType<GameTutorialSetup>();
       if (!inMenu)
       {
           GameObject obj = Instantiate(_setup.SceneTutorial().gameObject,
               FindObjectOfType<Canvas>().transform.position, Quaternion.identity,
               FindObjectOfType<Canvas>().transform);
           obj.SetActive(true);
           obj.transform.SetSiblingIndex(FindObjectOfType<Canvas>().transform.childCount -1);
       }
       else
       {
           GameObject obj = Instantiate(_setup.MenuTutorial().gameObject,
               FindObjectOfType<Canvas>().transform.position, Quaternion.identity,
               FindObjectOfType<Canvas>().transform);
           obj.SetActive(true);
           obj.transform.SetSiblingIndex(FindObjectOfType<Canvas>().transform.childCount -1);
       }
    }

    
}
