using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveCoinWhenDie : MonoBehaviour
{
    public int coinGiveMin = 5;
    public int coinGiveMax = 10;

    public void GiveCoin()
    {
        SoundManager.PlaySfx(SoundManager.Instance.coinCollect);
        int random = Random.Range(coinGiveMin, coinGiveMax);
        GlobalValue.SavedCoins += random;

        FloatingTextManager.Instance.ShowText((int)random + "", Vector2.up * 1, Color.yellow, transform.position);
    }
}
