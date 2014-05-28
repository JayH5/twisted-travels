using UnityEngine;
using System.Collections;

public class PlatformCleanupScript: MonoBehaviour {

	private float timer;
	private bool playerTouch = false;

	void Start()
	{
		/*foreach (Transform child in transform)
		{
			if (gameObject.transform.rotation.z > 0 && child.gameObject.name == "Box")
				child.rigidbody2D.gravityScale = -8;
			else if (child.gameObject.name == "Box")
				child.rigidbody2D.gravityScale = 8;
		}*/

		print (transform.rotation.z);
	}

	void Update ()
	{
		if (playerTouch)
			timer += 1.0F * Time.deltaTime;

		if (timer >= 20)
		{
			GameObject.Destroy(gameObject);
		}
	}

	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.tag == "Player")
			playerTouch = true;
	}
}
