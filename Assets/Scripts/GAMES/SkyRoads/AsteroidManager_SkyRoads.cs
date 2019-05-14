using UnityEngine;

public class AsteroidManager_SkyRoads : MonoBehaviour {

	[SerializeField]
	private bool speedBoost = false;

	[Header("Settings")]
	[SerializeField]
	private int multiplierSpeed = 2;

	[Header("Speceffects")]
	[SerializeField]
	private GameObject Explosion;
	[SerializeField]
	private GameObject ExplosionPlayer;

	[Header("Managers")]
	[SerializeField]
	private Mover moveManager;
	[SerializeField]
	private Rotator rotateManager;
	[SerializeField]
	private GameController_SkyRoads gameController;

	// main event
	void Start () {
		Init ();
	}

	void LateUpdate() {
		// update speed
		int globalSpeedGame = gameController.GlobalSpeedGame;
		if ((moveManager.speed * multiplierSpeed) != globalSpeedGame) {
			moveManager.speed = globalSpeedGame * multiplierSpeed;
			moveManager.UpdateVelocity ();
		}

		// update speed boost
		bool speedBoostGC = gameController.IsBoostSpeed ();
		float multForBoostSpeed = gameController.MultForBoostSpeed;
		if (speedBoostGC) {
			if (!speedBoost) {
				moveManager.speed *= multForBoostSpeed;
				moveManager.UpdateVelocity ();
				speedBoost = true;
			}
		} else {
			if (speedBoost) {
				moveManager.speed /= multForBoostSpeed;
				moveManager.UpdateVelocity ();
				speedBoost = false;
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Floor")
		{
			return;
		};

		if (other.tag == "Player")
		{
			//Instantiate(ExplosionPlayer, other.transform.position, other.transform.rotation);
			gameController.GameOver();
		};

		//Instantiate(Explosion, transform.position, transform.rotation);
		//Destroy(other.gameObject);
		Destroy(gameObject);

		gameController.AddScore (gameController.CountScoreForAsteroid);
	}

	// main logic
	void Init() {
		if (!gameController) {
			gameController = GameController_SkyRoads.Instance;

			if (!gameController) {
				Debug.Log ("Cannot find GameController!!");
			}
		}
	}
}
