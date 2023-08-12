using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTutorialManager : MonoBehaviour
{
    private GameTutorialSetup _setup;
    void Start()
    {
       _setup =  FindObjectOfType<GameTutorialSetup>();
       GameObject obj = Instantiate(_setup.SceneTutorial().gameObject, FindObjectOfType<Canvas>().transform.position,
           Quaternion.identity, FindObjectOfType<Canvas>().transform);
       obj.SetActive(true);
       obj.transform.SetSiblingIndex(FindObjectOfType<Canvas>().transform.childCount -1);
    }
    
}
