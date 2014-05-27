using UnityEngine;
using System.Collections;

public class BoxTriggerScript : MonoBehaviour {

	
	void OnTriggerEnter2D(Collider2D other) 
	{
		print ("YES!!!");
		if (other.tag == "Player")
		{
			print(transform.parent.gameObject.transform.rotation.z);
			foreach (Transform child in transform)
			{
			if (transform.parent.gameObject.transform.rotation.z > 0)
			{
				print ("WIN!");
				child.rigidbody2D.gravityScale = -10;
			}
			else 
				child.rigidbody2D.gravityScale = 10;
			}
		}

			
	}
}
