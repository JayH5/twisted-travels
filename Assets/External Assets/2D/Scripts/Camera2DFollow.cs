using UnityEngine;
using System.Collections;

public class Camera2DFollow : MonoBehaviour {
	
	public Transform target;
	public float positionDamping = 1;
	public float lookAheadFactor = 3;
	public float lookAheadReturnSpeed = 0.5f;
	public float lookAheadMoveThreshold = 0.1f;

	public float rotationDamping = 0.5f;
	public float turnAheadFactor = 3;
	public float turnAheadReturnSpeed = 10.0f;
	public float turnAheadMoveThreshold = 0.1f;
	
	float offsetZ;

	Vector3 lastTargetPosition;
	Vector3 currentVelocity;
	Vector3 lookAheadPos;

	Vector3 lastTargetUp;
	Vector3 currentAngularVelocity;
	Vector3 turnAheadUp;
	
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
			lookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(moveDelta);
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
		float rotDelta = Vector3.Angle(target.up, lastTargetUp);

		bool updateTurnAheadTarget = Mathf.Abs(rotDelta) > turnAheadMoveThreshold;

		if (updateTurnAheadTarget)
		{
			turnAheadUp = turnAheadFactor * Vector3.up * Mathf.Sign(rotDelta);
		}
		else
		{
			turnAheadUp = Vector3.MoveTowards(turnAheadUp, Vector3.zero, Time.deltaTime * turnAheadReturnSpeed);
		}

		Vector3 aheadTargetUp = target.up + turnAheadUp;
		//Debug.Log ("Target up: " + target.up);
		//Debug.Log ("aheadTargetUp: " + aheadTargetUp);
		Vector3 newRot = Vector3.SmoothDamp(transform.up, aheadTargetUp, ref currentAngularVelocity, rotationDamping);

		transform.up = newRot;

		lastTargetUp = target.up;
	}
}
