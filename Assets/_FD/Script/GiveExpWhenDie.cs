using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveExpWhenDie : MonoBehaviour
{
    public int expMin = 5;
    public int expMax = 7;
    [Space(3)] [Header("NightMode")] public int customNightMultiplier = 2;
    public bool useCustomNightMultiplierOnly = false;

    private bool _isEndless = false;
    public void GiveExp()
    {
        int random = Random.Range(expMin, expMax); 
        //SoundManager.PlaySfx(SoundManager.Instance.coinCollect);
        //User.Coin = random;
        GameManager.Instance.UpdateXp(random,transform);
        FloatingTextManager.Instance.ShowText((int)random + "", Vector2.up * 1, Color.yellow, transform.position);
    }

   
}

