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
    
    void Update()
    {
        timer += Time.deltaTime;

        if(timer >= timeLimit)
        {
            timer = 0.0f;
            spawnAirDrop();

            Debug.Log("grth");
        }

        xPosition = Random.Range(-9f, 4f);

        Destroy(clone, destroyTime);
        destroyAirDrop1();
    }

    void spawnAirDrop()
    {
        clone = Instantiate(xpBox, new Vector3(xPosition, 5f, 0), Quaternion.identity);
    }

    void destroyAirDrop1()
    {
        getclickDrop = clone.GetComponent<addXP>().clickDrop;

        if(getclickDrop)
        {
            Destroy(clone);

            SoundManager.PlaySfx(SoundManager.Instance.soundUpgrade);
        }
    }
}
