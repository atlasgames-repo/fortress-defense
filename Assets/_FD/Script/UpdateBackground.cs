using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateBackground : MonoBehaviour
{
    /* private void OnEnable()
    {
        if (GameLevelSetup.Instance && GameLevelSetup.Instance.GetBackground() != null)
            GetComponent<SpriteRenderer>().sprite = GameLevelSetup.Instance.GetBackground();
    } */

    public background background;
    public detail detail;

    void Start()
    {
        changeBackground();
    }

    void changeBackground()
    {/*
        if(1 <= GlobalValue.levelPlaying && GlobalValue.levelPlaying <= 10)
        {
            GetComponent<SpriteRenderer>().sprite = background.b1;
            Instantiate(detail.worldDetail1, detail.worldDetail1.transform.position, detail.worldDetail1.transform.rotation);
            Instantiate(detail.particle1, detail.particle1.transform.position, detail.particle1.transform.rotation);
        }
        else if(11 <= GlobalValue.levelPlaying && GlobalValue.levelPlaying <= 20)
        {
            GetComponent<SpriteRenderer>().sprite = background.b2;
            Instantiate(detail.worldDetail2, detail.worldDetail2.transform.position, detail.worldDetail2.transform.rotation);
            Instantiate(detail.particle2, detail.particle2.transform.position, detail.particle2.transform.rotation);
        }
        else if(21 <= GlobalValue.levelPlaying && GlobalValue.levelPlaying <= 30)
        {
            GetComponent<SpriteRenderer>().sprite = background.b3;
            Instantiate(detail.worldDetail3, detail.worldDetail3.transform.position, detail.worldDetail3.transform.rotation);
            Instantiate(detail.particle3, detail.particle3.transform.position, detail.particle3.transform.rotation);
        }*/

        //background
        GetComponent<SpriteRenderer>().sprite = background.wBackground[(int)(GlobalValue.levelPlaying/10)];
        //details
        Instantiate(detail.worldDetail[(int)(GlobalValue.levelPlaying/10)],
        detail.worldDetail[(int)(GlobalValue.levelPlaying/10)].transform.position,
        detail.worldDetail[(int)(GlobalValue.levelPlaying/10)].transform.rotation);
        //particles
        Instantiate(detail.particle[(int)(GlobalValue.levelPlaying/10)],
        detail.particle[(int)(GlobalValue.levelPlaying/10)].transform.position,
        detail.particle[(int)(GlobalValue.levelPlaying/10)].transform.rotation);
    }
}
