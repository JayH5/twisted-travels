using UnityEngine;
using System.Collections;

public class PlatformCollisionScript : MonoBehaviour {

	void OnCollisionEnter2D (Collision2D other)
	{

		//Destroy (gameObject.transform.parent.gameObject);
		//Spawn ();
		if (other.gameObject.tag != "Player")
			print ("COLLISION PLATFORM!");
	}
}
