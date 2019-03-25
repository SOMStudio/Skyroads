using UnityEngine;
using System.Collections;

public class PossitionInScreen : MonoBehaviour {

	private Camera activeCamera;
	private int savedWidth;
	private int savedHeight;

	private void SetPositionInScreen_WithDelay() {
		Invoke ("SetPositionInScreen", 0.05f);
	}

	private void StopInvoke() {
		CancelInvoke ("SetPositionInScreen");
	}

	public void SetPositionInScreen() {
		SetPosInScreen (new Vector3 (Screen.width - 1, Screen.height - 1, activeCamera.nearClipPlane + 5));
	}

	public void ChackPositionInScreen() {
		if (Screen.width != savedWidth || Screen.height != savedHeight) {
			SetPositionInScreen_WithDelay ();
		}
	}

	private void SetPosInScreen(Vector3 setPos) {
		Vector3 pos = activeCamera.ScreenToWorldPoint (setPos);

		transform.position = pos;

		savedWidth = Screen.width;
		savedHeight = Screen.height;
	}

	// Use this for initialization
	void Start () {
		activeCamera = Camera.main;

		SetPositionInScreen ();
	}

	void LateUpdate() {
		ChackPositionInScreen ();
	}
}
