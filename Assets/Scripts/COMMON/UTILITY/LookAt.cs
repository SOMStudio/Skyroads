using UnityEngine;
using System.Collections;

[AddComponentMenu("Utility/Look At")]

public class LookAt : MonoBehaviour {
	public Transform followTarget;
	public Vector3 targetOffset;
	public float moveSpeed= 0f;

	private Transform myTransform;

	void Start ()
	{
		myTransform= transform;
	}

	public void SetTarget( Transform aTransform )
	{
		followTarget = aTransform;
	}

	void LateUpdate() {
		if (followTarget) {
			Quaternion turgRotate = Quaternion.LookRotation ((followTarget.position + targetOffset) - myTransform.position);
			Quaternion myRotate = myTransform.rotation;

			if ((myRotate.eulerAngles - turgRotate.eulerAngles).magnitude > 0.1f) {
				if (moveSpeed == 0) {
					myTransform.LookAt (followTarget.position + targetOffset);
				} else {
					myTransform.rotation = Quaternion.Slerp (myRotate, turgRotate, moveSpeed * Time.deltaTime);
				}
			}
		}
	}
}
