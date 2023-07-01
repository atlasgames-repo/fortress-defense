using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TrophyManager : MonoBehaviour
{
    public Transform parent;
    public GameObject rootObject;

    public Color Enable, Disable;

    // Start is called before the first frame update
    void Start()
    {

    }
    public void ReloadPage()
    {
        StartCoroutine(StartEnum());
    }
    IEnumerator StartEnum()
    {
        Transform[] children = parent.GetComponentsInChildren<Transform>();
        for (int i = 0; i < children.Length; i++)
        {
            try
            {
                Destroy(children[i].gameObject);
            }
            catch (System.Exception)
            {
                APIManager.instance.RunStatus("Operation Failed", Color.red);
            }

        }
        // foreach (Transform child in children)
        // {
        //     Destroy(child.gameObject);
        //     yield return null;
        // }
        foreach (var key in Trophy.Trophies.Keys)
        {
            Add(key);
            yield return null;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    void Add(string key)
    {
        GameObject obj = Instantiate(rootObject, parent, false);
        Trophy.Trophies.TryGetValue(key, out _Trophy trophy);
        if (trophy != null)
        {
            obj.transform.GetChild(1).GetComponent<Image>().sprite = Trophy.self.GetSprite(key);
            if ((int)trophy.status > (int)TrophyStatus.ACHIEVED)
            {
                obj.transform.GetChild(1).GetComponent<Image>().color = Enable;
            }
            else
            {
                obj.transform.GetChild(1).GetComponent<Image>().color = Disable;
            }
            obj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = trophy.name;
            obj.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = trophy.details;
        }
    }
}
