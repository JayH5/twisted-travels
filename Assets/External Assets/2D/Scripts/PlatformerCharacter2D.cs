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

	public AudioClip jumpSound;
	public AudioClip dashSound;
	public AudioClip rotateSound;

	Animator anim;										// Reference to the player's animator component.	

	public float gravityAcceleration = -9.8f; // m/s^2

	public float rotationTime = .5f; // seconds

	private int currentRotation = 0;
	private int targetRotation = 0;

	private enum RotationDirection
	{
		Anticlockwise = 90, Clockwise = -90, None = 0
	}
	private RotationDirection currentRotationDirection = RotationDirection.None;

	/// <summary>
	/// This variable keeps track of the next *correct* rotation as we detect walls and cliffs ahead of the character.
	/// </summary>
	private RotationDirection nextRotationDirection = RotationDirection.None;

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
			isGrounded = value;
			anim.SetBool("Ground", value);
		}
	}
	bool isGrounded = false; // Start off the ground

    void Awake()
	{
		anim = GetComponent<Animator>();
		anim.SetBool("Ground", IsGrounded);

		// I don't know how to do the listener pattern in Unity so this is haxx
		GestureHandler handler = GetComponent<GestureHandler>();
		handler.setSwipeReceiver(this);
	}

	void FixedUpdate()
	{
		// Set the vertical animation
		float vSpeed = Vector3.Dot(rigidbody2D.velocity, transform.up);
		anim.SetFloat("vSpeed", vSpeed);

		anim.SetFloat("Speed", Vector3.Magnitude(transform.right));

		Vector2 right = new Vector2(transform.right.x, transform.right.y);
		right.Normalize();

		// Add running force if grounded
		if (IsGrounded)
		{
			rigidbody2D.AddForce(right * runForce);
		}

		if (slackCoolDown > 0.0f)
		{
			slackCoolDown -= Time.deltaTime;
		}

		if (dashCoolDown > 0.0f)
		{
			dashCoolDown -= Time.deltaTime;
		}

		// Add gravity
		Vector2 up = new Vector2(transform.up.x, transform.up.y);
		up.Normalize();
		Vector2 gravityForce = rigidbody2D.mass * gravityAcceleration * up;
		rigidbody2D.AddForce(gravityForce);
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
			Debug.Log("Cannot rotate :(");
			// TODO: Provide some hint that player shouldn't have tried to rotate
		}
	}

	private void tryJump()
	{
		if (IsGrounded)
		{
			AudioSource.PlayClipAtPoint(jumpSound, Vector3.zero);
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
		if (IsGrounded && dashCoolDown <= 0.0f)
		{
			AudioSource.PlayClipAtPoint(dashSound, Vector3.zero);
			dashCoolDown = dashCoolDownTime;
			StartCoroutine(dashAnimation());
		}
	}

	private void beginRotation(RotationDirection direction)
	{
		AudioSource.PlayClipAtPoint(rotateSound, Vector3.zero);

		targetRotation = currentRotation + (int) direction;
		currentRotationDirection = direction;

		Quaternion from = Quaternion.AngleAxis(currentRotation, Vector3.forward);
		Quaternion to = Quaternion.AngleAxis(targetRotation, Vector3.forward);

		// Need to "restore" speed after rotation. Store it here.
		preRotationVelocity = Mathf.Sqrt(Vector2.SqrMagnitude(rigidbody2D.velocity));

		StartCoroutine (rotationAnimation(from, to));
	}

	private bool canRotate(RotationDirection direction)
	{
		if (currentRotationDirection != RotationDirection.None)
			return false; // If not finished rotating, ignore new requests

		return direction == nextRotationDirection;
	}

	public void OnUpcomingWallDetected()
	{
		if (nextRotationDirection == RotationDirection.None)
		{
			Debug.Log("Upcoming wall!");
			nextRotationDirection = RotationDirection.Anticlockwise;
		}
	}

	public void OnUpcomingCliffDetected()
	{
		if (nextRotationDirection == RotationDirection.None)
		{
			Debug.Log("Upcoming cliff!");
			nextRotationDirection = RotationDirection.Clockwise;
		}
	}

	public void OnWallCollisionDetected()
	{
		// TODO: Die
		//Debug.Log("Wall collision detected!");
		//if (currentRotationDirection == RotationDirection.None)
			//nextRotationDirection = RotationDirection.Anticlockwise; // Save people stuck against walls for now
	}

	public void OnCliffEdgeDetected()
	{
		// TODO: Something?
		//Debug.Log("Cliff edge detected!");
		//if (currentRotationDirection == RotationDirection.None)
			//nextRotationDirection = RotationDirection.Clockwise; // Save people in free fall for now
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
		nextRotationDirection = RotationDirection.None;

		transform.rotation = to;

		// Restore pre-rotation speed
		rigidbody2D.velocity = transform.right * preRotationVelocity * postRotationBoostFraction;
	}

	IEnumerator dashAnimation()
	{
		//Debug.Log ("Dashing!");
		for (float i = 0.0f; i < 1.0f; i += Time.deltaTime / dashDuration)
		{
			float magnitude = Mathf.Lerp(dashForce, 0.0f, i);
			rigidbody2D.AddForce(transform.right * magnitude);
			yield return new WaitForFixedUpdate();
		}
	}

	IEnumerator slackAnimation()
	{
		//Debug.Log ("Slacking off...");
		for (float i = 0.0f; i < 1.0f; i += Time.deltaTime / slackDuration)
		{
			float magnitude = Mathf.Lerp(slackForce, 0.0f, i);
			rigidbody2D.AddForce(-transform.right * magnitude);
			yield return new WaitForFixedUpdate();
		}
	}

}
