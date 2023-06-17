using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchivementManager : MonoBehaviour
{
    public static AchivementManager self;
    public GameObject DialogBox;
    public Achivements achivements;
    public float Destroy_delay;
    void Awake()
    {
        self = this;
    }

    // Start is called before the first frame update
    private async void Start()
    {
        achivements = new Achivements(await APIManager.instance.Get_Achivements());
        StartCoroutine(StartEnum());
    }

    // Update is called once per frame
    IEnumerator StartEnum()
    {
        yield return new WaitForSeconds(5f);
        foreach (Achivement entety in achivements.list)
        {
            yield return new WaitForSeconds(0.1f);
            if (entety.is_achived && entety.is_served == false)
            {
                // show the achivement dialog and save the trophy is is_served
                RunStatus(entety);
                // Trophy.Serve(entety.Value._id);
            }
        }
    }

    private async void RunStatus(Achivement entety)
    {
        Transform root = GameObject.FindGameObjectWithTag("Achivement").transform;
        GameObject obj = Instantiate(DialogBox, root, false);
        obj.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = entety.description;
        obj.transform.GetChild(2).GetComponent<Image>().sprite = await entety.Get_Sprite();
        obj.GetComponent<Animator>().SetTrigger("In");
        Destroy(obj, Destroy_delay);
    }

}

