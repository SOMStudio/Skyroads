using UnityEngine;
using System.Collections.Generic;

public class LevelManager_SkyRoads : MonoBehaviour
{
	[Header("Asteroids")]
	[SerializeField] private GameObject spawn_1;
	[SerializeField] private GameObject spawn_2;
	[SerializeField] private GameObject spawn_3;
	[SerializeField] private Transform spawnPosition;

	private readonly List<AsteroidManager_SkyRoads> asteroidList = new List<AsteroidManager_SkyRoads>();
	private Vector3 prevPosSpawn = Vector3.zero;

	[Header("Road")]
	[SerializeField] private RoadManager_SkyRoads roadManager;

	[Header("Smooth Follow")]
	[SerializeField] private FallowTarget smoothFollow;

	[SerializeField] private float increaseDistance = 5;
	[SerializeField] private float increaseHeight = 2;

	[Header("Boost Speed Environment")]
	[SerializeField] private GameObject boostSpeedParticles;

	[Header("Boost Speed Player")]
	[SerializeField] private bool speedBoost;

	[SerializeField] private GameObject boostSpeedTrailLeft;
	[SerializeField] private GameObject boostSpeedTrailRight;

	[Header("Game Controller Ref")]
	[SerializeField] private GameController_SkyRoads gameController;

	[System.NonSerialized] public static LevelManager_SkyRoads Instance;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;

			if (!gameController)
				gameController = GameController_SkyRoads.Instance;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
	}

	private void Start()
	{
		DontDestroyOnLoad(this.gameObject);
	}
	
	public void StartLevel()
	{
		prevPosSpawn = Vector3.zero;
		
		DestroyWave();
	}

	public void GameOver(AsteroidManager_SkyRoads val)
	{
		asteroidList.Remove(val);
		
		gameController.GameOver();
	}

	public void AddPointsForAsteroid(AsteroidManager_SkyRoads val)
	{
		asteroidList.Remove(val);
		
		gameController.AddPointsForAsteroid();
	}
	
	public void SpawnWave()
	{
		Vector3 startPosition = GetPosition();
		Quaternion startQuaternion = Quaternion.identity;
		GameObject newAsteroid;

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
		
		var asteroidManager = newAsteroid.GetComponent<AsteroidManager_SkyRoads>();
		if (asteroidManager)
		{
			asteroidList.Add(asteroidManager);
			
			float speedLevel = gameController.GlobalSpeedGame * (speedBoost ? gameController.MultForBoostSpeed : 1);
			asteroidManager.SetSpeed(speedLevel);
		}
	}

	private Vector3 GetPosition()
	{
		float xPos = Random.Range(spawnPosition.position.x - 3.0f, spawnPosition.position.x + 3.0f);

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
			return GetPosition();
		}
		else
		{
			prevPosSpawn = resPos;

			return resPos;
		}
	}

	private void DestroyWave()
	{
		foreach (var item in asteroidList)
		{
			Destroy(item.gameObject);
		}

		asteroidList.Clear();
	}
	
	public void ChangeSpeedGame()
	{
		float speedLevel = gameController.GlobalSpeedGame * (speedBoost ? gameController.MultForBoostSpeed : 1);
		
		foreach (var item in asteroidList)
		{
			item.SetSpeed(speedLevel);
		}
		
		roadManager.SetSpeed(speedLevel);
	}

	public bool IsSpeedBoost()
	{
		return speedBoost;
	}

	public void SpeedBoost()
	{
		if (!speedBoost)
		{
			speedBoost = true;
			
			smoothFollow.TargetOffset += new Vector3(0, increaseHeight, -increaseDistance);
			
			boostSpeedParticles.SetActive(true);
			
			boostSpeedTrailLeft.SetActive(true);
			boostSpeedTrailRight.SetActive(true);
			
			ChangeSpeedGame();

			Invoke(nameof(SpeedReduce), 3);
		}
		else
		{
			if (IsInvoking(nameof(SpeedReduce)))
			{
				CancelInvoke(nameof(SpeedReduce));
				Invoke(nameof(SpeedReduce), 3);
			}
		}
	}

	private void SpeedReduce()
	{
		speedBoost = false;
		
		smoothFollow.TargetOffset += new Vector3(0, -increaseHeight, increaseDistance);
		
		boostSpeedParticles.SetActive(false);
		
		boostSpeedTrailLeft.SetActive(false);
		boostSpeedTrailRight.SetActive(false);
		
		ChangeSpeedGame();
	}
}
