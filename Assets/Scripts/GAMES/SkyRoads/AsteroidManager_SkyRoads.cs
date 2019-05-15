using UnityEngine;

public class AsteroidManager_SkyRoads : MonoBehaviour {

	[Header("Settings")]
	[SerializeField]
	private float multiplierSpeed = 1f;

	[Header("Speceffects")]
	[SerializeField]
	private GameObject Explosion;
	[SerializeField]
	private GameObject ExplosionPlayer;

	[Header("Managers")]
	[SerializeField]
	private AutoMoveRigidBody moveManager;
	[SerializeField]
	private AutoRotateRigidBody rotateManager;
	[SerializeField]
	private LevelManager_SkyRoads levelManager;

	// main event
	void Start () {
		Init ();
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			levelManager.GameOver (this);
		} else {
			return;
		}

		//Instantiate(Explosion, transform.position, transform.rotation);
		Destroy(gameObject);
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Floor") {
			levelManager.AddPointsForAsteroid (this);
		} else {
			return;
		}

		Destroy(gameObject);
	}

	// main logic
	void Init() {
		if (!levelManager) {
			levelManager = LevelManager_SkyRoads.Instance;

			if (!levelManager) {
				Debug.Log ("Cannot find LevelManager!!");
			}
		}
	}

	public void SetSpeed(float val) {
		moveManager.Speed = val * multiplierSpeed;
	}
}
