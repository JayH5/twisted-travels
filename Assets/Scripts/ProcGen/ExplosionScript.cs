using UnityEngine;
using System.Collections;

public class ExplosionScript : MonoBehaviour {

	private float timer;
	// Update is called once per frame
	void Update () {
		timer += 1.0f * Time.deltaTime;
		
		if (timer > 1.3f)
			DestroyObject (gameObject);
	}
}
