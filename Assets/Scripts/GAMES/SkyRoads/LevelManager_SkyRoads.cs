﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager_SkyRoads : MonoBehaviour {

	[Header("Asteroids")]
	public GameObject spawn_1;
	public GameObject spawn_2;
	public GameObject spawn_3;
	public Transform spawnPosition;

	private Vector3 prevPosSpawn = Vector3.zero;

	[Header("Smooth Follow")]
	public FallowTarget smoothFollow;
	public float increaseDistance = 5;
	public float increaseHeigh = 2;

	[Header("Boost Speed Environment")]
	public GameObject boostSpeedParticles;

	[Header("Boost Speed Player")]
	public GameObject boostSpeedTrailLeft;
	public GameObject boostSpeedTrailRight;

	private bool speedBoost = false;

	[Header("Game Controller Ref")]
	public GameController_SkyRoads gameController;

	[System.NonSerialized]
	public static LevelManager_SkyRoads Instance;

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

	// Spawn part
	public void SpawnWave() {
		Vector3 startPosition = GetPossitio();
		Quaternion startQuaternion = Quaternion.identity;

		int asteroidNumber = Random.Range(1, 3);
		if (asteroidNumber == 1)
		{
			Instantiate(spawn_1, startPosition, startQuaternion);
		}
		else if (asteroidNumber == 2)
		{
			Instantiate(spawn_2, startPosition, startQuaternion);
		}
		else
		{
			Instantiate(spawn_3, startPosition, startQuaternion);
		}
	}

	Vector3 GetPossitio() {
		float xPos = Random.Range (spawnPosition.position.x - 3.0f, spawnPosition.position.x + 3.0f);

		Vector3 resPos = new Vector3(xPos, spawnPosition.position.y, spawnPosition.position.z);
		bool findProblem = false;

		float distance = Vector3.Distance(resPos, prevPosSpawn);
		if (distance <= gameController.minDistanceBetweenObject)
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


	// Smooth Fallow
	public bool IsSpeedBoost() {
		return speedBoost;
	}

	public void SpeedBoost() {
		if (!speedBoost) {
			speedBoost = true;

			//smooth Follow
			smoothFollow.targetOffset.z -= increaseDistance;
			smoothFollow.targetOffset.y += increaseHeigh;

			//boost Envarinment Particles
			boostSpeedParticles.SetActive (true);

			//boost Player Particles
			boostSpeedTrailLeft.SetActive (true);
			boostSpeedTrailRight.SetActive (true);

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
		smoothFollow.targetOffset.z += increaseDistance;
		smoothFollow.targetOffset.y -= increaseHeigh;

		//boost Envarinment Particles
		boostSpeedParticles.SetActive (false);

		//boost Player Particles
		boostSpeedTrailLeft.SetActive (false);
		boostSpeedTrailRight.SetActive (false);
	}
}
