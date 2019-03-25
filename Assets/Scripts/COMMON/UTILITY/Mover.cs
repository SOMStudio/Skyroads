using UnityEngine;
using System.Collections;

public class Mover : ExtendedCustomMonoBehaviour {
	public float speed;

	void Start () {
		// chack for init
		if (!didInit) {
			Init ();
		}

		//calculate move
		UpdateVelocity ();
	}

	public override void Init ()
	{
		base.Init ();
	}

	public void UpdateVelocity() {
		myBody.velocity = Vector3.forward * speed;
	}
}
