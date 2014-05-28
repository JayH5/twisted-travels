using UnityEngine;
using System.Collections;

public class BoxGravityDirectionScript : MonoBehaviour {

	private bool toRight = false;
	private bool toLeft = false;

	// Use this for initialization
	void Start () {

		Transform daddy;

		//get the platform as daddy
		if (transform.parent.name[0] != 'S' && transform.parent.name[0] != 'M' && transform.parent.name[0] != 'L')
			daddy = transform.parent.transform.parent;
		else 
			daddy = transform.parent;

		//set daddy as a platformScleanupScript
		PlatformCleanupScript pcs = daddy.GetComponent<PlatformCleanupScript> ();
	
		//find platform orientation
		if (daddy.gameObject.transform.rotation.z > 0.9) //upside down
		{
			rigidbody2D.gravityScale = -8;
			print ("Pushing UP");
		}
		else if (daddy.gameObject.transform.rotation.z == 0) //normal way up
		{
			rigidbody2D.gravityScale = 8;
			print ("Pushing DOWN");
		}
		else if(pcs.upDown == 1) //up 
		{	
			rigidbody2D.gravityScale = 0;
			toRight = true;
			print ("Pushing RIGHT");
		}	
		else if(pcs.upDown == 0) //down
		{
			rigidbody2D.gravityScale = 0;
			toLeft = true;
			print ("Pushing LEFT");
		}

	}

	
	// Update is called once per frame
	void Update () {

		if (toRight)
			rigidbody2D.AddForce(new Vector2(9.8f*100,0f));

		if (toLeft)
			rigidbody2D.AddForce(new Vector2(-9.8f*100,0f));
	
	}
}
