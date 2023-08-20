using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AffectZone : MonoBehaviour
{
    [ReadOnly] public bool isActived = false;

    [Header("LIGHTNING")] public float lightingActiveTime = 3;
    public float lightingDamage = 10;
    public GameObject lightingFX;
    public float lightingRate = 1;
    public AudioClip lightingSound;
    [Header("FIRE")] public float fireActiveTime = 3;
    public float fireDamage = 10;
    public GameObject fireFX;
    public float fireRate = 1;
    public AudioClip fireSound;

    [Header("FROZEN")] public float frozenActiveTime = 3;
    public float frozenAffectTime = 3;
    public float frozenDamage = 10;
    public GameObject frozenFX;
    public AudioClip forzenSound;
    
    [Header("DARK")]
    public float darkActiveTime = 3;
    public float darkAffectTime = 3;
    public float darkDamage = 10;
    public GameObject darkFX;
    public AudioClip darkSound;
    
    [Header("POISON")] public float poisonActiveTime = 3;
    public float poisonAffectTime = 3;
    public float poisonDamage = 10;
    public GameObject poisonFX;
    public float poisonRate = 0.5f;
    public AudioClip poisonSound;

    [Header("MAGNET")] public float magnetRate = 3f;
    public float magnetActiveTime = 3;
    public GameObject magnetIcon;
    public float magnetScaleTime = 0.5f;
    public float elapsedTime = 0;
    public float magnetDamage = 3;
    private Vector3 _initialScale = new Vector3(0, 0, 0);
    private Vector3 _targetScale = new Vector3(1, 1, 1);
    public float magnetAttractionSpeed = 5f;
     GameObject _magnet;
    [HideInInspector]public List<Enemy> tempMagnetList;
    public float minMagnetDistance = 0.2f;

    [Header("AERO")]
    public float aeroRate = 3f;
    public float aeroActiveTime = 3;
    public GameObject aeroIcon;
    public float aeroScaleTime = 0.5f;
    public float elapsedAeroTime = 0;
    public float aeroDamage = 3;
    private Vector3 _initialAeroScale = new Vector3(0, 0, 0);
    private Vector3 _targetAeroScale = new Vector3(1, 1, 1);
    public float aeroAttractionSpeed = 5f;
     GameObject _aero;
    [HideInInspector]public List<Enemy> tempAeroList;
    public float minAeroDistance = 0.2f;
    // Start is called before the first frame update
    public List<Enemy> listEnemyInZone;
    AffectZoneType zoneType;
    public AffectZoneType getAffectZoneType
    {
        get { return zoneType; }
    }


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
                case AffectZoneType.Fire:
                    StartCoroutine(StopActiveCo());
                    break;
                case AffectZoneType.Poison:
                    StartCoroutine(StopActiveCo());
                    break;
                case AffectZoneType.Magnet:
                    StartCoroutine(StopActiveCo());
                    break;
                case AffectZoneType.Aero:
                    StartCoroutine(StopActiveCo());
                    break;
                case AffectZoneType.Frozen:
                    StartCoroutine(StopActiveCo());
                    break;
                case AffectZoneType.Dark:
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
//            Debug.LogError($"AffectZone Stat: {listEnemyInZone.Count}");
            if (listEnemyInZone.Count > 0)
            {
                List<Enemy> _tempList = new List<Enemy>(listEnemyInZone);
                foreach (var target in _tempList)
                {
                    if (target.gameObject != null)
                    {
                        var _weaponFX = new WeaponEffect();

                        switch (zoneType)
                        {
                            case AffectZoneType.Lighting:
                                _weaponFX.effectType = WEAPON_EFFECT.LIGHTING;
                                target.TakeDamage(lightingDamage, Vector2.zero, target.gameObject.transform.position,
                                    gameObject, BODYPART.NONE, _weaponFX);
                                if (lightingFX)
                                    SpawnSystemHelper.GetNextObject(lightingFX, true).transform.position =
                                        target.gameObject.transform.position;
                                SoundManager.PlaySfx(lightingSound);
                                yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
                                break;
                            case AffectZoneType.Fire:
                                 _weaponFX = new WeaponEffect();
                                _weaponFX.effectType = WEAPON_EFFECT.FIRE;
                                target.TakeDamage(fireDamage, Vector2.zero, target.gameObject.transform.position,
                                    gameObject, BODYPART.NONE, _weaponFX);
                                if (fireFX)
                                    SpawnSystemHelper.GetNextObject(fireFX, true).transform.position =
                                        target.gameObject.transform.position;
                                SoundManager.PlaySfx(fireSound);
                                yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
                                break;

                            case AffectZoneType.Frozen:
                                target.TakeDamage(frozenDamage, Vector2.zero, target.gameObject.transform.position,
                                    gameObject);
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
                            case AffectZoneType.Dark:
                                target.TakeDamage(darkDamage, Vector2.zero, target.gameObject.transform.position,
                                    gameObject);
                                target.Freeze(darkAffectTime, gameObject);
                                if (darkFX)
                                {
                                    var _fx = SpawnSystemHelper.GetNextObject(darkFX, true);
                                    _fx.GetComponent<AutoDestroy>().Init(darkAffectTime);
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
                            case AffectZoneType.Magnet:

                                if (!_magnet)
                                {
                                    _magnet = Instantiate(magnetIcon, transform.position, Quaternion.identity, transform);
                                    _initialScale = _magnet.transform.localScale;
                                    if (elapsedTime < magnetScaleTime)
                                    {
                                        elapsedTime += Time.deltaTime;
                                        float t = elapsedTime / magnetScaleTime;
                                        _magnet.transform.localScale = Vector3.Lerp(_initialScale, _targetScale, t);
                                    }
                                }
                                if (!target.GetComponent<SmartEnemyGrounded>().magnet)
                                {
                                    tempMagnetList.Add(target);
                                }
                                target.GetComponent<SmartEnemyGrounded>().magnet = true;
                                target.GetComponent<SmartEnemyGrounded>().magnetPos = transform.position;
                                target.GetComponent<SmartEnemyGrounded>().minMagnetDistance = minMagnetDistance;
                                target.GetComponent<SmartEnemyGrounded>().magnetAttractionSpeed = magnetAttractionSpeed;
                                target.TakeDamage(magnetDamage, Vector2.zero, target.gameObject.transform.position,
                                    gameObject, BODYPART.NONE,null);
                                // code for magnet
                                break;
                            case AffectZoneType.Aero:

                                if (!_aero)
                                {
                                    _aero = Instantiate(aeroIcon, transform.position, Quaternion.identity, transform);
                                    _initialAeroScale = _magnet.transform.localScale;
                                    if (elapsedAeroTime < aeroScaleTime)
                                    {
                                        elapsedAeroTime += Time.deltaTime;
                                        float t = elapsedTime / aeroScaleTime;
                                        _aero.transform.localScale = Vector3.Lerp(_initialAeroScale, _targetAeroScale, t);
                                    }
                                }
                                if (!target.GetComponent<SmartEnemyGrounded>().magnet)
                                {
                                    tempAeroList.Add(target);
                                }
                                target.GetComponent<SmartEnemyGrounded>().magnet = true;
                                target.GetComponent<SmartEnemyGrounded>().magnetPos = transform.position;
                                target.GetComponent<SmartEnemyGrounded>().minMagnetDistance = minAeroDistance;
                                target.GetComponent<SmartEnemyGrounded>().magnetAttractionSpeed = aeroAttractionSpeed;
                                target.TakeDamage(aeroDamage, Vector2.zero, target.gameObject.transform.position,
                                    gameObject, BODYPART.NONE,null);
                                // code for magnet
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
                case AffectZoneType.Fire:
                    yield return new WaitForSeconds(fireRate);
                    break;
                case AffectZoneType.Frozen:
                    Stop();
                    break;
                case AffectZoneType.Dark:
                    Stop();
                    break;
                case AffectZoneType.Poison:
                    yield return new WaitForSeconds(poisonRate);
                    break;
                case AffectZoneType.Magnet:
                    yield return new WaitForSeconds(magnetRate);
                    break;
                case AffectZoneType.Aero:
                    yield return new WaitForSeconds(aeroRate);
                    break;
            }

            yield return null;
        }
    }

    IEnumerator StopActiveCo()
    {
        float delay = 0;
        switch (zoneType)
        {
            case AffectZoneType.Lighting:
                delay = lightingActiveTime;
                break;
            case AffectZoneType.Fire:
                delay = fireActiveTime;
                break;
            case AffectZoneType.Frozen:
                delay = frozenActiveTime;
                break;
            case AffectZoneType.Dark:
                delay = darkActiveTime;
                break;
            case AffectZoneType.Poison:
                delay = poisonActiveTime;
                break;
            case AffectZoneType.Magnet:
                delay = magnetActiveTime;
                // stop magnet 
                break;
            case AffectZoneType.Aero:
                delay = aeroActiveTime;
                // stop magnet 
                break;
        }

        yield return new WaitForSeconds(delay);
        if (zoneType == AffectZoneType.Magnet)
        {
            StopMagnet();
        }else if (zoneType == AffectZoneType.Aero)
        {
            StopAero();
        }
        
        Stop();
    }

    void StopMagnet()
    {
        for (int i = 0; i < tempMagnetList.Count; i++)
        {
            tempMagnetList[i].GetComponent<SmartEnemyGrounded>().magnet = false;
        }
        Destroy(_magnet);
        tempMagnetList.Clear();   
    }
    void StopAero()
    {
        for (int i = 0; i < tempAeroList.Count; i++)
        {
            tempAeroList[i].GetComponent<SmartEnemyGrounded>().magnet = false;
        }
        Destroy(_aero);
        tempAeroList.Clear();   
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
                {
                    listEnemyInZone.Add(enemy);
                 //   Debug.LogError("Add: " + collision.gameObject.name + "list: " + listEnemyInZone.Count + " Zone: " + this.gameObject.name);
                }
            }
        }
        // Debug.LogError(collision.gameObject + "list: " + listEnemyInZone.Count);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            var enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                listEnemyInZone.Remove(enemy);
//                Debug.LogError("Remove: " + collision.gameObject.name + "list: " + listEnemyInZone.Count + " Zone: " + this.gameObject.name);
            }
        }
        // Debug.LogError(collision.gameObject);
    }
}