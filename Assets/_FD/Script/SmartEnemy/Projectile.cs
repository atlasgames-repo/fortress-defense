using UnityEngine;
using System.Collections;

public abstract class Projectile : MonoBehaviour {
    public float Speed = 3;

	public LayerMask LayerCollision;

	public GameObject Owner{ get; private set; }
	public Vector2 Direction{ get; private set; }
	public Vector2 InitialVelocity{ get; private set; }
	public bool CanGoBackOwner{ get; private set; }
	public float NewDamage{ get; set; }

	[HideInInspector]
	public bool Explosion;

    //protected float Damage;
    protected Vector2 force;
    protected WeaponEffect weaponEffect;

    // Use this for initialization
    public void Initialize (GameObject owner, Vector2 direction, Vector2 initialVelocity,bool isExplosion = false, bool canGoBackToOwner = false, float _newDamage = 0, WeaponEffect _weaponEffect = null) {
		transform.right = direction;	//turn the X asix to the direction
		Owner = owner;
		Direction = direction;
		InitialVelocity = initialVelocity;
		CanGoBackOwner = canGoBackToOwner && isExplosion;
		NewDamage = _newDamage;
        weaponEffect = _weaponEffect;

        Explosion = isExplosion;

		OnInitialized ();
	}

    public void Initialize(GameObject owner, Vector2 direction, Vector2 initialVelocity, bool isExplosion = false, float Mindamage = 0, float MaxDamage = 0, float critPercent = 0, Vector2 _force = default(Vector2), WeaponEffect _weaponEffect = null)
    {
        //Debug.LogError(direction + "..." + initialVelocity);
        Owner = owner;
        Direction = direction;
        InitialVelocity = initialVelocity;
        force = _force;
        NewDamage = Random.Range(Mindamage, MaxDamage);
        Explosion = isExplosion;
        weaponEffect = _weaponEffect;

        OnInitialized();
    }

    public virtual void OnInitialized(){
	}
    
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.LogError(other.gameObject);

        if ((LayerCollision.value & (1 << other.gameObject.layer)) == 0)
        {
            OnNotCollideWith(other);
            return;
        }

        var isOwner = Owner == other.gameObject;
        if (isOwner)
        {
            OnCollideOwner();
            return;
        }

        

        var takeDamage = (ICanTakeDamage)other.gameObject.GetComponent(typeof(ICanTakeDamage));
        if (takeDamage != null)
        {

            if (other.gameObject.GetComponent(typeof(Projectile)) != null)
            {
                var otherProjectile = (Projectile)other.gameObject.GetComponent(typeof(Projectile));
                if (Owner == otherProjectile.Owner)
                    return;

                OnCollideOther(other);
            }
            else
                OnCollideTakeDamage(other, takeDamage);
            return;
        }
        else
        {
            var takeBodyDamage = (ICanTakeDamageBodyPart)other.gameObject.GetComponent(typeof(ICanTakeDamageBodyPart));
            if (takeBodyDamage != null)
            {
                OnCollideTakeDamageBodyPart(other, takeBodyDamage);
            }
            else
                OnCollideOther(other);
        }
    }

	protected virtual void OnNotCollideWith(Collider2D other){
	}

	protected virtual void OnCollideOwner (){}

	protected virtual void OnCollideTakeDamage(Collider2D other, ICanTakeDamage takedamage){}

    protected virtual void OnCollideTakeDamageBodyPart(Collider2D other, ICanTakeDamageBodyPart takedamage) { }

    protected virtual void OnCollideOther(Collider2D other){}
}
