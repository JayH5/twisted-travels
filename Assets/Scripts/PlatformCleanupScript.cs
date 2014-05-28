using UnityEngine;
using System.Collections;

public class PlatformCleanupScript: MonoBehaviour {

	private float timer;
	private bool playerTouch = false;

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
