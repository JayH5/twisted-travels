using UnityEngine;
using System.Collections;

public class RocketScript : MonoBehaviour {

	public GameObject explosion;
	private GameObject childExplosion;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnCollisionEnter2D(Collision2D other) {
		childExplosion = (GameObject)Instantiate(explosion, new Vector3(transform.position.x, transform.position.y, transform.position.z) , Quaternion.identity);
		DestroyObject (gameObject);
	}
}
