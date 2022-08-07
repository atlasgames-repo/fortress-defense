using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : Projectile, IListener, ICanTakeDamage
{
    public Sprite hitImageBlood;
    public SpriteRenderer arrowImage;
    Vector2 oldPos;
     int Damage = 30;
    public GameObject DestroyEffect;
    public int pointToGivePlayer;
    public float timeToLive = 3;
    public AudioClip soundHitEnemy;
    [Range(0, 1)]
    public float soundHitEnemyVolume = 0.5f;
    public AudioClip soundHitNothing;
    [Range(0, 1)]
    public float soundHitNothingVolume = 0.5f;

    public GameObject ExplosionObj;
    float timeToLiveCounter = 0;
    public bool parentToHitObject = true;

    bool isHit = false;
    Rigidbody2D rig;
    bool criticalDamage = false;
    WeaponEffect arrowEffect;
    float disableAtYPos = -1;
    WEAPON_EFFECT forceEffect;
    void OnEnable()
    {
        timeToLiveCounter = timeToLive;
        isHit = false;

        if (rig == null)
            rig = GetComponent<Rigidbody2D>();
        rig.isKinematic = false;
    }

    public void Init(int damageMin, int damageMax, Vector2 velocityForce, float gravityScale, bool isCritical, WeaponEffect _arrowEffect, WEAPON_EFFECT _forceEffect = WEAPON_EFFECT.NONE, float _disableAtYPos = - 1)
    {
        arrowEffect = _arrowEffect;
        forceEffect = _forceEffect;
        Damage = Random.Range(damageMin, damageMax);
        criticalDamage = isCritical;
        rig = GetComponent<Rigidbody2D>();
        rig.gravityScale = gravityScale;
        rig.velocity = velocityForce;
        disableAtYPos = _disableAtYPos;
    }

    void Start()
    {
        oldPos = transform.position;

        GameManager.Instance.listeners.Add(this);
    }

    public Vector2 checkTargetDistanceOffset = new Vector2(-0.25f,0);
    public float checkTargetDistance = 1;

    // Update is called once per frame
    void Update()
    {
        if (isHit)
            return;

        if ((Vector2)transform.position != oldPos)
        {
            transform.right = ((Vector2)transform.position - oldPos).normalized;
        }

        //check hit target
        RaycastHit2D hit = Physics2D.Linecast(oldPos, transform.position, LayerCollision);
        if (hit)
        {
            Hit(hit);
            isHit = true;
        }

        oldPos = transform.position;

        if ((timeToLiveCounter -= Time.deltaTime) <= 0)
        {
            DestroyProjectile();
        }

        if(disableAtYPos != -1)
        {
            if (transform.position.y < disableAtYPos)
            {
                SoundManager.PlaySfx(soundHitNothing, soundHitNothingVolume);
                StartCoroutine(DestroyProjectile(3));
                isHit = true;
            }
        }
        //check hit
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine((Vector2)transform.position + checkTargetDistanceOffset, (Vector2)transform.position + checkTargetDistanceOffset + (Vector2)transform.right * checkTargetDistance);
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
 
    }

    void Hit(RaycastHit2D other)
    {
        transform.position = other.point + (Vector2)(transform.position - transform.Find("head").position);
        
        var takeDamage = (ICanTakeDamage)other.collider.gameObject.GetComponent(typeof(ICanTakeDamage));
        if (takeDamage != null)
        {
            OnCollideTakeDamage(other.collider, takeDamage);
            if (criticalDamage)
                FloatingTextManager.Instance.ShowText("CRIT!", Vector2.up, Color.yellow, other.collider.gameObject.transform.position, 30);

        }
        else
            OnCollideOther(other.collider);
        //}
    }

    IEnumerator DestroyProjectile(float delay = 0)
    {
        var rig = GetComponent<Rigidbody2D>();
        rig.velocity = Vector2.zero;
        rig.isKinematic = true;

        yield return new WaitForSeconds(delay);
        if (DestroyEffect != null)
            SpawnSystemHelper.GetNextObject(DestroyEffect, true).transform.position = transform.position;

        if (Explosion)
        {
            var bullet = Instantiate(ExplosionObj, transform.position, Quaternion.identity) as GameObject;
        }

        gameObject.SetActive(false);
    }

    public void TakeDamage(float damage, Vector2 force, Vector2 hitPoint, GameObject instigator, BODYPART bodyPart = BODYPART.NONE)
    {
        SoundManager.PlaySfx(soundHitNothing, soundHitNothingVolume);
        StartCoroutine(DestroyProjectile(1));
    }

    protected override void OnCollideOther(Collider2D other)
    {
        SoundManager.PlaySfx(soundHitNothing, soundHitNothingVolume);
        StartCoroutine(DestroyProjectile(3));
        if (parentToHitObject)
            transform.parent = other.gameObject.transform;

    }

    protected override void OnCollideTakeDamage(Collider2D other, ICanTakeDamage takedamage)
    {
        //Debug.LogError(other.name);
        base.OnCollideTakeDamage(other, takedamage);

        takedamage.TakeDamage(Damage, Vector2.zero, transform.position, Owner,BODYPART.NONE, arrowEffect, forceEffect);
        SoundManager.PlaySfx(soundHitEnemy, soundHitEnemyVolume);
        StartCoroutine(DestroyProjectile(0));

        if (parentToHitObject)
            transform.parent = other.gameObject.transform;

        if (arrowImage && hitImageBlood)
        {
            arrowImage.sprite = hitImageBlood;
        }
    }

    protected override void OnCollideTakeDamageBodyPart(Collider2D other, ICanTakeDamageBodyPart takedamage)
    {
        
        base.OnCollideTakeDamageBodyPart(other, takedamage);
        WeaponEffect weaponEffect = new WeaponEffect();
        takedamage.TakeDamage(Damage, force, transform.position, Owner);
        StartCoroutine(DestroyProjectile(0));

        if (parentToHitObject)
            transform.parent = other.gameObject.transform;

        if (arrowImage && hitImageBlood)
        {
            arrowImage.sprite = hitImageBlood;
        }
    }

    bool isStop = false;
    #region IListener implementation

    public void IPlay()
    {
        //		throw new System.NotImplementedException ();
    }

    public void ISuccess()
    {
        //		throw new System.NotImplementedException ();
    }

    public void IPause()
    {
        //		throw new System.NotImplementedException ();
    }

    public void IUnPause()
    {
        //		throw new System.NotImplementedException ();
    }

    public void IGameOver()
    {
        //		throw new System.NotImplementedException ();
    }

    public void IOnRespawn()
    {
        //		throw new System.NotImplementedException ();
    }

    public void IOnStopMovingOn()
    {
        //		Debug.Log ("IOnStopMovingOn");
        //		anim.enabled = false;
        isStop = true;
        //		GetComponent<Rigidbody2D> ().isKinematic = true;
    }

    public void IOnStopMovingOff()
    {
        //		anim.enabled = true;
        isStop = false;
        //		GetComponent<Rigidbody2D> ().isKinematic = false;
    }

    public void TakeDamage(float damage, Vector2 force, Vector2 hitPoint, GameObject instigator, BODYPART bodyPart = BODYPART.NONE, WeaponEffect weaponEffect = null, WEAPON_EFFECT forceEffect = WEAPON_EFFECT.NONE)
    {
        StartCoroutine(DestroyProjectile(0));
    }

    #endregion
}
