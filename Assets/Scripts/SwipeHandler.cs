using UnityEngine;
using System.Collections;

public class SwipeHandler : MonoBehaviour {

	//swipe vars
	public float minMovement = 20.0f;

	// Flags to enable/disable touch detection
	public bool sendUpMessage = true;
	public bool sendDownMessage = true;
	public bool sendLeftMessage = true;
	public bool sendRightMessage = true;
	public GameObject MessageTarget = null;

	private Vector2 StartPos;
	private int SwipeID = -1;

	//rotation vars
	public int rotationStep = 5; //rotation degrees per step
	public Vector3 currentRotation = new Vector3 (0, 0, 0); 
	private int rotationDirection = 0; // -1 for clockwise, 1 for anti-clockwise
	private Vector3 targetRotation;
	private float rotangle; //testing
	private bool rotDone = true;

	//others
	public PlatformerCharacter2D player;

	void Start()
	{
		transform.eulerAngles = new Vector3 (0, 0, 0);
		//print (currentRotation.z);
	}

	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Escape)) {	Application.Quit();	}

		if (MessageTarget == null) { MessageTarget = gameObject; }

		if (!HandleKeyboardInput()) // Give precedence to keyboard controls
		{
			HandleTouchInput ();
		}

		//move camera with player.
		transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);

	}

	bool HandleKeyboardInput()
	{
		bool handled = false;
		if (Input.GetKeyDown (KeyCode.Space))
		{
			currentRotation = gameObject.transform.eulerAngles;
			player.jump = true;
			handled = true;
		}
		else if (Input.GetKeyDown (KeyCode.UpArrow))
		{
			MessageTarget.SendMessage ("OnSwipeUp", SendMessageOptions.DontRequireReceiver);
			handled = true;
		}
		else if (Input.GetKeyDown (KeyCode.DownArrow))
		{
			MessageTarget.SendMessage ("OnSwipeDown", SendMessageOptions.DontRequireReceiver);
			handled = true;
		}
		return handled;
	}

	bool HandleTouchInput()
	{
		bool handled = false;
		foreach (var touch in Input.touches) // For each finger... (but only keep track of one)
		{
			var position = touch.position;
			var fingerId = touch.fingerId;
			if (touch.phase == TouchPhase.Began && fingerId != SwipeID) // Begin touch
			{
				SwipeID = fingerId;
				StartPos = position;
				handled = true;
			}
			else if (touch.fingerId == SwipeID) // Continue touch detection
			{
				var delta = position - StartPos;
				// Swipe detected if finger moved more than `minMovement`
				if (touch.phase == TouchPhase.Moved && delta.magnitude > minMovement)
				{
					SwipeID = -1; // Swipe detected, this gesture is complete
					if (Mathf.Abs (delta.x) > Mathf.Abs (delta.y)) // Moved more horizontally than vertically
					{
						if (sendRightMessage && delta.x > 0)
						{
							MessageTarget.SendMessage ("OnSwipeRight", SendMessageOptions.DontRequireReceiver);
						}
						else if (sendLeftMessage && delta.x < 0)
						{
							MessageTarget.SendMessage ("OnSwipeLeft", SendMessageOptions.DontRequireReceiver);
						}
					}
					else
					{
						if (sendUpMessage && delta.y > 0)
						{
							MessageTarget.SendMessage ("OnSwipeUp", SendMessageOptions.DontRequireReceiver);
						}
						else if (sendDownMessage && delta.y < 0)
						{
							MessageTarget.SendMessage ("OnSwipeDown", SendMessageOptions.DontRequireReceiver);
						}
					}
					handled = true;
				}
				// Tap detected if finger released without swipe being detected
				else if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
				{
					SwipeID = -1;
					currentRotation = gameObject.transform.eulerAngles;

					player.jump = true;
					handled = true;
				}
			}
		}
		return handled;
	}

	void OnSwipeDown()
	{
		if (rotDone == true)
		{
			rotationDirection = 1; //anti-clock
			rotDone = false;
			rotateObject();
		}
	}

	void OnSwipeUp()
	{
		if (rotDone == true)
		{
			rotationDirection = -1; //clock
			rotDone = false;
			rotateObject();
		}
	}
	
	private void rotateObject()
	{
		currentRotation = gameObject.transform.eulerAngles;
		targetRotation.z = (currentRotation.z + (90 * rotationDirection));
		StartCoroutine (objectRotationAnimation());
	}

	IEnumerator objectRotationAnimation()
	{
		// add rotation step to current rotation.
		currentRotation.z += (rotationStep * rotationDirection * 0.2f);
		gameObject.transform.eulerAngles = currentRotation;
		
		yield return new WaitForSeconds (0);
		
		if (((int)(currentRotation.z) >
		     (int)targetRotation.z && rotationDirection < 0)  ||  // for clockwise
		    ((int)(currentRotation.z) <  (int)targetRotation.z && rotationDirection > 0)) // for anti-clockwise
		{
			StartCoroutine (objectRotationAnimation());
		}
		else
		{
			rotDone = true;

			currentRotation = gameObject.transform.eulerAngles;
			
			if (currentRotation.z > 89 && currentRotation.z < 91)
			{
				player.speedX = 0;
				player.speedY = 1;
				player.targetUp = new Vector3 (-1, 0, 0);
				//player.transform.forward = new Vector3(0f, 0f, 1f);
				player.gravityY = 0;
				player.gravityX = 25;
			}
			else if (currentRotation.z > 179 && currentRotation.z < 181)
			{
				player.speedX = -1;
				player.speedY = 0;
				player.targetUp = new Vector3 (0, -0.001f, 0); //not too sure why this has to be like this, but putting it as 1 makes the sprite face the opposite direction of motion... ???
				//player.transform.forward = new Vector3(-1f, 0f, 0f);
				player.gravityY = 25;
				player.gravityX = 0;
			}
			else if (currentRotation.z > 269 && currentRotation.z < 271)
			{
				player.speedX = 0;
				player.speedY = -1;
				player.targetUp = new Vector3 (1, 0, 0);
				//player.transform.forward = new Vector3(0f, 0f, -1f);
				player.gravityY = 0;
				player.gravityX = -25;
			}
			else if (currentRotation.z > -1 && currentRotation.z < 1)
			{
				player.speedX = 1;
				player.speedY = 0;
				player.targetUp = new Vector3 (0, 1, 0);
				//	player.transform.forward = new Vector3(-1f, 0f, 0f);
				player.gravityY = -25;
				player.gravityX = 0;
			}
		}
	}

}
