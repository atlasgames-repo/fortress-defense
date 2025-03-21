using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryMenuEvents : MonoBehaviour
{
    public delegate void Next();
    public enum Operation
    {
        ADD = 1,
        SUBTRACT = -1,
    }
    public float Speed,ConvertDelay,ItemDelay;
    [ReadOnly] public int amount = 1;
    public TMPro.TextMeshProUGUI CoinTxt,ExpTxt,TwoerHealthTxt,StarTxt;
    [ReadOnly] public Color DefaultColor;
    public Color ScaleColor;
    [ReadOnly] public float DefaultScale;
    [Range(0.01f,0.1f)] public float Scale;
    [ReadOnly] public int Coin,Exp,TwoerHealth,Star;

    private bool StopSound = false;
    
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        DefaultScale = CoinTxt.fontSize;
        DefaultColor = CoinTxt.color;
        Coin = 0;
        Exp = GameManager.Instance.currentExp;
        var theFortress = FindObjectsOfType<TheFortrest>();
        foreach (var fortrest in theFortress)
        {
            TwoerHealth = (int)fortrest.currentHealth;
            Star = TwoerStar(TwoerHealth,fortrest.maxHealth);
        }

        CoinTxt.text = "0";
        ExpTxt.text = Exp.ToString();
        TwoerHealthTxt.text = TwoerHealth.ToString();
        StarTxt.text = Star.ToString();
        User.Coin = Exp + TwoerHealth + Star;
        yield return new WaitForSeconds(ItemDelay*2);
        StartCoroutine(Convert(ExpTxt,Exp,()=>{
            StartCoroutine(Convert(TwoerHealthTxt,TwoerHealth,()=>{
                StartCoroutine(Convert(StarTxt,Star));
            }));
        }));
        
    }
    IEnumerator Convert(TMPro.TextMeshProUGUI _ref,int from, Next next = null){
        
        int loop = from;
        StartCoroutine(PlaySound(loop));
        while(loop > 0)
        {
            float delay = ConvertDelay * ( Speed * Mathf.Exp(1) * (1 / (float)loop) );
            yield return new WaitForSeconds(delay);
            CoinTxt.color = ScaleColor;
            //CoinTxt.fontSize += Scale;
            bool res1 = Converter(_ref,amount,Operation.SUBTRACT);
            if (!res1)
                break;
            from -= amount;
            bool res2 = Converter(CoinTxt,amount,Operation.ADD);
            if (!res2)
                break;
            Coin += amount;
            
            loop -= amount;
        }
        if(next == null)
        {
            CoinTxt.color = DefaultColor;
            CoinTxt.fontSize = DefaultScale;
        }
        StopSound = true;
        _ref.transform.parent.gameObject.SetActive(false);
        yield return new WaitForSeconds(ItemDelay);
        next?.Invoke();
    }
    IEnumerator PlaySound(int time){
        float SoundLength = SoundManager.Instance.coinCollect.length;
        while (time > 0)
        {
            if (StopSound)
            {
                StopSound = false;
                break;
            }
            SoundManager.PlaySfx(SoundManager.Instance.coinCollect);
            float delay = 10 * Mathf.Exp(1) * (SoundLength / (time + 20));
            yield return new WaitForSeconds(delay);
            time --;
        }
    }

    bool Converter(TMPro.TextMeshProUGUI txt,int value, Operation operation){
        
        bool result = int.TryParse(txt.text, out int current);
        if (!result)
            return false;
        current += value * (int)operation;
        txt.text = current.ToString();
        return true;     
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    int TwoerStar(int TwoerHealth,float maxHealth){
        float TwoerState = ((float)TwoerHealth / maxHealth);
        if (TwoerState > 0.8f)
            return 3;
        if (TwoerState > 0.5f)
            return 2;
        if (TwoerState > 0f)
            return 1;
        return 1;
    }
}
