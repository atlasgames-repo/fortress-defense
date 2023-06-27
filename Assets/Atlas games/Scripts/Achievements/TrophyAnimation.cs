using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrophyAnimation : MonoBehaviour
{
    [ReadOnly] public Button button;
    [ReadOnly] public Animator animator;
    [ReadOnly] public bool flipped = false;
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        animator = GetComponent<Animator>();
    }

    public void Flipp()
    {
        if (flipped)
            flipped = false;
        else
            flipped = true;
        animator.SetBool("Flipped", flipped);
    }
}
