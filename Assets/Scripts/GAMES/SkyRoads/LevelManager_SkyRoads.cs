using UnityEngine;
using System.Collections.Generic;

public class LevelManager_SkyRoads : MonoBehaviour {

	[Header("Asteroids")]
	[SerializeField]
	private GameObject spawn_1;
	[SerializeField]
	private GameObject spawn_2;
	[SerializeField]
	private GameObject spawn_3;
	[SerializeField]
	private Transform spawnPosition;

	private List<AsteroidManager_SkyRoads> asteroidList = new List<AsteroidManager_SkyRoads>();
	private Vector3 prevPosSpawn = Vector3.zero;

	[Header("Road")]
	[SerializeField]
	private RoadManager_SkyRoads roadManager;

	[Header("Smooth Follow")]
	[SerializeField]
	private FallowTarget smoothFollow;
	[SerializeField]
	private float increaseDistance = 5;
	[SerializeField]
	private float increaseHeigh = 2;

	[Header("Boost Speed Environment")]
	[SerializeField]
	private GameObject boostSpeedParticles;

	[Header("Boost Speed Player")]
	[SerializeField]
	private bool speedBoost = false;
	[SerializeField]
	private GameObject boostSpeedTrailLeft;
	[SerializeField]
	private GameObject boostSpeedTrailRight;

	[Header("Game Controller Ref")]
	[SerializeField]
	private GameController_SkyRoads gameController;

	[System.NonSerialized]
	public static LevelManager_SkyRoads Instance;

	// main event
	void Awake () {
		// activate instance
		if (Instance == null) {
			Instance = this;

			if (!gameController)
				gameController = GameController_SkyRoads.Instance;
		} else if (Instance != this) {
			Destroy (gameObject);
		}
	}

	void Start() {
		// keep this object alive
		DontDestroyOnLoad (this.gameObject);
	}

	// main logic
	public void StartLevel() {
		prevPosSpawn = Vector3.zero;

		// clear for restart
		DestroyWave ();
	}

	public void GameOver(AsteroidManager_SkyRoads val) {
		//remove from list
		asteroidList.Remove (val);

		// game over
		gameController.GameOver ();
	}

	public void AddPointsForAsteroid(AsteroidManager_SkyRoads val) {
		//remove from list
		asteroidList.Remove (val);

		// add bonuses
		gameController.AddPointsForAsteroid ();
	}

	// Spawn part
	public void SpawnWave() {
		Vector3 startPosition = GetPossitio();
		Quaternion startQuaternion = Quaternion.identity;
		GameObject newAsteroid = null;

		int asteroidNumber = Random.Range(1, 3);
		if (asteroidNumber == 1)
		{
			newAsteroid = Instantiate(spawn_1, startPosition, startQuaternion);
		}
		else if (asteroidNumber == 2)
		{
			newAsteroid = Instantiate(spawn_2, startPosition, startQuaternion);
		}
		else
		{
			newAsteroid = Instantiate(spawn_3, startPosition, startQuaternion);
		}

		// add in list and init
		var asteroidManager = newAsteroid.GetComponent<AsteroidManager_SkyRoads>();
		if (asteroidManager) {
			asteroidList.Add (asteroidManager);

			// set current speed
			float speedLevel = gameController.GlobalSpeedGame * (speedBoost ? gameController.MultForBoostSpeed : 1);
			asteroidManager.SetSpeed (speedLevel);
		}
	}

	Vector3 GetPossitio() {
		float xPos = Random.Range (spawnPosition.position.x - 3.0f, spawnPosition.position.x + 3.0f);

		Vector3 resPos = new Vector3(xPos, spawnPosition.position.y, spawnPosition.position.z);
		bool findProblem = false;

		float distance = Vector3.Distance(resPos, prevPosSpawn);
		float minDistanceBetweenObject = gameController.MinDistanceBetweenObject;
		if (distance <= minDistanceBetweenObject)
		{
			findProblem = true;
		}

		if (findProblem) 
		{
			return GetPossitio();
		}
		else
		{
			prevPosSpawn = resPos;

			return resPos;
		}
	}

	void DestroyWave() {
		foreach (var item in asteroidList) {
			Destroy (item.gameObject);
		}

		asteroidList.Clear ();
	}

	// Smooth Fallow
	public void ChangeSpeedGame() {
		float speedLevel = gameController.GlobalSpeedGame * (speedBoost ? gameController.MultForBoostSpeed : 1);

		// Asteroids
		foreach (var item in asteroidList) {
			item.SetSpeed (speedLevel);
		}

		// road
		roadManager.SetSpeed (speedLevel);
	}

	public bool IsSpeedBoost() {
		return speedBoost;
	}

	public void SpeedBoost() {
		if (!speedBoost) {
			speedBoost = true;

			//smooth Follow
			smoothFollow.TargetOffset += new Vector3(0, increaseHeigh, -increaseDistance);

			//boost Envarinment Particles
			boostSpeedParticles.SetActive (true);

			//boost Player Particles
			boostSpeedTrailLeft.SetActive (true);
			boostSpeedTrailRight.SetActive (true);

			//change spped game
			ChangeSpeedGame ();

			Invoke ("SpeedReduce", 3);
		} else {
			if (IsInvoking ("SpeedReduce")) {
				CancelInvoke ("SpeedReduce");
				Invoke ("SpeedReduce", 3);
			}
		}
	}

	private void SpeedReduce() {
		speedBoost = false;

		//smooth Follow
		smoothFollow.TargetOffset += new Vector3(0, -increaseHeigh, increaseDistance);

		//boost Envarinment Particles
		boostSpeedParticles.SetActive (false);

		//boost Player Particles
		boostSpeedTrailLeft.SetActive (false);
		boostSpeedTrailRight.SetActive (false);

		//change spped game
		ChangeSpeedGame ();
	}
}
