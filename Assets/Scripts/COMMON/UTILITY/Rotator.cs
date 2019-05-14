using UnityEngine;
using System.Collections;

public class Rotator : ExtendedCustomMonoBehaviour {

	public Vector3 direction = Vector3.forward;
	public float speed = 5f;

	// main logic

	public override void Init() {
		// init base
		base.Init ();

		// rotate object
		UpdateVelocity ();
	}

	public void UpdateVelocity() {
		//calculate rotate
		myBody.angularVelocity = direction * speed;
	}
}
