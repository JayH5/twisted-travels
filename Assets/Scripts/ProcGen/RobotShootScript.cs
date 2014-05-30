using UnityEngine;
using System.Collections;

public class RobotShootScript : MonoBehaviour {

	protected Animator animator;
	public GameObject rocket;

	SpriteRenderer spr;
	PlatformCleanupScript pcs;
	Transform daddy;

	private GameObject childRocket = null;
	private float timer;
	private bool startTimer = false;
	
	void Start () 
	{
		animator = GetComponent<Animator>();

		spr = GetComponent<SpriteRenderer> ();
		
		//get the platform as daddy
		if (transform.parent != null)
		{
			if (transform.parent.name[0] != 'S' && transform.parent.name[0] != 'M' && transform.parent.name[0] != 'L')
				daddy = transform.parent.transform.parent;
			else 
				daddy = transform.parent;
		}
		else daddy = transform;
		
		//set daddy as a platformScleanupScript
		pcs = daddy.GetComponent<PlatformCleanupScript> ();
	}
	
	void Update () 
	{
		if(animator)
		{
			//get the current state
			AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
			
			//if we're in "Run" mode, respond to input for jump, and set the Jump parameter accordingly. 
			if(stateInfo.nameHash == Animator.StringToHash("Base Layer.RobotShoot"))
			{
				if (startTimer == false)
				{
					//make sure the box spawn at the right rotation
					if (daddy.gameObject.transform.rotation.z == 0) //normal
					{
						childRocket = (GameObject)Instantiate (rocket, new Vector3(transform.position.x - 1f, transform.position.y + 0.18f, transform.position.z) , Quaternion.AngleAxis(180,new Vector3(0,0,1)));
						childRocket.transform.parent = transform;
						startTimer = true;
					}
					else if (daddy.gameObject.transform.rotation.z > 0.9) //upside down
					{	
						childRocket = (GameObject)Instantiate (rocket, new Vector3(transform.position.x + 1f, transform.position.y - 0.18f, transform.position.z) , Quaternion.identity);
						childRocket.transform.parent = transform;
						startTimer = true;
					}
					else if(pcs.upDown == 1) //up 
					{	
						childRocket = (GameObject)Instantiate (rocket, new Vector3(transform.position.x  - 0.18f, transform.position.y  - 1f, transform.position.z) , Quaternion.AngleAxis(270,new Vector3(0,0,1)));
						childRocket.transform.parent = transform;
						startTimer = true;
					}	
					else if(pcs.upDown == 0) //down
					{
						childRocket = (GameObject)Instantiate (rocket, new Vector3(transform.position.x + 0.18f, transform.position.y + 1f, transform.position.z) , Quaternion.AngleAxis(90,new Vector3(0,0,1)));
						childRocket.transform.parent = transform;
						startTimer = true;
					}
				}
			}
			/*else
			{
				animator.SetBool("Jump", false);                
			}
			
			float h = Input.GetAxis("Horizontal");
			float v = Input.GetAxis("Vertical");
			
			//set event parameters based on user input
			animator.SetFloat("Speed", h*h+v*v);
			animator.SetFloat("Direction", h, DirectionDampTime, Time.deltaTime);*/
		}
		if ( startTimer)
			timer += 1.0F * Time.deltaTime;
		if (timer >= 2f)
		{
			startTimer = false;
			timer = 0;
		}

		if (childRocket != null)
		{
			if(pcs.upDown == 1) //up 
			{	
				childRocket.rigidbody2D.AddForce(new Vector2(0,-20f));
			}	
			else if(pcs.upDown == 0) //down
			{
				childRocket.rigidbody2D.AddForce(new Vector2(0,20f));
			}
			else if (daddy.gameObject.transform.rotation.z > 0.9) //upside down
			{	
				childRocket.rigidbody2D.AddForce(new Vector2(20f,0));
			}
			else //normal
			{
				childRocket.rigidbody2D.AddForce(new Vector2(-20f,0));
			}

		}
	} 
}
