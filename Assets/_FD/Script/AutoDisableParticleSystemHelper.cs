using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class AutoDisableParticleSystemHelper : MonoBehaviour
{
    public bool OnlyDeactivate = true;

    void OnEnable()
    {
        StartCoroutine("CheckAlive");
    }

    IEnumerator CheckAlive()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (!GetComponent<ParticleSystem>().IsAlive(true))
            {
                if (OnlyDeactivate)
                {
                    this.gameObject.SetActive(false);
                }
                else
                    GameObject.Destroy(this.gameObject);
                break;
            }
        }
    }
}
