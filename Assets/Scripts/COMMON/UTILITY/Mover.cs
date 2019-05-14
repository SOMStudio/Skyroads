using UnityEngine;
using System.Collections;

public class Mover : ExtendedCustomMonoBehaviour {

	public Vector3 direction = Vector3.forward;
	public float speed = 5f;

	// main logic

	public override void Init () {
		// init base
		base.Init ();

		// move object
		UpdateVelocity ();
	}

	public void UpdateVelocity() {
		myBody.velocity = direction * speed;
	}
}
