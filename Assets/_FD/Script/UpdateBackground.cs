using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateBackground : MonoBehaviour
{
    public background background;
    public volume volume;
    //public detail detail;

    void Start()
    {
        changeBackground();
    }

    void changeBackground()
    {
        if(GlobalValue.levelPlaying < 150)
        {
            Instantiate(background.wBackground[(int)((GlobalValue.levelPlaying - 0.1)/10)], new Vector2(0, 0), Quaternion.identity);
            Instantiate(volume.volumes[(int)((GlobalValue.levelPlaying - 0.1)/10)], new Vector2(0, 0), Quaternion.identity);
            /*Instantiate(detail.particle[(int)((GlobalValue.levelPlaying - 0.1)/10)],
            detail.particle[(int)((GlobalValue.levelPlaying - 0.1)/10)].transform.position,
            detail.particle[(int)((GlobalValue.levelPlaying - 0.1)/10)].transform.rotation);*/
        }
        else if(GlobalValue.levelPlaying > 1000)
        {
            Instantiate(background.endlessBackground[(int)((GlobalValue.levelPlaying) - 1001)], new Vector2(0, 0), Quaternion.identity);
            Instantiate(volume.endlessVolumes[(int)((GlobalValue.levelPlaying) - 1001)], new Vector2(0, 0), Quaternion.identity);

        }
    }
}
