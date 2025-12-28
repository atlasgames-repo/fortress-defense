using System.Collections;
using UnityEngine;

public class airDrop : MonoBehaviour
{
    public GameObject xpBox;
    public GameObject clone;

    public float timeLimit = 10f;
    float timer = 0.0f;
    public float xPosition;
    public float destroyTime = 2f;
    public bool getclickDrop;
    private Coroutine coroutine;

    private void Start() {
        coroutine = StartCoroutine(AirDrop());
    }

    private void OnApplicationQuit() { //ensure that the coroutine doesn't run indefinitely
        if (coroutine != null)
            StopCoroutine(coroutine);
    }
    ~airDrop() { // same as OnApplicationQuit but when the object is destroyed instead of application exit
        if (coroutine != null)
            StopCoroutine(coroutine);
    }
    IEnumerator AirDrop()
    {
        while (true) {
            yield return new WaitForSeconds(timeLimit);
            xPosition = Random.Range(-9f, 4f);
            spawnAirDrop();
            Destroy(clone, destroyTime);
            //destroyAirDrop1();
        }
    }

    void spawnAirDrop()
    {
        clone = Instantiate(xpBox, new Vector3(xPosition, 5f, 0), Quaternion.identity);
    }

    void Update() {
        addXP addxp;
        clone.TryGetComponent<addXP>(out addxp);

        if(addxp && addxp.clickDrop)   {
            Destroy(clone);

            SoundManager.PlaySfx(SoundManager.Instance.soundUpgrade);
        }
    }
}
