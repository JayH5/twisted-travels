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

	[SerializeField] LayerMask whatIsGround;			// A mask determining what is ground to the character
	
	Transform groundCheck;								// A position marking where to check if the player is grounded.
	float groundedRadius = .2f;							// Radius of the overlap circle to determine if grounded
	bool grounded = false;								// Whether or not the player is grounded.
	Transform ceilingCheck;								// A position marking where to check for ceilings
	//float ceilingRadius = .01f;						// Radius of the overlap circle to determine if the player can stand up
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

	// We cheat physics by preserving the player's velocity after rotating gravity
	private float preRotationVelocity;
	// If we restore all the velocity then things are too quick... tone it down a bit
	public float postRotationBoostFraction = 0.7f;

	private GestureHandler handler;

    void Awake()
	{
		// Setting up references.
		groundCheck = transform.Find("GroundCheck");
		ceilingCheck = transform.Find("CeilingCheck");
		anim = GetComponent<Animator>();
		handler = GetComponent<GestureHandler>();
		handler.setSwipeReceiver(this);
	}


	void FixedUpdate()
	{
		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
		anim.SetBool("Ground", grounded);

		// Set the vertical animation
		// TODO: calculate vertical speed properly
		anim.SetFloat("vSpeed", rigidbody2D.velocity.y);

		anim.SetFloat("Speed", Vector3.Magnitude(transform.right));

		Vector2 right = new Vector2(transform.right.x, transform.right.y);
		right.Normalize();

		// Add running force if grounded
		if (grounded)
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
			beginRotation(RotationDirection.Clockwise);
			break;
		case SwipeDirection.Down:
			beginRotation(RotationDirection.Anticlockwise);
			break;
		case SwipeDirection.Left:
			trySlack();
			break;
		case SwipeDirection.Right:
			tryDash();
			break;
		}
	}

	private void tryJump()
	{
		if (grounded)
		{
			AudioSource.PlayClipAtPoint(jumpSound, new Vector3(0,0,0));
			anim.SetBool("Ground", false);
			rigidbody2D.AddForce(transform.up * jumpForce);
		}
	}

	private void trySlack()
	{
		if (grounded && slackCoolDown <= 0.0f)
		{
			slackCoolDown = slackCoolDownTime;
			StartCoroutine(slackAnimation());
		}
	}

	private void tryDash()
	{
		if (grounded && dashCoolDown <= 0.0f)
		{
			AudioSource.PlayClipAtPoint(dashSound, new Vector3(0,0,0));
			dashCoolDown = dashCoolDownTime;
			StartCoroutine(dashAnimation());
		}
	}

	private void beginRotation(RotationDirection direction)
	{
		if (currentRotationDirection != RotationDirection.None)
			return; // If not finished rotating, ignore new requests

		AudioSource.PlayClipAtPoint(rotateSound, new Vector3());

		targetRotation = currentRotation + (int) direction;
		currentRotationDirection = direction;

		Quaternion from = Quaternion.AngleAxis(currentRotation, Vector3.forward);
		Quaternion to = Quaternion.AngleAxis(targetRotation, Vector3.forward);

		// Need to "restore" speed after rotation. Store it here.
		preRotationVelocity = Mathf.Sqrt(Vector2.SqrMagnitude(rigidbody2D.velocity));

		StartCoroutine (rotationAnimation(from, to));
	}

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
