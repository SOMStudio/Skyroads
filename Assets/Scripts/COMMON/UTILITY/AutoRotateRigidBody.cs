using UnityEngine;
using System.Collections;

[AddComponentMenu("Utility/Auto rotate rigit body")]

public sealed class AutoRotateRigidBody : ExtendedCustomMonoBehaviour
{
	[Header("Settings")]
	[SerializeField]
	private Vector3 direction = Vector3.forward;
	[SerializeField]
	private float speed = 1f;

	// main logic
	public override void Init() {
		// init base
		base.Init ();

		canControl = true;

		// rotate object
		UpdateVelocity ();
	}

	public void UpdateVelocity() {
		if (canControl) {
			myBody.angularVelocity = direction * speed;
		}
	}

	public Vector3 Direction {
		get { return direction; }
		set { direction = value; }
	}

	public float Speed {
		get { return speed; }
		set { speed = value; UpdateVelocity (); }
	}
}
