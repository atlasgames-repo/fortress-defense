using UnityEngine;

public class thunderStorm : MonoBehaviour
{
    public GameObject rianParticle;
    public GameObject thunderVisual;
    GameObject thunderVisualINT;

    public float rndm;

    void Start()
    {
        
    }


    void Update()
    {
        rndm = Random.Range(1f, 500f);

        if((int)rndm == 47 && GlobalValue.levelPlaying > 1000)
        {
            ths();
            //rndm = 0;
        }
    }

    public void ths()
    {
        thunderVisualINT = Instantiate(thunderVisual, new Vector2(0, 0), Quaternion.identity);
        Destroy(thunderVisualINT, 5f);
        SoundManager.PlaySfx(SoundManager.Instance.thunderSFX);
        SoundManager.PlaySfx(SoundManager.Instance.rain);
        Instantiate(rianParticle, new Vector2(-5.266938f, -0.4270768f), Quaternion.identity);
    }
}
