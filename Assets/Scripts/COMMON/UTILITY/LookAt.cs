using UnityEngine;
using System.Collections;

[AddComponentMenu("Utility/Look At")]

public class LookAt : ExtendedCustomMonoBehaviour {

	[Header("Settings")]
	[SerializeField]
	private Transform followTarget;
	[SerializeField]
	private Vector3 targetOffset = Vector3.zero;
	[SerializeField]
	private float moveSpeed= 0f;

	// main event
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

	// main logic
	public override void Init ()
	{
		// base init
		base.Init ();

		canControl = true;
	}

	public Transform FollowTarget {
		get { return followTarget; }
		set { followTarget = value; }
	}

	public Vector3 TargetOffset {
		get { return targetOffset; }
		set { targetOffset = value; }
	}

	public float MoveSpeed {
		get { return moveSpeed; }
		set { moveSpeed = value; }
	}
}
