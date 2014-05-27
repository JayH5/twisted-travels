using UnityEngine;
using System.Collections;

public class Camera2DFollow : MonoBehaviour {
	
	public Transform target;
	public float positionDamping = 1;
	public float lookAheadFactor = 3;
	public float lookAheadReturnSpeed = 0.5f;
	public float lookAheadMoveThreshold = 0.1f;

	public float rotationDamping = 0.5f;
	
	float offsetZ;

	Vector3 lastTargetPosition;
	Vector3 currentVelocity;
	Vector3 lookAheadPos;

	Vector3 currentAngularVelocity;
	
	// Use this for initialization
	void Start () {
		lastTargetPosition = target.position;
		offsetZ = (transform.position - target.position).z;
		transform.parent = null;
	}
	
	// Update is called once per frame
	void Update () {
		
		// only update lookahead pos if accelerating or changed direction
		Vector3 movement = target.position - lastTargetPosition;
		float moveDelta = Mathf.Abs(movement.x) > Mathf.Abs(movement.y) ? movement.x : movement.y;

	    bool updateLookAheadTarget = Mathf.Abs(moveDelta) > lookAheadMoveThreshold;

		if (updateLookAheadTarget)
		{
			lookAheadPos = lookAheadFactor * transform.right.normalized;
		}
		else
		{
			lookAheadPos = Vector3.MoveTowards(lookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);	
		}
		
		Vector3 aheadTargetPos = target.position + lookAheadPos + Vector3.forward * offsetZ;
		Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref currentVelocity, positionDamping);
		
		transform.position = newPos;
		
		lastTargetPosition = target.position;

		// Now do the same for rotation...
		transform.up = Vector3.SmoothDamp(transform.up, target.up, ref currentAngularVelocity, rotationDamping);
	}
}
