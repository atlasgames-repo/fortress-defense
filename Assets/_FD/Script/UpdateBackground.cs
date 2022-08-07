using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateBackground : MonoBehaviour
{
    private void OnEnable()
    {
        if (GameLevelSetup.Instance && GameLevelSetup.Instance.GetBackground() != null)
            GetComponent<SpriteRenderer>().sprite = GameLevelSetup.Instance.GetBackground();
    }
}
