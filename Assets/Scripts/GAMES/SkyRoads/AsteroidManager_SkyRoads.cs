using UnityEngine;
using System.Collections;

public class AsteroidManager_SkyRoads : MonoBehaviour {

	[SerializeField]
	private bool speedBoost = false;

	[Header("Settings")]
	public int multiplierSpeed = 2;

	[Header("Speceffects")]
	public GameObject Explosion;
	public GameObject ExplosionPlayer;

	[Header("Managers")]
	public Mover moveManager;
	public Rotator rotateManager;
	public GameController_SkyRoads gameController;

	void Start () {
		Init ();
	}

	void LateUpdate() {
		// update speed
		if ((moveManager.speed * multiplierSpeed) != gameController.globalSpeedGame) {
			moveManager.speed = gameController.globalSpeedGame * multiplierSpeed;
			moveManager.UpdateVelocity ();
		}

		// update speed boost
		bool speedBoostGC = gameController.IsBoostSpeed ();
		if (speedBoostGC) {
			if (!speedBoost) {
				moveManager.speed *= gameController.multForBoostSpeed;
				moveManager.UpdateVelocity ();
				speedBoost = true;
			}
		} else {
			if (speedBoost) {
				moveManager.speed /= gameController.multForBoostSpeed;
				moveManager.UpdateVelocity ();
				speedBoost = false;
			}
		}
	}

	void Init() {
		if (!gameController) {
			gameController = GameController_SkyRoads.Instance;

			if (!gameController) {
				Debug.Log ("Cannot find GameController!!");
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

		gameController.AddScore (gameController.countScoreForAsteroid);
	}
}
