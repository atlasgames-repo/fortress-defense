using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AffectZone : MonoBehaviour
{
    [ReadOnly] public bool isActived = false;

    [Header("LIGHTNING")]
    public float lightingActiveTime = 3;
    public float lightingDamage = 10;
    public GameObject lightingFX;
    public float lightingRate = 1;
    public AudioClip lightingSound;

    [Header("FROZEN")]
    public float frozenActiveTime = 3;
    public float frozenAffectTime = 3;
    public float frozenDamage = 10;
    public GameObject frozenFX;
    public AudioClip forzenSound;

    [Header("POISON")]
    public float poisonActiveTime = 3;
    public float poisonAffectTime = 3;
    public float poisonDamage = 10;
    public GameObject poisonFX;
    public float poisonRate = 0.5f;
    public AudioClip poisonSound;

    // Start is called before the first frame update
    List<Enemy> listEnemyInZone;
    AffectZoneType zoneType;
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        listEnemyInZone = new List<Enemy>();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        isActived = false;
    }

    public void Active(AffectZoneType _type)
    {
        if (!isActived)
        {
            zoneType = _type;
            StartCoroutine(ActiveCo());
            switch (zoneType)
            {
                case AffectZoneType.Lighting:
                    StartCoroutine(StopActiveCo());
                    break;
                case AffectZoneType.Poison:
                    StartCoroutine(StopActiveCo());
                    break;
            }
           
        }
    }

    IEnumerator ActiveCo()
    {
        yield return new WaitForSeconds(Time.fixedDeltaTime);
        yield return null;
        isActived = true;
        if (anim)
            anim.SetBool("isActivating", true);
        while (true)
        {
            if (listEnemyInZone.Count > 0)
            {
                List<Enemy> _tempList = new List<Enemy>(listEnemyInZone);
                foreach (var target in _tempList)
                {
                    if (target.gameObject != null)
                    {
                        switch (zoneType)
                        {
                            case AffectZoneType.Lighting:
                                var _weaponFX = new WeaponEffect();
                                _weaponFX.effectType = WEAPON_EFFECT.LIGHTING;
                                target.TakeDamage(lightingDamage, Vector2.zero, target.gameObject.transform.position, gameObject, BODYPART.NONE, _weaponFX);
                                if (lightingFX)
                                    SpawnSystemHelper.GetNextObject(lightingFX, true).transform.position = target.gameObject.transform.position;
                                SoundManager.PlaySfx(lightingSound);
                                yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
                                break;
                            case AffectZoneType.Frozen:
                                target.TakeDamage(frozenDamage, Vector2.zero, target.gameObject.transform.position, gameObject);
                                target.Freeze(frozenAffectTime, gameObject);
                                if (frozenFX)
                                {
                                    var _fx = SpawnSystemHelper.GetNextObject(frozenFX, true);
                                    _fx.GetComponent<AutoDestroy>().Init(frozenAffectTime);
                                    _fx.transform.position = target.gameObject.transform.position;
                                }
                                SoundManager.PlaySfx(forzenSound);
                                yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
                                break;
                            case AffectZoneType.Poison:
                                target.Poison(poisonDamage, poisonActiveTime, gameObject);
                                if (poisonFX)
                                {
                                    var _fx = SpawnSystemHelper.GetNextObject(poisonFX, true);
                                    _fx.GetComponent<AutoDestroy>().Init(poisonAffectTime);
                                    _fx.transform.position = target.gameObject.transform.position;
                                }
                                SoundManager.PlaySfx(poisonSound);
                                break;
                        }
                    }
                }
            }

            switch (zoneType)
            {
                case AffectZoneType.Lighting:
                    yield return new WaitForSeconds(lightingRate);
                    break;
                case AffectZoneType.Frozen:
                    Stop();
                    break;
                case AffectZoneType.Poison:
                    yield return new WaitForSeconds(poisonRate);
                    break;
            }
            yield return null;
        }
    }

    IEnumerator StopActiveCo()
    {
        float delay = 0;
        switch(zoneType)
        {
            case AffectZoneType.Lighting:
                delay = lightingActiveTime;
                break;
            case AffectZoneType.Frozen:
                delay = frozenActiveTime;
                break;
            case AffectZoneType.Poison:
                delay = poisonActiveTime;
                break;
        }
        yield return new WaitForSeconds(delay);

        Stop();
    }

    void Stop()
    {
        AffectZoneManager.Instance.FinishAffect();
        StopAllCoroutines();
        isActived = false;
        if (anim)
            anim.SetBool("isActivating", false);
        gameObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            var enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (!listEnemyInZone.Contains(enemy))
                    listEnemyInZone.Add(enemy);
            }
        }
        //Debug.LogError(collision.gameObject + "list: " + listEnemyInZone.Count);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            var enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
                listEnemyInZone.Remove(enemy);
        }
        //Debug.LogError(collision.gameObject);
    }
}
