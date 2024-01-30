using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveExpWhenDie : MonoBehaviour
{
    public int expMin = 5;
    public int expMax = 7;
    private bool _isEndless = false;
    public void GiveExp()
    {
        int random = Random.Range(expMin, expMax);
        //SoundManager.PlaySfx(SoundManager.Instance.coinCollect);
        if (!_isEndless)
        {
            GameManager.Instance.AddExp(random, transform);
        }
        else
        {
         GameManager.Instance.AddRxp(random,transform);   
         GameManager.Instance.AddTotalRxp(random);
        }
        //User.Coin = random;
        

        //FloatingTextManager.Instance.ShowText((int)random + "", Vector2.up * 1, Color.yellow, transform.position);
    }

    void Start()
    {
        if (FindObjectOfType<EndlessWaveGenerator>())
        {
            _isEndless = true;
        }
    }
}
