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
        foreach (Achivement key in AchivementManager.self.achivements.list)
        {
            yield return new WaitForEndOfFrame();
            Add(key);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    async void Add(Achivement key)
    {
        GameObject obj = Instantiate(rootObject, parent, false);
        obj.transform.GetChild(1).GetComponent<Image>().sprite = await key.Get_Sprite();
        if (key.is_achived)
        {
            obj.transform.GetChild(1).GetComponent<Image>().color = Enable;
        }
        else
        {
            obj.transform.GetChild(1).GetComponent<Image>().color = Disable;
        }
        obj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = key.name;
        obj.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = key.description;
    }
}
