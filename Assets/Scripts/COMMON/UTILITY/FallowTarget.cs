using UnityEngine;
using System.Collections;

public class FallowTarget : MonoBehaviour {
	
	public Transform followTarget;
	public Vector3 targetOffset;
	public float moveSpeed= 2f;

	private Transform myTransform;

	void Start ()
	{
		myTransform = transform;
	}

	public void SetTarget( Transform aTransform )
	{
		followTarget = aTransform;
	}

	void LateUpdate ()
	{
		if (followTarget) {
			if ((myTransform.position - (followTarget.position + targetOffset)).magnitude > 0.1f) {
				if (moveSpeed == 0) {
					myTransform.position = followTarget.position + targetOffset;
				} else {
					myTransform.position = Vector3.Lerp (myTransform.position, followTarget.position + targetOffset, moveSpeed * Time.deltaTime);
				}
			}
		}
	}
}
