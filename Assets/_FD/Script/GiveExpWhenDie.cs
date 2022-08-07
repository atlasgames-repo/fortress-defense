using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveExpWhenDie : MonoBehaviour
{
    public int expMin = 5;
    public int expMax = 7;

    public void GiveExp()
    {
        //SoundManager.PlaySfx(SoundManager.Instance.coinCollect);
        int random = Random.Range(expMin, expMax);
        //GlobalValue.SavedCoins += random;
        GameManager.Instance.AddExp(random, transform);

        //FloatingTextManager.Instance.ShowText((int)random + "", Vector2.up * 1, Color.yellow, transform.position);
    }
}
