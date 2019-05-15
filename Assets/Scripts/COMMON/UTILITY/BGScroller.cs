using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Utility/Back ground scroller")]

public class BGScroller : ExtendedCustomMonoBehaviour
{
	[Header("Settings")]
	[SerializeField]
	private Vector3 direction = Vector3.back;
	[SerializeField]
	private float scrollSpeed;
	[SerializeField]
	private float tileSizeZ;

	private Vector3 startPosition;

	// main event
	void Update () {
		if (canControl) {
			float newPosition = Mathf.Repeat (Time.time * scrollSpeed, tileSizeZ);
			transform.position = startPosition + direction * newPosition;
		}
	}

	// main logic
	public override void Init ()
	{
		// base init
		base.Init ();

		canControl = true;

		// save start possition
		startPosition = transform.position;
	}

	public float ScrollSpeed {
		get { return scrollSpeed; }
		set { scrollSpeed = value; }
	}
}
