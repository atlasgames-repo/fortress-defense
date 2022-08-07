using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[AddComponentMenu("ADDP/Enemy AI/Throw Attack")]
public class EnemyThrowAttack : MonoBehaviour {
	[Header("Grenade")]
	public float angleThrow = 60;		//the angle to throw the bomb
	public float throwForceMin = 290;		//how strong?
    public float throwForceMax = 320;
    public float addTorque = 100;		
	public float throwRate = 0.5f;
    public float damage = 30;
	public Transform throwPosition;		//throw the bomb at this position
	public GameObject _Grenade;		//the bomb prefab object
	public AudioClip soundAttack;
	float lastShoot = 0;

	public LayerMask targetPlayer;
    public bool onlyAttackTheFortrest = true;
	public Transform checkPoint;
	public float radiusDetectPlayer = 5;
	public bool isAttacking { get; set; }

	public bool AllowAction(){
		return Time.time - lastShoot > throwRate;
	}

//	public void Action(){
//		if (_Grenade == null)
//			return;
//
//		lastShoot = Time.time;
//		SoundManager.PlaySfx (soundAttack);
//	}

	public void Throw(bool isFacingRight){
		Vector3 throwPos = throwPosition.position;
		GameObject obj = Instantiate (_Grenade, throwPos, Quaternion.identity) as GameObject;
        obj.GetComponent<Projectile>().NewDamage = (float) damage;

        float angle; 
		angle = isFacingRight ? angleThrow : 135;

		obj.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle));

		obj.GetComponent<Rigidbody2D>().AddRelativeForce(obj.transform.right * Random.Range(throwForceMin, throwForceMax));
		obj.GetComponent<Rigidbody2D> ().AddTorque (obj.transform.right.x * addTorque);

	}



    // Update is called once per frame
    public bool CheckPlayer()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(checkPoint.position, radiusDetectPlayer, Vector2.zero, 0, targetPlayer);
        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                if (onlyAttackTheFortrest)
                {
                    if (hit.collider.gameObject.GetComponent<TheFortrest>())
                        return true;
                }
                else
                    return true;

            }
        }

        return false;
    }

	public void Action(){
		if (_Grenade == null)
			return;
		lastShoot = Time.time;

	}

	void OnDrawGizmosSelected(){
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere (checkPoint.position, radiusDetectPlayer);
	}
}
