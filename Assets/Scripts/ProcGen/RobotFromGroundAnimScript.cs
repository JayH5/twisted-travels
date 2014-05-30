using UnityEngine;
using System.Collections;

public class RobotFromGroundAnimScript : MonoBehaviour {

	Animator anim;
	SpriteRenderer spr;
	PlatformCleanupScript pcs;
	Transform daddy;
	
	public GameObject robot;
	
	private float timer;
	private bool started = false;
	private bool spawned = false;
	
	void Start () {
		anim = GetComponent<Animator>();
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
	
	void OnTriggerEnter2D(Collider2D other) 
	{
		if (other.tag == "Player")
		{
			if (spawned == false)
				spr.enabled = true;
			
			started = true;
			anim.enabled = true;

		}
	}
	
	void Update ()
	{
		if (started)
		{
			//print ("Update");
			timer += 1.0F * Time.deltaTime;
		}
		if (timer >= 0.9f && spawned == false)
		{
			//make sure the box spawn at the right rotation

			if (daddy.gameObject.transform.rotation.z == 0) //normal
			{
				print ("4");
				GameObject childRob = (GameObject)Instantiate (robot, new Vector3(transform.position.x, transform.position.y + 0.75f, transform.position.z) , Quaternion.identity);
				childRob.transform.parent = transform;
			}
			else if (daddy.gameObject.transform.rotation.z > 0.9) //upside down
			{
				print ("3");
				GameObject childRob = (GameObject)Instantiate (robot, new Vector3(transform.position.x, transform.position.y - 0.75f, transform.position.z) , Quaternion.AngleAxis(180,new Vector3(0,0,1)));
				childRob.transform.parent = transform;
			}
			else if(pcs.upDown == 1) //up 
			{	
				print ("1");
				GameObject childRob = (GameObject)Instantiate (robot, new Vector3(transform.position.x - 0.75f, transform.position.y, transform.position.z) , Quaternion.AngleAxis(90,new Vector3(0,0,1)));
				childRob.transform.parent = transform;
			}	
			else if(pcs.upDown == 0) //down
			{
				print ("2");
				GameObject childRob = (GameObject)Instantiate (robot, new Vector3(transform.position.x + 0.75f, transform.position.y, transform.position.z) , Quaternion.AngleAxis(270,new Vector3(0,0,1)));
				childRob.transform.parent = transform;
			}
			
			spawned = true;
			spr.enabled = false;
		}
	}
}
