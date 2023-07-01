using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchivementManager : MonoBehaviour
{
    public static AchivementManager self;
    public GameObject DialogBox;
    public float Destroy_delay, Start_delay = 5;
    void Awake()
    {
        self = this;
    }

    // Start is called before the first frame update
    public void Start()
    {
        StartCoroutine(StartEnum());
    }

    // Update is called once per frame
    IEnumerator StartEnum()
    {
        yield return new WaitForSeconds(Start_delay);
        foreach (KeyValuePair<string, _Trophy> entety in Trophy.Trophies)
        {
            yield return new WaitForSeconds(0.1f);
            if (entety.Value.status == TrophyStatus.ACHIEVED)
            {
                // show the achivement dialog and save the trophy is is_served
                RunStatus(entety.Value);
                Trophy.STATUS_UP = entety.Value._id;
            }
            if (entety.Value.status == TrophyStatus.RECIVED)
            {
                // Give the player what its received from the achievement
                User.Gem = entety.Value.price;
                Trophy.STATUS_UP = entety.Value._id;
            }
        }
    }
    void RunStatus(_Trophy trophy)
    {
        Transform root = GameObject.FindGameObjectWithTag("Achivement").transform;
        GameObject obj = Instantiate(DialogBox, root, false);
        obj.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = trophy.details;
        obj.transform.GetChild(2).GetComponent<Image>().sprite = Trophy.self.GetSprite(trophy._id);
        obj.GetComponent<Animator>().SetTrigger("In");
        Destroy(obj, Destroy_delay);
    }

}

