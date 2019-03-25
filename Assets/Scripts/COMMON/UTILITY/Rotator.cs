using UnityEngine;
using System.Collections;

public class Rotator : ExtendedCustomMonoBehaviour {

	public float angleSpeedMin;
	public float angleSpeedMax;

	void Start () {
		// chack for init
		if (!didInit) {
			Init ();
		}

		//get random Ranges
		float anleSpeed = Random.Range (angleSpeedMin, angleSpeedMax);
		var velocityDirection = new Vector3(Random.Range(-1,1), Random.Range(-1,1), Random.Range(-1,1));

		//calculate rotate
		myBody.angularVelocity = velocityDirection * anleSpeed;
	}

	public override void Init ()
	{
		base.Init ();
	}
}
