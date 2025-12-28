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
    [Header("FIRE")] public float fireAffectTime = 3;
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
    [HideInInspector] public List<Enemy> tempMagnetList;
    public float minMagnetDistance = 0.2f;
    public AudioClip magnetSound;

    // repel tuning
    [Header("MAGNET - REPEL TUNING")]
    public float repelForce = 15f;
    public float repelSpeed = 8f;
    public float destroyTimeoutAfterRepel = 4f;
    public float outOfScreenMargin = 0.2f;

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
    [HideInInspector] public List<Enemy> tempAeroList;
    public float minAeroDistance = 0.2f;
    public AudioClip aeroSound;

    public List<Enemy> listEnemyInZone;
    AffectZoneType zoneType;

    [Header("Armagdon")] public GameObject fireBall;
    public float aramgdonFallTime = 1.3f;
    private GameObject[] _spawnedArmagdons;
    public Vector2[] fireBallSpawnOffset;
    public AudioClip impaceSfx;
    public float offset;
    public float fireBallSpeed = 2f;

    [Header("Fortress wall")] public GameObject defenseWall;
    public Transform wallPosition;
    public int wallHealth = 150;
    public AffectZoneType getAffectZoneType
    {
        get { return zoneType; }
    }
    [Range(1, 500)]
    public int min_xp_consum, max_xp_consum;

    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        listEnemyInZone = new List<Enemy>();
        tempMagnetList = new List<Enemy>();
        tempAeroList = new List<Enemy>();
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
                case AffectZoneType.Fire:
                case AffectZoneType.Poison:
                case AffectZoneType.Magnet:
                case AffectZoneType.Aero:
                case AffectZoneType.Frozen:
                case AffectZoneType.Dark:
                    StartCoroutine(StopActiveCo());
                    break;
            }
            int XPConsume = AffectZoneManager.Instance.XPconsume(_type);
            if (GameManager.Instance.currentExp < XPConsume || !AffectZoneManager.Instance.isZoneUsedFirstTime)
            {
                AffectZoneManager.Instance.isZoneUsedFirstTime = true;
                return;
            }
            GameManager.Instance.currentExp -= XPConsume;
            FloatingTextManager.Instance.ShowText("-" + XPConsume + " XP", Vector2.up * 1, Color.red, transform.position, 40);
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
                    if (target == null || target.gameObject == null) continue;

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
                            CameraShake.instance.StartShake(0.05f, 0.05f);
                            SoundManager.PlaySfx(lightingSound);
                            yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
                            break;
                        case AffectZoneType.Fire:
                            target.TakeDamage(fireDamage, Vector2.zero, target.gameObject.transform.position,
                                gameObject);
                            if (fireFX)
                            {
                                var _fx = SpawnSystemHelper.GetNextObject(fireFX, true);
                                _fx.GetComponent<AutoDestroy>().Init(fireAffectTime);
                                _fx.transform.position = target.gameObject.transform.position;
                            }
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
                            _weaponFX = new WeaponEffect();
                            _weaponFX.effectType = WEAPON_EFFECT.DARK;
                            target.TakeDamage(darkDamage, Vector2.zero, target.gameObject.transform.position,
                                gameObject, BODYPART.NONE, _weaponFX);
                            if (darkFX)
                                SpawnSystemHelper.GetNextObject(darkFX, true).transform.position =
                                    target.gameObject.transform.position;
                            SoundManager.PlaySfx(darkSound);
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

                            for (int i = listEnemyInZone.Count - 1; i >= 0; i--)
                            {
                                var enemy = listEnemyInZone[i];
                                if (enemy == null) continue;

                                Vector2 repelDir = (enemy.transform.position - transform.position);
                                if (repelDir.magnitude < 0.1f)
                                {
                                    repelDir = Random.insideUnitCircle.normalized;
                                    Debug.Log($"[Magnet] Enemy {enemy.name} is too close → random direction applied");
                                }
                                else
                                {
                                    repelDir = repelDir.normalized;
                                }

                                Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
                                if (rb != null)
                                {
                                    rb.linearVelocity = repelDir * repelSpeed;
                                    rb.AddForce(repelDir * repelForce * Time.deltaTime, ForceMode2D.Impulse);
                                    Debug.Log($"[Magnet] Enemy {enemy.name} repelled → vel:{rb.linearVelocity}");
                                }
                                else
                                {
                                    enemy.transform.position += (Vector3)(repelDir * repelSpeed * Time.deltaTime);
                                    Debug.Log($"[Magnet] Enemy {enemy.name} moved manually (no Rigidbody2D).");
                                }

                                enemy.TakeDamage(magnetDamage * Time.deltaTime, Vector2.zero,
                                    enemy.transform.position, gameObject, BODYPART.NONE, null);

                                enemy.Die();
                            }

                            if (Time.time % magnetRate < Time.deltaTime)
                            {
                                SoundManager.PlaySfx(magnetSound);
                            }
                            break;



                        case AffectZoneType.Aero:
                            if (!_aero)
                            {
                                _aero = Instantiate(aeroIcon, transform.position, Quaternion.identity, transform);
                                _initialAeroScale = _aero.transform.localScale;
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
                                gameObject, BODYPART.NONE, null);
                            SoundManager.PlaySfx(aeroSound);
                            // code for magnet
                            break;
                        case AffectZoneType.DefenseWall:
                            ActivateDefenseWall();
                            break;
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
                delay = fireAffectTime;
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
                //delay = magnetActiveTime;
                //break;
            case AffectZoneType.Aero:
                delay = aeroActiveTime;
                break;
        }
        yield return new WaitForSeconds(delay);
        if (zoneType == AffectZoneType.Magnet)
        {
            StopMagnet();
        }
        else if (zoneType == AffectZoneType.Aero)
        {
            StopAero();
        }
        Stop();
    }

    void StopMagnet()
    {
        if (_magnet)
            Destroy(_magnet);

        listEnemyInZone.Clear();
        Debug.Log("[Magnet] Magnet stopped → enemy list cleared");
    }

    void StopAero()
    {
        if (tempAeroList != null)
        {
            for (int i = 0; i < tempAeroList.Count; i++)
            {
                if (tempAeroList[i] != null)
                    tempAeroList[i].GetComponent<SmartEnemyGrounded>().magnet = false;
            }
            tempAeroList.Clear();
        }
        if (_aero)
            Destroy(_aero);
    }

    public void Stop()
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
            if (enemy != null && !listEnemyInZone.Contains(enemy))
            {
                listEnemyInZone.Add(enemy);
                Debug.Log($"[Magnet] Enemy entered zone: {enemy.name}");
            }
        }
    }


    public void ActivateArmagdon()
    {
        Vector2 firstOffsetTarget = new Vector2();
        for (int i = 0; i < fireBallSpawnOffset.Length; i++)
        {
            Vector2 newTarget = new Vector2();
            if (i == 0)
            {
                firstOffsetTarget = fireBallSpawnOffset[i];
            }
            else
            {
                newTarget = fireBallSpawnOffset[i] - firstOffsetTarget;
            }
            var spawnedFireBall = SpawnSystemHelper.GetNextObject(fireBall, true);
            spawnedFireBall.transform.position = (Vector2)transform.position + fireBallSpawnOffset[i] + firstOffsetTarget;
            StartCoroutine(ThrowFireBall(spawnedFireBall.gameObject, spawnedFireBall.transform.position, (Vector2)transform.position + newTarget));
        }
    }

    public void ActivateDefenseWall()
    {
        var newWall = SpawnSystemHelper.GetNextObject(defenseWall);
        newWall.transform.position = wallPosition.position;
        newWall.GetComponent<DefenseWall>().Init(wallHealth, GetComponent<AffectZone>());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            var enemy = collision.GetComponent<Enemy>();
            if (enemy != null && listEnemyInZone.Contains(enemy))
            {
                listEnemyInZone.Remove(enemy);
                Debug.Log($"[Magnet] Enemy exited zone: {enemy.name}");
            }
        }
    }


    private IEnumerator ThrowFireBall(GameObject fireBall, Vector3 startPos, Vector3 target)
    {
        fireBall.transform.position = startPos;
        while (Vector3.Distance(fireBall.transform.position, target) > 0.01f)
        {
            Vector3 direction = (target - fireBall.transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle += offset;
            fireBall.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            fireBall.transform.Translate(direction * fireBallSpeed * Time.deltaTime, Space.World);
            yield return null;
        }
        Destroy(fireBall);
        GameObject fireFx = SpawnSystemHelper.GetNextObject(fireFX, true);
        fireFx.GetComponent<AutoDestroy>().Init(fireAffectTime);
        fireFx.transform.position = target;
        fireBall.transform.position = target;
        SoundManager.PlaySfx(impaceSfx);
        Active(AffectZoneType.Fire);
        CameraShake.instance.StartShake(0.1f, 0.1f);
    }

    private IEnumerator DestroyWhenOutOfScreen(GameObject enemy, float timeout)
    {
        float elapsed = 0f;
        while (enemy != null && elapsed < timeout)
        {
            if (Camera.main != null)
            {
                Vector3 viewPos = Camera.main.WorldToViewportPoint(enemy.transform.position);
                if (viewPos.x < -outOfScreenMargin || viewPos.x > 1 + outOfScreenMargin || viewPos.y < -outOfScreenMargin || viewPos.y > 1 + outOfScreenMargin)
                {
                    RemoveFromListsAndDestroy(enemy);
                    yield break;
                }
            }
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (enemy != null)
        {
            RemoveFromListsAndDestroy(enemy);
        }
    }

    private IEnumerator MoveTransformAwayAndDestroy(GameObject enemy, Vector2 dir, float speed, float maxDuration)
    {
        float t = 0f;
        while (enemy != null && t < maxDuration)
        {
            enemy.transform.position += (Vector3)(dir * speed * Time.deltaTime);
            if (Camera.main != null)
            {
                Vector3 viewPos = Camera.main.WorldToViewportPoint(enemy.transform.position);
                if (viewPos.x < -outOfScreenMargin || viewPos.x > 1 + outOfScreenMargin || viewPos.y < -outOfScreenMargin || viewPos.y > 1 + outOfScreenMargin)
                {
                    RemoveFromListsAndDestroy(enemy);
                    yield break;
                }
            }
            t += Time.deltaTime;
            yield return null;
        }

        if (enemy != null)
        {
            RemoveFromListsAndDestroy(enemy);
        }
    }

    private void RemoveFromListsAndDestroy(GameObject enemy)
    {
        if (enemy == null) return;

        var enemyComp = enemy.GetComponent<Enemy>();
        if (enemyComp != null)
        {
            listEnemyInZone.Remove(enemyComp);
            if (tempMagnetList != null && tempMagnetList.Contains(enemyComp))
                tempMagnetList.Remove(enemyComp);
            if (tempAeroList != null && tempAeroList.Contains(enemyComp))
                tempAeroList.Remove(enemyComp);
        }

        Destroy(enemy);
    }
}
