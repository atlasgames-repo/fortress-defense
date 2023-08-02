using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour {
    public bool onlyDisactive = false;
	public float destroyAfterTime = 3f;

    private void OnEnable()
    {
        StartCoroutine(DisableCo());
    }

    public void Init(float delay)
    {
        destroyAfterTime = delay;
    }

    IEnumerator DisableCo()
    {
        yield return null;

        yield return new WaitForSeconds(destroyAfterTime);
        if (onlyDisactive)
            gameObject.SetActive(false);
        else Destroy(gameObject);
    }
}
