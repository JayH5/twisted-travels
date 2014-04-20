using UnityEngine;

public class PlatformerCharacter2D : MonoBehaviour 
{
	bool facingRight = true;							// For determining which way the player is currently facing.

	[SerializeField] float maxSpeed = 10f;				// The fastest the player can travel in the x axis.
	[SerializeField] float jumpForce = 400f;			// Amount of force added when the player jumps.	

	[Range(0, 1)]
	//[SerializeField] float crouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	
	[SerializeField] bool airControl = false;			// Whether or not a player can steer while jumping;
	[SerializeField] LayerMask whatIsGround;			// A mask determining what is ground to the character
	
	Transform groundCheck;								// A position marking where to check if the player is grounded.
	float groundedRadius = .2f;							// Radius of the overlap circle to determine if grounded
	bool grounded = false;								// Whether or not the player is grounded.
	Transform ceilingCheck;								// A position marking where to check for ceilings
	float ceilingRadius = .01f;							// Radius of the overlap circle to determine if the player can stand up
	Animator anim;										// Reference to the player's animator component.

	//My added stuff
	public float gravityY = -25f;
	public float gravityX = 0.0f;
	public float speedX = 1; 
	public float speedY = 0;

	public Vector3 targetUp = new Vector3(0, 1, 0);
	public float damping = 10;

	public SwipeHandler camera;


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
	}


	public void Move(bool crouch, bool jump)
	{

		// If crouching, check to see if the character can stand up
		if(!crouch && anim.GetBool("Crouch"))
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if( Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround))
				crouch = true;
		}

		// Set whether or not the character is crouching in the animator
		anim.SetBool("Crouch", crouch);

		//only control the player if grounded or airControl is turned on
		if(grounded || airControl)
		{
			// Reduce the speed if crouching by the crouchSpeed multiplier
			//move = (crouch ? move * crouchSpeed : move);

			// The Speed animator parameter is set to the absolute value of the horizontal input.
			if (speedX > 0)
				anim.SetFloat("Speed", Mathf.Abs(speedX));
			else if (speedY > 0)
				anim.SetFloat("Speed", Mathf.Abs(speedY));

			// Move the character depending on camera rotation
			if (camera.currentRotation.z > 89 && camera.currentRotation.z < 91)
			{
				rigidbody2D.velocity = new Vector2((speedX * maxSpeed) + rigidbody2D.velocity.x + (gravityX * Time.deltaTime), (speedY * maxSpeed) + (gravityY * Time.deltaTime));
			}
			else if (camera.currentRotation.z > 179 && camera.currentRotation.z < 181)
			{
				rigidbody2D.velocity = new Vector2((speedX * maxSpeed) + (gravityX * Time.deltaTime), (speedY * maxSpeed) + rigidbody2D.velocity.y + (gravityY * Time.deltaTime));
			}
			else if (camera.currentRotation.z > 269 && camera.currentRotation.z < 271)
			{
				rigidbody2D.velocity = new Vector2((speedX * maxSpeed) + rigidbody2D.velocity.x + (gravityX * Time.deltaTime), (speedY * maxSpeed) + (gravityY * Time.deltaTime));
			}
			else if (camera.currentRotation.z > -1 && camera.currentRotation.z < 1)
			{
				rigidbody2D.velocity = new Vector2((speedX * maxSpeed)+ (gravityX * Time.deltaTime), (speedY * maxSpeed) + rigidbody2D.velocity.y + (gravityY * Time.deltaTime));
			}

			// If the input is moving the player right and the player is facing left...
//			if(move > 0 && !facingRight)
//				// ... flip the player.
//				Flip();
//			// Otherwise if the input is moving the player left and the player is facing right...
//			else if(move < 0 && facingRight)
//				// ... flip the player.
//				Flip();
		}

        // If the player should jump...
        if (grounded && jump) {
            // Add a vertical force to the player.
            anim.SetBool("Ground", false);
			//jump is in different directions, depending on gravity
            //rigidbody2D.AddForce(new Vector2(0f, jumpForce));

			if (camera.currentRotation.z > 89 && camera.currentRotation.z < 91)
			{
				rigidbody2D.AddForce(new Vector2(-jumpForce, 0));
			}
			else if (camera.currentRotation.z > 179 && camera.currentRotation.z < 181)
			{
				rigidbody2D.AddForce(new Vector2(0, -jumpForce));
			}
			else if (camera.currentRotation.z > 269 && camera.currentRotation.z < 271)
			{
				rigidbody2D.AddForce(new Vector2(jumpForce, 0));
			}
			else if (camera.currentRotation.z > -1 && camera.currentRotation.z < 1)
			{
				rigidbody2D.AddForce(new Vector2(0, jumpForce));
			}
        }
	}

	
	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
