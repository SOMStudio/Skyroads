using UnityEngine;
using System.Collections;

[AddComponentMenu("Utility/Fallow Target")]

public class FallowTarget : ExtendedCustomMonoBehaviour {
	
	[Header("Settings")]
	[SerializeField]
	private Transform followTarget;
	[SerializeField]
	private Vector3 targetOffset = Vector3.zero;
	[SerializeField]
	private float moveSpeed= 2f;

	// main event
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
