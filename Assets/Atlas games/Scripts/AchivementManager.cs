using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchivementManager : MonoBehaviour
{
    public static AchivementManager self;
    public GameObject DialogBox;
    public float Destroy_delay;
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
        yield return new WaitForSeconds(5f);
        foreach (KeyValuePair<string, _Trophy> entety in Trophy.self.Trophies)
        {
            yield return new WaitForSeconds(0.1f);
            if (entety.Value.is_achived && entety.Value.is_served == false)
            {
                // show the achivement dialog and save the trophy is is_served
                RunStatus(entety.Value);
                Trophy.Serve(entety.Value._id);
            }
        }
    }
    void RunStatus(_Trophy trophy)
    {
        Transform root = GameObject.FindGameObjectWithTag("Achivement").transform;
        GameObject obj = Instantiate(DialogBox, root, false);
        obj.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = trophy.details;
        obj.transform.GetChild(2).GetComponent<Image>().sprite = trophy.image;
        obj.GetComponent<Animator>().SetTrigger("In");
        Destroy(obj, Destroy_delay);
    }

}

