using UnityEngine;
using System.Collections;

public class BasicTrackingCamera : MonoBehaviour {

	public Transform target;

	float offsetZ;
	
	Quaternion shiftRotation = Quaternion.identity;
	public float shiftDuration = 0.3f; // Seconds

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
		transform.rotation = target.rotation * shiftRotation;
	}

	public void shift(RotationDirection direction)
	{
		StartCoroutine(shiftAnimation(direction));
	}

	IEnumerator shiftAnimation(RotationDirection direction)
	{
		Quaternion from = Quaternion.identity;
		Quaternion to = Quaternion.AngleAxis((int) direction / 5, Vector3.forward); // Shift 18 deg
		for (float i = 0.0f; i < 1.0f; i += Time.deltaTime / shiftDuration)
		{
			shiftRotation = Quaternion.Slerp(from, to, Mathf.Sin(i * Mathf.PI));
			yield return new WaitForSeconds(0);
		}
		shiftRotation = Quaternion.identity;
	}
}
