using UnityEngine;

public class thunderStorm : MonoBehaviour
{
    public GameObject rianParticle;
    public GameObject thunderVisual;
    GameObject thunderVisualINT;

    public GameObject dayLight;
    public GameObject nightLight;
    public Animator nightLightAnim;

    public float rndm;

    void Awake()
    {
        dayLight.SetActive(true);
        nightLight.SetActive(false);
    }

    void Start()
    {
        rainS();
    }


    void Update()
    {
        rndm = Random.Range(1f, 500f);

        if((int)rndm == 47 && GlobalValue.levelPlaying > 1001)
        {
            ths();
            //rndm = 0;
        }
    }

    public void ths()
    {
        //thunderVisualINT = Instantiate(thunderVisual, new Vector2(0, 0), Quaternion.identity);
        //Destroy(thunderVisualINT, 5f);
        nightLightAnim.SetTrigger("shine");
        SoundManager.PlaySfx(SoundManager.Instance.thunderSFX);
        SoundManager.PlaySfx(SoundManager.Instance.rain);
    }

    public void rainS()
    {
        if(GlobalValue.levelPlaying > 1001)
        {
            Instantiate(rianParticle, new Vector2(-5.266938f, -0.4270768f), Quaternion.identity);

            dayLight.SetActive(false);
            nightLight.SetActive(true);
        }
    }
}
