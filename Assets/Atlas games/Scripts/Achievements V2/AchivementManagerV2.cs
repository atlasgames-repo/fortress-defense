using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchivementManagerV2 : MonoBehaviour
{
    public RectTransform parent;
    public GameObject rootObject;
    public int NameIndex, DescriptionIndex, SliderIndex, ClaimIndex, DoneIndex;

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

        foreach (string key in BasePlayerPrefs<AchievementModel>.Keys)
        {
            Add(key);
            yield return null;
        }
    }
    void Add(string key)
    {
        GameObject obj = Instantiate(rootObject, parent, false);
        BasePlayerPrefs<AchievementModel>.TryGetValue(key, out AchievementModel trophy);
        if (trophy != null)
        {
            obj.transform.GetChild(NameIndex).GetComponent<TextMeshProUGUI>().text = trophy.name;
            obj.transform.GetChild(DescriptionIndex).GetComponent<TextMeshProUGUI>().text = trophy.description;
            AchievementTasksV2.self.TryGetEvent(trophy._id, out AchievementEventsV2 Events);
            if (Events != null)
                obj.transform.GetChild(SliderIndex).GetComponent<Slider>().value = Events.Proccess;

            if ((int)trophy.status >= (int)TrophyStatus.ACHIEVED)
            {
                SetUpButtons(obj,ClaimIndex);
            }
            else
            {
                SetUpButtons(obj,SliderIndex);
            }
            if ((int)trophy.status == (int)TrophyStatus.PAYED)
            {
                SetUpButtons(obj,DoneIndex);
            }

        }
    }
    void SetUpButtons(GameObject obj, int state){
        obj.transform.GetChild(ClaimIndex).gameObject.SetActive(state == ClaimIndex);
        obj.transform.GetChild(SliderIndex).gameObject.SetActive(state == SliderIndex);
        obj.transform.GetChild(DoneIndex).gameObject.SetActive(state == DoneIndex);
    }

}
