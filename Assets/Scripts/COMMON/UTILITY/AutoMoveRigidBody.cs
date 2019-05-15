using UnityEngine;
using System.Collections;

[AddComponentMenu("Utility/Auto move rigit body")]

public sealed class AutoMoveRigidBody : ExtendedCustomMonoBehaviour
{
	[Header("Settings")]
	[SerializeField]
	private Vector3 direction = Vector3.forward;
	[SerializeField]
	private float speed = 1f;

	// main logic
	public override void Init () {
		// init base
		base.Init ();

		canControl = true;

		// move object
		UpdateVelocity ();
	}

	public void UpdateVelocity() {
		if (canControl) {
			myBody.velocity = direction * speed;
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
