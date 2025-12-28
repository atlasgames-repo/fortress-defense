using UnityEngine;

public class Grave : MonoBehaviour
{
    public void PlaySFX() {
        SoundManager.PlaySfx(SoundManager.Instance.graveHit,0.7f);
        ParticleSystem prt;
        transform.GetChild(0).TryGetComponent<ParticleSystem>(out prt);
        if (prt == null)
            return;
        prt.Play();
    }
}
