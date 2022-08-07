using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedCamera : MonoBehaviour
{
    [ReadOnly] public float fixedWidth;
    public float orthographicSize = 3.8f;
    // Start is called before the first frame update
    void Start()
    {
        if (GameMode.Instance)
        {
            fixedWidth = orthographicSize * (GameMode.Instance.resolution.x / GameMode.Instance.resolution.y);
            Camera.main.orthographicSize = fixedWidth / (Camera.main.aspect);
        }
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (GameMode.Instance)
    //    {
    //        Camera.main.orthographicSize = fixedWidth / (Camera.main.aspect);
    //    }
    //}
}
