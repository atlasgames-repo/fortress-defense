using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrophyManager : MonoBehaviour
{
    public RectTransform parent;
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
        // Get all child objects
        GameObject[] children = new GameObject[parent.transform.childCount];
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            children[i] = parent.transform.GetChild(i).gameObject;
            yield return null;
        }

        // Delete child objects
        foreach (GameObject child in children)
        {
            Destroy(child);
            yield return null;
        }

        foreach (string key in Trophy.Keys)
        {
            Add(key);
            yield return null;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    async void Add(string key)
    {
        GameObject obj = Instantiate(rootObject, parent, false);
        Trophy.TryGetTrophy(key, out _Trophy trophy);
        if (trophy != null)
        {
            obj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = trophy.name;
            obj.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = trophy.details;
            Image image = obj.transform.GetChild(1).GetComponent<Image>();

            image.sprite = await APIManager.instance.Get_rofile_picture(trophy.imageURL);
            if ((int)trophy.status > (int)TrophyStatus.ACHIEVED)
            {
                image.color = Enable;
            }
            else
            {
                image.color = Disable;
            }

        }
    }
}
