using UnityEngine;
using Gestures;
using System.Collections;

public class PlatformerCharacter2D : MonoBehaviour, IGestureReceiver
{
	public float dashCoolDownTime = 2.0f; // Dash cooldown in seconds
	private float dashCoolDown = 0.0f;
	public float dashForce = 2.0f;
	public float dashDuration = 2.0f; // Dash duration in seconds

	public float slackCoolDownTime = 2.0f; // Slack cooldown time in seconds
	private float slackCoolDown = 0.0f;
	public float slackForce = 2.0f;
	public float slackDuration = 2.0f; // Slack duration in seconds

	public float runForce = 4f;				// The fastest the player can travel in the x axis.
	public float jumpForce = 40f;			// Amount of force added when the player jumps.

	public float maxSpeed = 8f;
	public float flyTimeBeforeDieTime;

	public AudioClip jumpSound;
	public AudioClip dashSound;
	public AudioClip rotateSound;

	public ParticleSystem dashParticle;

	Animator anim;										// Reference to the player's animator component.
	public AudioClip boxPortalSpawnSound;

	public float gravityAcceleration = -9.8f; // m/s^2

	public float rotationTime = .5f; // seconds

	private int currentRotation = 0;
	private int targetRotation = 0;
	private float flyTimer = 0.0f;

	public bool Dead
	{
		get { return dead; }
	}
	private bool dead = false;
	
	private RotationDirection currentRotationDirection = RotationDirection.None;

	// We cheat physics by preserving the player's velocity after rotating gravity
	private float preRotationVelocity;
	// If we restore all the velocity then things are too quick... tone it down a bit
	public float postRotationBoostFraction = 0.7f;

	/// <summary>
	/// Gets or sets a value indicating whether the player is grounded.
	/// </summary>
	/// <value><c>true</c> if this instance is grounded; otherwise, <c>false</c>.</value>
	public bool IsGrounded
	{
		get { return isGrounded; }
		set
		{
			if (value != isGrounded)
			{
				// Becoming ungrounded
				if (isGrounded)
				{
					flyTimer = flyTimeBeforeDieTime;
				}
				// Becoming grounded
				else
				{
					flyTimer = 0;
				}
			}

			isGrounded = value;
			anim.SetBool("Ground", value);

		}
	}
	bool isGrounded = false; // Start off the ground

	public LayerMask floorLayerMask;
	public float cliffCheckDistance = 1.0f;
	public float cliffCheckDrop = 0.2f;
	public float cliffCheckRadius = 0.2f;

	/// <summary>
	/// Maximum distance to search downwards for the floor.
	/// </summary>
	public float groundRayMaxDistance = 3.0f;

	/// <summary>
	/// Maximum dstance to search to the right for a wall.
	/// </summary>
	public float wallRayMaxDistance = 1.7f;

	/// <summary>
	/// Debugging for cliff detection. Set true for cast rays to be drawn.
	/// </summary>
	public bool debug = false;
	Vector3 lastGroundRayOrigin;
	Vector3 lastGroundRayHit;
	Vector3 lastCliffCheck;
	Vector3 lastWallRayOrigin;
	Vector3 lastWallRayHit;

	public BasicTrackingCamera cameraScript;
	
	public float Distance
	{
		get { return distance; }
	}
	float distance = 0f;
	Vector3 previousPosition;
	float nextDistanceUpdate = 0f; // For debug
	float distanceBetweenUpdates = 10f;

	/// <summary>
	/// Gets a value indicating whether the player is dashing.
	/// </summary>
	public bool IsDashing
	{
		get { return isDashing; }
	}
	bool isDashing = false;

	/// <summary>
	/// Gets a value indicating whether the player is slacking.
	/// </summary>
	public bool IsSlacking
	{
		get { return isSlacking; }
	}
	bool isSlacking = false;

	public EffectsPlayer effectsPlayer;

	void Awake()
	{
		anim = GetComponent<Animator>();
		anim.SetBool("Ground", IsGrounded);

		// I don't know how to do the listener pattern in Unity so this is haxx
		GestureHandler handler = GetComponent<GestureHandler>();
		handler.setSwipeReceiver(this);

		dashParticle.Stop ();

		previousPosition = transform.position;
	}

	void FixedUpdate()
	{
		// Set the vertical animation
		float vSpeed = Vector2.Dot(rigidbody2D.velocity, transform.up);
		float speed = Vector2.Dot(rigidbody2D.velocity, transform.right);

		anim.SetFloat("vSpeed", vSpeed);
		anim.SetFloat("Speed", speed);

		Vector2 right = transform.right.normalized;

		// Add running force if grounded and below max speed
		if (IsGrounded)
		{
			if (speed < maxSpeed) // If below target speed, speed up
				rigidbody2D.AddForce(right * runForce);
			else if (!isDashing) // If above target speed and done dashing, slow down
				rigidbody2D.AddForce(-right * runForce);
		}

		if (slackCoolDown > 0.0f)
		{
			slackCoolDown -= Time.deltaTime;
		}

		if (dashCoolDown > 0.0f)
		{
			dashCoolDown -= Time.deltaTime;
		}

		if (flyTimer > 0.0f)
		{
			flyTimer -= Time.deltaTime;
			if (flyTimer < 0.0f)
			{
				dead = true;
			}
		}

		// Add gravity
		Vector2 up = new Vector2(transform.up.x, transform.up.y);
		up.Normalize();
		Vector2 gravityForce = rigidbody2D.mass * gravityAcceleration * up;
		rigidbody2D.AddForce(gravityForce);

		// Update the distance tracking - slightly hax distance calculation but should be fine
		distance += Vector3.Dot(transform.position - previousPosition, transform.right);
		previousPosition = transform.position;
	}

	void Update()
	{
		if (debug)
		{
			Debug.DrawLine(lastWallRayOrigin, lastWallRayHit);
			Debug.DrawLine(lastGroundRayOrigin, lastGroundRayHit);
			Debug.DrawLine(lastGroundRayHit, lastCliffCheck);

			if (distance >= nextDistanceUpdate)
			{
				Debug.Log("Distance run: " + distance);
				nextDistanceUpdate += distanceBetweenUpdates;
			}
		}
	}

	public void onTap()
	{
		tryJump();
	}

	public void onSwipe(SwipeDirection direction)
	{
		//if (!grounded) // No rotating in the air
		//	return;

		switch (direction)
		{
		case SwipeDirection.Up:
			tryRotate(RotationDirection.Clockwise);
			break;
		case SwipeDirection.Down:
			tryRotate(RotationDirection.Anticlockwise);
			break;
		case SwipeDirection.Left:
			trySlack();
			break;
		case SwipeDirection.Right:
			tryDash();
			break;
		}
	}

	private void tryRotate(RotationDirection direction)
	{
		if (canRotate(direction))
		{
			Debug.Log("Can rotate, beginning...");
			beginRotation(direction);
		}
		else
		{
			cameraScript.shift(direction);
		}
	}

	private void tryJump()
	{
		if (IsGrounded)
		{
			effectsPlayer.jump();
			anim.SetBool("Ground", false);
			rigidbody2D.AddForce(transform.up * jumpForce);
		}
	}

	private void trySlack()
	{
		if (IsGrounded && slackCoolDown <= 0.0f)
		{
			slackCoolDown = slackCoolDownTime;
			StartCoroutine(slackAnimation());
		}
	}

	private void tryDash()
	{
		if (dashCoolDown <= 0.0f)
		{
			effectsPlayer.dash();
			dashCoolDown = dashCoolDownTime;
			StartCoroutine(dashAnimation());
		}
	}

	private bool canRotate(RotationDirection direction)
	{
		if (currentRotationDirection != RotationDirection.None)
			return false; // If not finished rotating, ignore new requests

		// If trying to rotate clockwise, check for cliff ahead
		if (direction == RotationDirection.Clockwise)
		{
			// Check for wall first because that check is more accurate
			return !checkForUpcomingWall() && checkForUpcomingCliff();
		}
		else if (direction == RotationDirection.Anticlockwise)
		{
			return checkForUpcomingWall();
		}

		return false;
	}

	private void beginRotation(RotationDirection direction)
	{
		effectsPlayer.rotate();

		targetRotation = currentRotation + (int) direction;
		currentRotationDirection = direction;

		Quaternion from = Quaternion.AngleAxis(currentRotation, Vector3.forward);
		Quaternion to = Quaternion.AngleAxis(targetRotation, Vector3.forward);

		// Need to "restore" speed after rotation. Store it here.
		preRotationVelocity = Mathf.Sqrt(Vector2.SqrMagnitude(rigidbody2D.velocity));

		StartCoroutine (rotationAnimation(from, to));
	}

	public void OnCollisionDetected(Collider2D collider)
	{
		// TODO: Die
		Debug.Log("Collision detected with a " + collider.name);
		if (isDashing && collider.tag == "Breakable box")
		{
			print ("here");
			GameObject box = collider.gameObject;
			box.GetComponent<BoxSmash>().smash(transform.right);
		}

		if(collider.tag == "BoxPortal")
		{
			AudioSource.PlayClipAtPoint(boxPortalSpawnSound, new Vector3 (0,0,0));
			collider.tag = "Untagged";
		}else if(collider.tag == "Floor")
		{ Debug.Log("Collided with Wall: You DEAD!");
			dead = true;
		}
	}

	private bool checkForUpcomingCliff()
	{
		// To check for an upcoming cliff, we check for a gap in the floor ahead
		Vector2 groundBelow = findGroundBelow();

		if (groundBelow == default(Vector2))
		{
			Debug.LogWarning("Ground not found below player!");
			return true; // Let them rotate anyway
		}

		// Go right and down by some distance
		Vector2 up = transform.up.normalized;
		Vector2 right = transform.right.normalized;
		Vector2 point = groundBelow + right * cliffCheckDistance - up * cliffCheckDrop;

		if (debug)
			lastCliffCheck = new Vector3(point.x, point.y);

		return Physics2D.OverlapCircle(point, cliffCheckRadius, floorLayerMask) == null;
	}

	private Vector2 findGroundBelow()
	{
		// Find the back of the feet, they are the origin of the raycast
		Vector2 origin = transform.position
			+ transform.up.normalized * -0.61f
			+ transform.right.normalized * -0.16f;

		// Want to cast a ray downwards to hit the ground
		Vector2 direction = -transform.up;

		// Cast the ray
		RaycastHit2D hit = Physics2D.Raycast(origin, direction, groundRayMaxDistance, floorLayerMask);

		if (debug)
		{
			lastGroundRayOrigin = origin;
			lastGroundRayHit = hit.point;
		}

		// If we hit something return position of hit, else zero
		return hit.collider != null ? hit.point : default(Vector2);
	}

	private bool checkForUpcomingWall()
	{
		// Cast ray from player to right and try hit wall
		Vector2 origin = transform.position;
		Vector2 direction = transform.right;

		RaycastHit2D hit = Physics2D.Raycast(origin, direction, wallRayMaxDistance, floorLayerMask);

		if (debug)
		{
			lastWallRayOrigin = origin;
			lastWallRayHit = hit.collider != null ? hit.point : origin + direction.normalized * wallRayMaxDistance;
		}

		return hit.collider != null;
	}

	//
	// ANIMATION COROUTINES
	//
	IEnumerator rotationAnimation(Quaternion from, Quaternion to)
	{
		for (float i = 0.0f; i < 1.0f; i += Time.deltaTime / rotationTime)
		{
			transform.rotation = Quaternion.Slerp(from, to, i);
			yield return new WaitForSeconds(0);
		}
		currentRotation = targetRotation;
		currentRotationDirection = RotationDirection.None;

		transform.rotation = to;

		// Restore pre-rotation speed
		rigidbody2D.velocity = transform.right * preRotationVelocity * postRotationBoostFraction;
	}

	IEnumerator dashAnimation()
	{
		//Debug.Log ("Dashing!");
		isDashing = true;
		dashParticle.Play();
		for (float i = 0.0f; i < 1.0f; i += Time.deltaTime / dashDuration)
		{
			float magnitude = Mathf.Lerp(dashForce, 0.0f, i);
			rigidbody2D.AddForce(transform.right * magnitude);
			yield return new WaitForFixedUpdate();
		}
		dashParticle.particleSystem.Stop();
		isDashing = false;
	}

	IEnumerator slackAnimation()
	{
		//Debug.Log ("Slacking off...");
		isSlacking = true;
		for (float i = 0.0f; i < 1.0f; i += Time.deltaTime / slackDuration)
		{
			float magnitude = Mathf.Lerp(slackForce, 0.0f, i);
			rigidbody2D.AddForce(-transform.right * magnitude);
			yield return new WaitForFixedUpdate();
		}
		isSlacking = false;
	}


}
