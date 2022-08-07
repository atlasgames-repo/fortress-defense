using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum HEALTH_CHARACTER { PLAYER, ENEMY}

[System.Serializable]
public class FortrestLevel
{
    public float maxHealth = 1000;
    public Sprite[] stateFortrestSprites;
}
public class TheFortrest : MonoBehaviour, ICanTakeDamage
{
    //public HEALTH_CHARACTER healthCharacter;
    public FortrestLevel[] fortrestLevels;

    [ReadOnly] public int fortrestLevel = 1;
    public int[] enemyFortrestHealth;

    [HideInInspector]
    public float maxHealth;
    Sprite[] stateFortrestSprites;

    [ReadOnly] public float extraHealth = 0;
    [ReadOnly] public float currentHealth;

    
    public SpriteRenderer fortrestSprite;

    [Header("SHAKNG")]
    public float speed = 30f; //how fast it shakes
    public float amount = 0.5f; //how much it shakes
    public float shakeTime = 0.3f;
    public bool shakeX, shakeY;

    Vector2 startingPos;
    IEnumerator ShakeCoDo;

    void Awake()
    {
        startingPos = transform.position;

        //defaultLevel = healthCharacter == HEALTH_CHARACTER.PLAYER ? GlobalValue.UpgradeStrongWall : defaultFortrest - 1;
        //if (healthCharacter == HEALTH_CHARACTER.PLAYER)
        //{
            maxHealth = fortrestLevels[Mathf.Min(fortrestLevels.Length - 1, GlobalValue.UpgradeStrongWall)].maxHealth;
            stateFortrestSprites = fortrestLevels[GlobalValue.UpgradeStrongWall].stateFortrestSprites;
            fortrestSprite.sprite = stateFortrestSprites[0];
        //}
        //else
        //{
        //    maxHealth = fortrestLevels[GameLevelSetup.Instance.GetEnemyFortrestLevel() - 1].maxHealth;
        //    stateFortrestSprites = fortrestLevels[GameLevelSetup.Instance.GetEnemyFortrestLevel() - 1].stateFortrestSprites;
        //    fortrestSprite.sprite = stateFortrestSprites[0];
        //}
    }

    IEnumerator ShakeCo(float time)
    {
        float counter = 0;
        while (counter < time)
        {
            transform.position = startingPos + new Vector2(Mathf.Sin(Time.time * speed) * amount * (shakeX ? 1 : 0), Mathf.Sin(Time.time * speed) * amount * (shakeY ? 1 : 0));

            yield return null;
            counter += Time.deltaTime;
        }

        transform.position = startingPos;
    }

    // Start is called before the first frame update
    void Start()
    {
        extraHealth = maxHealth * GlobalValue.StrongWallExtra;
        maxHealth += extraHealth;
        currentHealth = maxHealth;
        MenuManager.Instance.UpdateHealthbar(currentHealth, maxHealth/*, healthCharacter*/);
    }

    public void TakeDamage(float damage, Vector2 force, Vector2 hitPoint, GameObject instigator, BODYPART bodyPart = BODYPART.NONE, WeaponEffect weaponEffect = null, WEAPON_EFFECT forceEffect = WEAPON_EFFECT.NONE)
    {
        currentHealth -= damage;
        FloatingTextManager.Instance.ShowText("" + (int)damage, Vector2.up * 2, Color.yellow, transform.position);

        MenuManager.Instance.UpdateHealthbar(currentHealth, maxHealth/*, healthCharacter*/);

        if (currentHealth <= 0)
        {
            //if (healthCharacter == HEALTH_CHARACTER.PLAYER)
                GameManager.Instance.GameOver();
            //else
            //    GameManager.Instance.Victory();
        }
        else
        {
            if (ShakeCoDo != null)
                StopCoroutine(ShakeCoDo);

            ShakeCoDo = ShakeCo(shakeTime);
            StartCoroutine(ShakeCoDo);
        }

        //update fortrest state
        if (currentHealth > 0)
        {
            for (int i = (stateFortrestSprites.Length - 1); i > 0 ; i--)
            {
                if (currentHealth < ((maxHealth / (stateFortrestSprites.Length - 1)) * i))
                {
                    fortrestSprite.sprite = stateFortrestSprites[(stateFortrestSprites.Length - 1) - i];
                }
            }
        }
        else
            fortrestSprite.sprite = stateFortrestSprites[stateFortrestSprites.Length - 1];
    }
}
