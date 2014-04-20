using UnityEngine;
using System.Collections;

public class RunningScript : MonoBehaviour {

	public Vector2 speed = new Vector2(10,0);
	
	public float gravityY = -150f;
	public float gravityX = 0.0f;
	public Vector3 targetUp = new Vector3(0, -1, 0);
	public float damping = 10;

	public float jumpTimer = 0;
	public float jumpSpeed = 0;
	//public float jumpForce = 0;
	//public float jumpHeight = 0;

	public Vector2 movement;	

	public bool jump = false;
	private bool touchingPlatform;

	private bool isFalling = false;
	private SwipeHandler camera;

	void Start()
	{
		//get player reference to change gravity and speed/direction
		GameObject go = GameObject.Find ("Main Camera");	
		camera = go.GetComponent<SwipeHandler> ();
	}

	void Update () {
		//movement forward plus the effect of "gravity"
		movement = new Vector2(speed.x + (gravityX * Time.deltaTime), speed.y + (gravityY * Time.deltaTime));
		//to get sprite facing the same way they are running
		transform.up = Vector3.Slerp(transform.up, targetUp, Time.deltaTime * damping);

		//jumping
		if (jump)
		{
			jumpTimer += 1;
			//touchingPlatform = false;
			if (jumpTimer >= 20)
			{
				jumpTimer = 0;
				jump = false;
			}
		}
	}

	/*void OnCollisionEnter () {
		touchingPlatform = true;
	}
	
	void OnCollisionExit () {
		touchingPlatform = false;
	}*/

	void FixedUpdate(){
		rigidbody2D.velocity = movement; //move the sprite every fixed update;

		//rigidbody2D.velocity += new Vector2(0,jumpHeight);
		//rigidbody2D.AddForce (new Vector2 (0, jumpForce));
		//jumpForce = 0;
		//rigidbody2D.velocity.y += jumpHeight;
		int cameraRotation = Mathf.RoundToInt(camera.currentRotation.z);
		if (jump)
		{
			if (cameraRotation == 90)
			{
				rigidbody2D.velocity += new Vector2 (-jumpSpeed * (1/jumpTimer), 0);
			}
			else if (cameraRotation == 180)
			{
				rigidbody2D.velocity += new Vector2 (0, -jumpSpeed * (1/jumpTimer));
			}
			else if (cameraRotation == 270)
			{
				rigidbody2D.velocity += new Vector2 (jumpSpeed * (1/jumpTimer), 0);
			}
			else if (cameraRotation == 0)
			{
				rigidbody2D.velocity += new Vector2 (0, jumpSpeed * (1/jumpTimer));
			}
		}
			
	}
}
