using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("ADDP/Enemy AI/Smart Enemy Ground Control")]
[RequireComponent (typeof (Controller2D))]
public class SmartEnemyGrounded : Enemy, ICanTakeDamage {
    
	public bool isSocking{ get; set; }
	public bool isDead{ get; set; }

	[HideInInspector]
	public Vector3 velocity;
	private Vector2 _direction;
	[HideInInspector]
	public Controller2D controller;

	float velocityXSmoothing = 0;
	Vector2 pushForce;
	private float _directionFace;
    
    [Header("New")]

	bool allowCheckAttack = true;

	EnemyRangeAttack rangeAttack;
	EnemyMeleeAttack meleeAttack;
	EnemyThrowAttack throwAttack;
	EnemyCallMinion callMinion;
    SpawnItemHelper spawnItem;
    
	public override void Start ()
	{
		base.Start ();

		controller = GetComponent<Controller2D> ();
			_direction = isFacingRight() ? Vector2.right : Vector2.left;

        if((_direction == Vector2.right && startBehavior == STARTBEHAVIOR.WALK_LEFT) || (_direction == Vector2.left && startBehavior == STARTBEHAVIOR.WALK_RIGHT))
        {
            Flip();
        }
		isPlaying = true;
		isSocking = false;

		controller.collisions.faceDir = 1;

		rangeAttack = GetComponent<EnemyRangeAttack> ();
		meleeAttack = GetComponent<EnemyMeleeAttack> ();
		throwAttack = GetComponent<EnemyThrowAttack> ();
		callMinion = GetComponent<EnemyCallMinion> ();

        if (rangeAttack && rangeAttack.GunObj)
			rangeAttack.GunObj.SetActive (attackType == ATTACKTYPE.RANGE);
		if (meleeAttack && meleeAttack.MeleeObj)
			meleeAttack.MeleeObj.SetActive (attackType == ATTACKTYPE.MELEE);

		spawnItem = GetComponent<SpawnItemHelper> ();


        //Do Get Upgrade
        if (upgradedCharacterID != null)
        {
            //if (meleeAttack)
            //{
            //    meleeAttack.dealDamage = upgradedCharacterID.UpgradeMeleeDamage;
            //    meleeAttack.criticalPercent = upgradedCharacterID.UpgradeCriticalDamage;
            //}
            if (rangeAttack)
            {
                rangeAttack.damage = upgradedCharacterID.UpgradeRangeDamage;
            }
        }
    }
    
	public override void Update ()
	{
		base.Update ();
		HandleAnimation ();

        if (enemyState != ENEMYSTATE.WALK || GameManager.Instance.State != GameManager.GameState.Playing)
        {
            velocity.x = 0;
            return;
        }

        if (checkTarget.CheckTarget(isFacingRight() ? 1 : -1))
            DetectPlayer(delayChasePlayerWhenDetect);
    }
    
    public virtual void LateUpdate(){
        if (GameManager.Instance.State != GameManager.GameState.Playing)
            return;
        else if (!isPlaying || isSocking || enemyEffect == ENEMYEFFECT.SHOKING)
        {
            velocity = Vector2.zero;
            return;
        }

		float targetVelocityX = _direction.x * moveSpeed;
        if (isSocking  || enemyEffect == ENEMYEFFECT.SHOKING)
        {
            targetVelocityX = 0;
        }

        if (enemyState != ENEMYSTATE.WALK || enemyEffect == ENEMYEFFECT.FREEZE)
            targetVelocityX = 0;

        if (isStopping || isStunning)
            targetVelocityX = 0;

        velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? 0.1f : 0.2f);
        
		velocity.y += -gravity * Time.deltaTime;
        
        if ((_direction.x > 0 && controller.collisions.right) || (_direction.x < 0 && controller.collisions.left))
            velocity.x = 0;
        
		controller.Move (velocity * Time.deltaTime * multipleSpeed, false, isFacingRight ());

		if (controller.collisions.above || controller.collisions.below)
			velocity.y = 0;

        if (isPlaying && isPlayerDetected && allowCheckAttack && enemyEffect != ENEMYEFFECT.FREEZE)
        {
            CheckAttack();
        }
	}

	void Flip(){
		_direction = -_direction;
		transform.rotation = Quaternion.Euler (new Vector3 (transform.rotation.x, isFacingRight () ? 0 : 180, transform.rotation.z));
	}
    
    public override void Stun(float time = 2)
    {
        base.Stun(time);
        StartCoroutine(StunCo(time));
    }

    IEnumerator StunCo(float time)
    {
        AnimSetTrigger("stun");
        isStunning = true;
        yield return new WaitForSeconds(time);
        isStunning = false;
    }

    public override void StunManuallyOn()
    {
        AnimSetTrigger("stun");
        isStunning = true;
    }

    public override void StunManuallyOff()
    {
        isStunning = false;
    }
    
    public override void DetectPlayer(float delayChase = 0)
    {
        base.DetectPlayer(delayChase);
    }
    
	public void CallMinion(){
		AnimSetTrigger ("callMinion");
		SetEnemyState (ENEMYSTATE.ATTACK);
		allowCheckAttack = false;
	}

	void CheckAttack(){
		//CHECK AND CALL MINION IF THIS ENEMY HAS SCRIPT CALLMINION
			switch (attackType) {
			case ATTACKTYPE.RANGE:
				if (rangeAttack.AllowAction ()) {
					SetEnemyState (ENEMYSTATE.ATTACK);

					if (rangeAttack.CheckPlayer (isFacingRight ())) {
						rangeAttack.Action ();
						AnimSetTrigger ("shoot");
						DetectPlayer ();
					} else if (!rangeAttack.isAttacking && enemyState == ENEMYSTATE.ATTACK) {
						SetEnemyState (ENEMYSTATE.WALK);
					}
				}

				break;
			case ATTACKTYPE.MELEE:
				if (meleeAttack.AllowAction ()) {
					if (meleeAttack.CheckPlayer (isFacingRight ())) {
						SetEnemyState (ENEMYSTATE.ATTACK);
						meleeAttack.Action ();
						AnimSetTrigger ("melee");
					} else if (!meleeAttack.isAttacking && enemyState == ENEMYSTATE.ATTACK) {
						SetEnemyState (ENEMYSTATE.WALK);
					}
				}
				break;

			case ATTACKTYPE.THROW:
				if (throwAttack.AllowAction ()) {
					SetEnemyState (ENEMYSTATE.ATTACK);

					if (throwAttack.CheckPlayer ()) {
						throwAttack.Action ();
						AnimSetTrigger ("throw");
					} else if (!throwAttack.isAttacking && enemyState == ENEMYSTATE.ATTACK) {
						SetEnemyState (ENEMYSTATE.WALK);
					}
				}
				break;
			default:
				break;
		}
	}

	void AllowCheckAttack(){
		allowCheckAttack = true;
	}

	void HandleAnimation(){
		AnimSetFloat ("speed", Mathf.Abs (velocity.x));
		AnimSetBool ("isRunning", Mathf.Abs (velocity.x) > walkSpeed);
        AnimSetBool("isStunning", isStunning);
    }

	public void SetForce(float x, float y){
		velocity = new Vector3 (x, y, 0);
	} 
    
	public void AnimMeleeAttackStart(){
		meleeAttack.Check4Hit ();
	}

	public void AnimMeleeAttackEnd(){
		meleeAttack.EndCheck4Hit ();
	}

	public void AnimThrow(){
		throwAttack.Throw (isFacingRight());
	}

	public void AnimShoot(){
		rangeAttack.Shoot (isFacingRight ());
	}

	public void AnimCallMinion(){
		callMinion.CallMinion (isFacingRight ());
		Invoke ("AllowCheckAttack", 2);
	}

	public override void Die ()
	{
		if (isDead)
			return;

		base.Die ();

		isDead = true;

		CancelInvoke ();

		var cols= GetComponents<BoxCollider2D>();
		foreach (var col in cols)
			col.enabled = false;

		if (spawnItem && spawnItem.spawnWhenDie)
			spawnItem.Spawn ();
        
        AnimSetBool("isDead", true);

        if (enemyEffect == ENEMYEFFECT.BURNING)
			return;

		if (enemyEffect == ENEMYEFFECT.EXPLOSION || dieBehavior == DIEBEHAVIOR.DESTROY) {
			gameObject.SetActive (false);
			return;
		}
        
		StopAllCoroutines ();
            StartCoroutine(DisableEnemy(AnimationHelper.getAnimationLength(anim, "Die") + 2f));
    }

	public override void Hit (Vector2 force, bool pushBack = false, bool knockDownRagdoll = false, bool shock = false)
	{
		if (!isPlaying || isStunning)
			return;

		base.Hit (force, pushBack, knockDownRagdoll, shock);
		if (isDead)
			return;

        AnimSetTrigger("hit");

        if (spawnItem && spawnItem.spawnWhenHit)
			spawnItem.Spawn ();

        if (knockDownRagdoll)
            ;
        else if (pushBack)
            StartCoroutine(PushBack(force));
        else if (shock)
            StartCoroutine(Shock());
        else
            ;
    }

	public override void KnockBack (Vector2 force, float stunningTime = 0)
	{
		base.KnockBack (force);

        SetForce(force.x, force.y);
    }

	public IEnumerator PushBack(Vector2 force){
		SetForce (force.x, force.y);

		if (isDead) {
			Die ();
			yield break;
		}
	}

    public IEnumerator Shock()
    {
        if (isDead)
        {
            Die();
            yield break;
        }
    }

    IEnumerator DisableEnemy(float delay){
		yield return new WaitForSeconds (delay);
        if (disableFX)
            SpawnSystemHelper.GetNextObject(disableFX, true).transform.position = spawnDisableFX != null ? spawnDisableFX.position : transform.position;
        gameObject.SetActive (false);
	}
}
