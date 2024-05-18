using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndergroundHole : MonoBehaviour
{
    public Transform spriteMaskTransform;
    public GameObject holeBehind;
    public GameObject holeFront;
    private Animator _anim;
    public float fadeTime = 3.5f;
    private AudioSource _source;
    private float _climbingTime;
    public void Init(float climbingTime, float yScale,float pileScale)
    {
        _source.Play();
        _anim.SetTrigger("Open");
        transform.localScale *= pileScale;
        spriteMaskTransform.localScale = new Vector3(1, yScale, 1);
        _climbingTime = climbingTime;
        StartCoroutine(DisableMask(_climbingTime));
        StartCoroutine(AnimateFade());
    }
    IEnumerator DisableMask(float climbTime)
    {
        yield return new WaitForSeconds(climbTime);
        spriteMaskTransform.GetChild(0).gameObject.SetActive(false);
    }
    IEnumerator AnimateFade()
    {
        yield return new WaitForSeconds(fadeTime);
        _anim.SetTrigger("Fade");
    }

    public void DisableHole()
    {
        Invoke("DeactivateHole", 0.01f);
    }

    void DeactivateHole()
    {
        gameObject.SetActive(false);
    }
}
