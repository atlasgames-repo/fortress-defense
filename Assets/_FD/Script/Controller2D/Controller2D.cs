using UnityEngine;
using System.Collections;

public class Controller2D : RaycastController {
	
	float maxClimbAngle = 80;
	float maxDescendAngle = 80;
	float checkGroundAheadLength = 0.35f;
	
	public CollisionInfo collisions;
	[HideInInspector]
	public Vector2 playerInput;
	[HideInInspector]
	public bool HandlePhysic = true;
	[HideInInspector]
	public bool inverseGravity = false;

    private bool isFacingRight;
    bool ignoreCheckGroundAhead = true;

    public override void Start() {
		base.Start ();
		collisions.faceDir = 1;
	}
	
	public void Move(Vector3 velocity, bool standingOnPlatform, bool _isFacingRight = false, bool _ignoreCheckGroundAhead = true) {
        isFacingRight = _isFacingRight;
        ignoreCheckGroundAhead = _ignoreCheckGroundAhead;
        Move (velocity, Vector2.zero, standingOnPlatform);
	}

	public void Move(Vector3 velocity, Vector2 input, bool standingOnPlatform = false) {
		CalculateRaySpacing ();
		UpdateRaycastOrigins ();
		collisions.Reset ();
		collisions.velocityOld = velocity;
		playerInput = input;

		if (velocity.x != 0) {
			collisions.faceDir = (int)Mathf.Sign (velocity.x);
		}

		if (velocity.y < 0) {
			DescendSlope (ref velocity);
		}

		if (HandlePhysic) {
			HorizontalCollisions (ref velocity);
			if (velocity.y != 0) {
				VerticalCollisions (ref velocity);
			}
		}

        CheckGroundedAhead(velocity);

        //if (!ignoreCheckGroundAhead)
        //{
        //   if(!CheckGroundedAhead(velocity))
        //    {
        //        velocity.x = 0;
        //    }
        //}


        //if (collisions.right || collisions.left)
        //    velocity.x = 0;

        transform.Translate (velocity,Space.World);

		if (standingOnPlatform) {
			collisions.below = true;
		}

        //if (gameObject == GameManager.Instance.Player.gameObject)
        //    CameraFollow.Instance.DoLateUpdate();
	}

    [ReadOnly] public bool isWall;

	void HorizontalCollisions(ref Vector3 velocity) {
		float directionX = collisions.faceDir;
		float rayLength = Mathf.Abs (velocity.x) + skinWidth;
//		Debug.Log (directionX);
		if (Mathf.Abs(velocity.x) < skinWidth) {
			rayLength = 5*skinWidth;
		}

        isWall = false;

        for (int i = 0; i < horizontalRayCount; i ++) {
			Vector2 rayOrigin = (directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight;
			rayOrigin += (Vector2) transform.up * (horizontalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

			Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength,Color.red);

			if (hit) {

				if (hit.distance == 0) {
					continue;
				}
			
				float slopeAngle = Vector2.Angle (hit.normal, transform.up);

                if (slopeAngle > 85 && slopeAngle < 95)
                    isWall = true;

				if (i == 0 && slopeAngle <= maxClimbAngle) {
					if (collisions.descendingSlope) {
						collisions.descendingSlope = false;
						velocity = collisions.velocityOld;
					}
					float distanceToSlopeStart = 0;
					if (slopeAngle != collisions.slopeAngleOld) {
						distanceToSlopeStart = hit.distance - skinWidth;
						velocity.x -= distanceToSlopeStart * directionX;
					}
					ClimbSlope (ref velocity, slopeAngle);
					velocity.x += distanceToSlopeStart * directionX;
				}

				if (!collisions.climbingSlope || slopeAngle > maxClimbAngle) {
					velocity.x = (hit.distance - skinWidth) * directionX;
					rayLength = hit.distance;

					if (collisions.climbingSlope) {
						velocity.y = Mathf.Tan (collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs (velocity.x);
					}

					collisions.left = directionX == -1;
					collisions.right = directionX == 1;

					collisions.ClosestHit = hit;
				}
			}
		}
	}
	
	void VerticalCollisions(ref Vector3 velocity) {
		float directionY = Mathf.Sign (velocity.y);
		float rayLength = Mathf.Abs (velocity.y) + skinWidth;

		for (int i = 0; i < verticalRayCount; i ++) {

			Vector2 rayOrigin = (directionY == -1)?raycastOrigins.bottomLeft:raycastOrigins.topLeft;
			rayOrigin += (Vector2)Vector2.right * (verticalRaySpacing * i + velocity.x);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

			Debug.DrawRay(rayOrigin, transform.up * directionY * rayLength,Color.red);

			if (hit) {
				if (hit.collider.tag == "Through") {
					if (directionY == (inverseGravity?-1 : 1) || hit.distance == 0) {
						continue;
					}

                    //Debug.LogError(collisions.fallingThroughPlatform+"/"+ playerInput.y);
					if (collisions.fallingThroughPlatform) {
						continue;
					}
					if (playerInput.y == -1) {
						collisions.fallingThroughPlatform = true;
						Invoke ("ResetFallingThroughPlatform", .2f);
						continue;
					}
				}
     //           else if ((GameManager.Instance.Player.isClimbing && GameManager.Instance.Player.currentLadder && !GameManager.Instance.Player.currentLadder.GetComponent<Ladder8DirsZone> ()) && GetComponent<Player>()!=null)
					//continue;

				velocity.y = (hit.distance - skinWidth) * directionY;
				rayLength = hit.distance;

				if (collisions.climbingSlope) {
					velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
				}

				if (!inverseGravity) {
					collisions.below = directionY == -1;
					collisions.above = directionY == 1;
				} else {
					collisions.below = directionY == 1;
					collisions.above = directionY == -1;
				}


				collisions.ClosestHit = hit;

                collisions.hitBelowObj = null;
                collisions.hitAboveObj = null;


                if (directionY == -1)
                    collisions.hitBelowObj = hit.collider.gameObject;

                if (directionY == 1)
                    collisions.hitAboveObj = hit.collider.gameObject;
            }
		}

		if (collisions.climbingSlope) {
			float directionX = Mathf.Sign(velocity.x);
			rayLength = Mathf.Abs(velocity.x) + skinWidth;
			Vector2 rayOrigin = ((directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight) + (Vector2) transform.up * velocity.y;
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin,transform.right * directionX,rayLength,collisionMask);

			if (hit) {
				float slopeAngle = Vector2.Angle(hit.normal,transform.up);
				if (slopeAngle != collisions.slopeAngle) {
					velocity.x = (hit.distance - skinWidth) * directionX;
					collisions.slopeAngle = slopeAngle;
				}
			}
		}
	}

    bool CheckGroundedAhead(Vector3 velocity)
    {
        float directionX = collisions.faceDir;

        if (velocity.x == 0)
            directionX = isFacingRight ? 1 : -1;

        Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, checkGroundAheadLength, collisionMask);


        Debug.DrawRay(rayOrigin, Vector2.down * checkGroundAheadLength, Color.green);

        if (hit)
        {
            collisions.isGrounedAhead = true;
            return true;
        }
        else
            return false;


    }

	void ClimbSlope(ref Vector3 velocity, float slopeAngle) {
		float moveDistance = Mathf.Abs (velocity.x);
		float climbVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;

		if (velocity.y <= climbVelocityY) {
			velocity.y = climbVelocityY;
			velocity.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (velocity.x);
			collisions.below = true;
			collisions.climbingSlope = true;
			collisions.slopeAngle = slopeAngle;
		}
	}

	void DescendSlope(ref Vector3 velocity) {
		float directionX = Mathf.Sign (velocity.x);
		Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
		RaycastHit2D hit = Physics2D.Raycast (rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

		if (hit) {
			float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
			if (slopeAngle != 0 && slopeAngle <= maxDescendAngle) {
				if (Mathf.Sign(hit.normal.x) == directionX) {
					if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x)) {
						float moveDistance = Mathf.Abs(velocity.x);
						float descendVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;
						velocity.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (velocity.x);
						velocity.y -= descendVelocityY;

						collisions.slopeAngle = slopeAngle;
						collisions.descendingSlope = true;
						collisions.below = true;
					}
				}
			}
		}
	}

	void ResetFallingThroughPlatform() {
		collisions.fallingThroughPlatform = false;
	}

	public struct CollisionInfo {
		public bool above, below;
		public bool left, right;

		public RaycastHit2D ClosestHit;
        public GameObject hitBelowObj, hitAboveObj;

        public bool isGrounedAhead;

		public bool climbingSlope;
		public bool descendingSlope;
		public float slopeAngle, slopeAngleOld;
		public Vector3 velocityOld;
		public int faceDir;
		public bool fallingThroughPlatform;

		public void Reset() {
			above = below = false;
			left = right = false;
			isGrounedAhead = false;
			climbingSlope = false;
			descendingSlope = false;

			slopeAngleOld = slopeAngle;
			slopeAngle = 0;
		}
	}

}
