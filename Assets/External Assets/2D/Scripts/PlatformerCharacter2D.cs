using UnityEngine;

public class PlatformerCharacter2D : MonoBehaviour 
{
	//bool facingRight = true;							// For determining which way the player is currently facing.

	[SerializeField] float maxSpeed = 10f;				// The fastest the player can travel in the x axis.
	[SerializeField] float jumpForce = 400f;			// Amount of force added when the player jumps.	

	[Range(0, 1)]
	//[SerializeField] float crouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	
	//[SerializeField] bool airControl = false;			// Whether or not a player can steer while jumping;
	[SerializeField] LayerMask whatIsGround;			// A mask determining what is ground to the character
	
	Transform groundCheck;								// A position marking where to check if the player is grounded.
	float groundedRadius = .2f;							// Radius of the overlap circle to determine if grounded
	bool grounded = false;								// Whether or not the player is grounded.
	Transform ceilingCheck;								// A position marking where to check for ceilings
	//float ceilingRadius = .01f;							// Radius of the overlap circle to determine if the player can stand up
	Animator anim;										// Reference to the player's animator component.

	//My added stuff
	public float gravityY = -25f;
	public float gravityX = 0.0f;
	public float speedX = 1; 
	public float speedY = 0;

	public bool jump = false;									// To determine when the player presses jump

	public Vector3 targetUp = new Vector3(0, 1, 0);
	public float damping = 10;

	public SwipeHandler swipeHandler;


    void Awake()
	{
		// Setting up references.
		groundCheck = transform.Find("GroundCheck");
		ceilingCheck = transform.Find("CeilingCheck");
		anim = GetComponent<Animator>();
	}


	void FixedUpdate()
	{
		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
		anim.SetBool("Ground", grounded);

		// Set the vertical animation
		anim.SetFloat("vSpeed", rigidbody2D.velocity.y);

		//slep the charactor to face the direcion required
		transform.up = Vector3.Slerp(transform.up, targetUp, Time.deltaTime * damping);

		//move!
		// The Speed animator parameter is set to the absolute value of the horizontal input.
		if (speedX > 0)
			anim.SetFloat("Speed", Mathf.Abs(speedX));
		else if (speedY > 0)
			anim.SetFloat("Speed", Mathf.Abs(speedY));
		
		// Move the character depending on  rotation
		if (swipeHandler.currentRotation.z > 89 && swipeHandler.currentRotation.z < 91)
		{
			rigidbody2D.velocity = new Vector2((speedX * maxSpeed) + rigidbody2D.velocity.x + (gravityX * Time.deltaTime), (speedY * maxSpeed) + (gravityY * Time.deltaTime));
		}
		else if (swipeHandler.currentRotation.z > 179 && swipeHandler.currentRotation.z < 181)
		{
			rigidbody2D.velocity = new Vector2((speedX * maxSpeed) + (gravityX * Time.deltaTime), (speedY * maxSpeed) + rigidbody2D.velocity.y + (gravityY * Time.deltaTime));
		}
		else if (swipeHandler.currentRotation.z > 269 && swipeHandler.currentRotation.z < 271)
		{
			rigidbody2D.velocity = new Vector2((speedX * maxSpeed) + rigidbody2D.velocity.x + (gravityX * Time.deltaTime), (speedY * maxSpeed) + (gravityY * Time.deltaTime));
		}
		else if (swipeHandler.currentRotation.z > -1 && swipeHandler.currentRotation.z < 1)
		{
			rigidbody2D.velocity = new Vector2((speedX * maxSpeed)+ (gravityX * Time.deltaTime), (speedY * maxSpeed) + rigidbody2D.velocity.y + (gravityY * Time.deltaTime));
		}
	
        //determine the direction of the jump based on gravity
        if (grounded && jump) {
            anim.SetBool("Ground", false);
			if (swipeHandler.currentRotation.z > 89 && swipeHandler.currentRotation.z < 91)
			{
				rigidbody2D.AddForce(new Vector2(-jumpForce, 0));
			}
			else if (swipeHandler.currentRotation.z > 179 && swipeHandler.currentRotation.z < 181)
			{
				rigidbody2D.AddForce(new Vector2(0, -jumpForce));
			}
			else if (swipeHandler.currentRotation.z > 269 && swipeHandler.currentRotation.z < 271)
			{
				rigidbody2D.AddForce(new Vector2(jumpForce, 0));
			}
			else if (swipeHandler.currentRotation.z > -1 && swipeHandler.currentRotation.z < 1)
			{
				rigidbody2D.AddForce(new Vector2(0, jumpForce));
			}
        }

		if (grounded)
			jump = false;
	}	
}
