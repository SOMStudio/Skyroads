using UnityEngine;
using System.Collections;

[AddComponentMenu("Utility/Auto spin object")]

public sealed class AutoSpinObject : ExtendedCustomMonoBehaviour
{	
	[Header("Settings")]
	[SerializeField]
	private Vector3 spinVector = new Vector3(1,0,0);

	// main events

	void LateUpdate () {
		if (canControl) {
			myTransform.Rotate (spinVector * Time.deltaTime);
		}
	}

	// main logic

	public override void Init ()
	{
		// init base
		base.Init ();

		canControl = true;
	}

	public Vector3 SpinVector {
		get { return spinVector; }
		set { spinVector = value; }
	}
}
