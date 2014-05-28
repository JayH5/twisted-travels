using UnityEngine;
using System.Collections;

public class BoxGravityDirectionScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

		//print (transform.parent.name [0]);

		if (transform.parent.name[0] != 'S' && transform.parent.name[0] != 'M' && transform.parent.name[0] != 'L')
		{
			if (transform.parent.transform.parent.gameObject.transform.rotation.z > 0.9) //180 degrees
				rigidbody2D.gravityScale = -8;
			else 
				rigidbody2D.gravityScale = 8;
		}
		else
		{
			if (transform.parent.gameObject.transform.rotation.z > 0.9) //180 degrees
				rigidbody2D.gravityScale = -8;
			else 
				rigidbody2D.gravityScale = 8;
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
