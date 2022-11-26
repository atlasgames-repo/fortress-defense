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
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
        foreach (var key in Trophy.self.Trophies.Keys)
        {
            yield return new WaitForEndOfFrame();
            Add(key);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    void Add(string key)
    {
        GameObject obj = Instantiate(rootObject, parent, false);
        _Trophy trophy = null;
        Trophy.self.Trophies.TryGetValue(key, out trophy);
        if (trophy != null)
        {
            obj.transform.GetChild(1).GetComponent<Image>().sprite = trophy.image;
            if (trophy.is_achived)
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
