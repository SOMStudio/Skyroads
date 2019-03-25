using UnityEngine;
using System.Collections;

public class BaseWindowControll : MonoBehaviour {

	public GameObject gameObj;
	public CanvasGroup canvasGrope;

	void Awake () {
		if (!gameObject)
			gameObj = GetComponent<GameObject> ();
		if (!canvasGrope)
			canvasGrope = GetComponent<CanvasGroup> ();
	}

	public void SetActive(bool val) {
		if (gameObject)
			gameObject.SetActive (val);
	}

	public bool IsActive() {
		return gameObject.activeSelf;
	}

	public void SetInteractable(bool val) {
		if (canvasGrope)
			canvasGrope.interactable = val;
	}

	public bool IsInteractable() {
		return canvasGrope.interactable;
	}
}
