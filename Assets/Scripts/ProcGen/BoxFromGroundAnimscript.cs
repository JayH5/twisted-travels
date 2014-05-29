using UnityEngine;
using System.Collections;

public class BoxFromGroundAnimscript : MonoBehaviour {

	Animator anim;
	SpriteRenderer spr;

	public GameObject box;

	private float timer;
	private bool started = false;
	private bool spawned = false;

	void Start () {
		anim = GetComponent<Animator>();
		spr = GetComponent<SpriteRenderer> ();
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
		if (timer >= 0.7f && spawned == false)
		{
			GameObject childBox = (GameObject)Instantiate (box, new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z) , Quaternion.identity);
			childBox.transform.parent = transform;
			spawned = true;
			spr.enabled = false;
		}
	}
}
