using UnityEngine;
using UnityEngine.Events;

public class DestroyByBoundery : MonoBehaviour {

	public UnityEvent eventBoundery;

	void OnTriggerExit(Collider other) {
		if (eventBoundery != null) {
			eventBoundery.Invoke ();
		}

		Destroy(other.gameObject);
	}
}
