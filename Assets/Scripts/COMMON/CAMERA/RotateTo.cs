using UnityEngine;
using System.Collections;

public class RotateTo : MonoBehaviour {
	public Transform followTarget;
	public Vector3 targetOffset;
	public float moveSpeed= 2f;

	private Transform myTransform;

	void Start ()
	{
		myTransform= transform;
	}

	public void SetTarget( Transform aTransform )
	{
		followTarget= aTransform;
	}

	void Update ()
	{
		if (followTarget) {
			Vector3 targetVector3 = followTarget.eulerAngles;

			if (myTransform.eulerAngles.y > 180)
				targetVector3.y = 360;
			if (myTransform.eulerAngles.z > 180)
				targetVector3.z = 360;
		
			if ((myTransform.eulerAngles - (targetVector3 + targetOffset)).magnitude > 0.1f) {
				if (moveSpeed == 0) {
					transform.LookAt (followTarget.position + targetOffset);
				} else {
					myTransform.eulerAngles = Vector3.Lerp (myTransform.eulerAngles, targetVector3 + targetOffset, moveSpeed * Time.deltaTime);
				}
			}
		}
	}
}
