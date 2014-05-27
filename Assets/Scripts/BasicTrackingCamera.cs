using UnityEngine;
using System.Collections;

public class BasicTrackingCamera : MonoBehaviour {

	public Transform target;

	float offsetZ;

	// Use this for initialization
	void Start ()
	{
		offsetZ = (transform.position - target.position).z;
		transform.parent = null;
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position = new Vector3(target.position.x, target.position.y, offsetZ);
		transform.rotation = target.rotation;
	}
}
