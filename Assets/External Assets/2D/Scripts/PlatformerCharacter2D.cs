using UnityEngine;
using Gestures;
using System.Collections;

public class PlatformerCharacter2D : MonoBehaviour, IGestureReceiver
{
	public float runForce = 4f;				// The fastest the player can travel in the x axis.
	public float jumpForce = 40f;			// Amount of force added when the player jumps.	
	
	[SerializeField] LayerMask whatIsGround;			// A mask determining what is ground to the character
	
	Transform groundCheck;								// A position marking where to check if the player is grounded.
	float groundedRadius = .2f;							// Radius of the overlap circle to determine if grounded
	bool grounded = false;								// Whether or not the player is grounded.
	Transform ceilingCheck;								// A position marking where to check for ceilings
	//float ceilingRadius = .01f;							// Radius of the overlap circle to determine if the player can stand up
	Animator anim;										// Reference to the player's animator component.

	public float gravityAcceleration = -9.8f; // m/s^2

	public bool jump = false;									// To determine when the player presses jump

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

		// Add running force if grounded
		if (grounded)
		{
			Vector2 right = new Vector2(transform.right.x, transform.right.y);
			right.Normalize();
			rigidbody2D.AddForce(right * runForce);
		}

		// Add gravity
		Vector2 up = new Vector2(transform.up.x, transform.up.y);
		up.Normalize();
		Vector2 gravityForce = rigidbody2D.mass * gravityAcceleration * up;
		rigidbody2D.AddForce(gravityForce);
	
        // Jump if we're on the ground and jump pressed
        if (grounded && jump) {
            anim.SetBool("Ground", false);
			rigidbody2D.AddForce(up * jumpForce);
			jump = false;
        }
	}

	public void onTap()
	{
		jump = true;
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
				// TODO
				break;
			case SwipeDirection.Right:
				// TODO
				break;
		}
	}

	private void beginRotation(RotationDirection direction)
	{
		if (currentRotationDirection != RotationDirection.None)
			return; // If not finished rotating, ignore new requests

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

		// Restore pre-rotation speed
		rigidbody2D.velocity = transform.right * preRotationVelocity * postRotationBoostFraction;
	}

}
