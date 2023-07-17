using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShake : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        ScreenShake.instance.StartShake(0.1f, 0.05f);
    }
}
