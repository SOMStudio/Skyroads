using UnityEngine;
using System.Collections;

public class CatchEventObject : MonoBehaviour {
	[System.NonSerialized]
	public static CatchEventObject instance = null;

	public OnEventCatch OnMouseEnterEvent;
	public OnEventCatch OnMouseExitEvent;

	void Awake () {
		instance = this;
	}

	void OnMouseEnter() {
		if (OnMouseEnterEvent != null)
			OnMouseEnterEvent ();
	}

	void OnMouseExit() {
		if (OnMouseExitEvent != null)
			OnMouseExitEvent ();
	}
}
