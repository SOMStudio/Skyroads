using UnityEngine;
using System.Collections.Generic;

public class GameController_SkyRoads : BaseGameController {

	public bool developState = false;

	[Header("Main Settings")]
	public int globalSpeedGame = 10;
	public int secondIncreaseComplexity = 60;

	private int lastIncreaseCopmlexity = 0;

	[Header("Spawn Settings")]
	public int spawnCount = 5;
	public float startWait = 3.0f;
	public float waveDelay = 3.0f;
	public float objectDelay = 1.0f;
	public float minDistanceBetweenObject = 2.0f;

	private float time = 0.0f;
	private float timeOneSecond = 0.0f;
	private float timeSpown = 0.0f;
	private int waveCount = 0;

	private bool startWaitPassed = false;
	private bool waveDelayPassed = false;
	private bool objectDelayPassed = false;

	[Header("Bonuse Settings")]
	public int countScoreForAsteroid = 5;
	public int countScoreForSecont = 1;

	private int score = 0;
	private int avoidAsteroid = 0;
	private int timePlay = 0;

	private bool scoreBestNew = false;
	private bool asteroidBestNew = false;
	private bool timeBestNew = false;

	[Header("Boost Settings")]
	public float multForBoostSpeed = 2.0f;

	private TimerClass timerLevel;

	private bool restartGame;
	private bool gameOver;
	private bool needSaveData = false;

	[System.NonSerialized]
	public static GameController_SkyRoads Instance;

	[Header("Managers")]
	public MenuManager_SkyRoads menuMenager;
	public PlayerManager_SkyRoads playerManager;
	public BaseTopDownSpaceShip controlManager;
	public LevelManager_SkyRoads levelManager;
	public BaseSoundController soundManager;
	public BaseMusicController musicManager;

	void Awake() {
		// activate instance
		if (Instance == null) {
			Instance = this;

			InitManagers ();
		} else if (Instance != this) {
			Destroy (gameObject);
		}
	}

	void Start() {
		// load date
		playerManager.LoadPrivateDataPlayer ();

		// start level
		StartGame ();
	}

	void Update() {
		if (gameOver)
		{
			FinalAction ();
		}
		else
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				levelManager.SpeedBoost ();
			}

			MainAction ();
		}
	}

	void LateUpdate() {
		if (!gameOver)
		{
			BonusAction ();
			SpeedAction ();

			//user interface
			UpdateMenuInfo ();
			UpdateMenuResult ();
		}
	}

	void Init() {
		// activate instance
		if (Instance == null) {
			Instance = this;

			InitManagers ();
		} else if (Instance != this) {
			Destroy (gameObject);
		}
	}

	void InitManagers() {
		if (!menuMenager) {
			menuMenager = MenuManager_SkyRoads.Instance;
		}
		if (!playerManager) {
			playerManager = PlayerManager_SkyRoads.Instance;
		}
		if (!levelManager) {
			levelManager = LevelManager_SkyRoads.Instance;
		}
		if (!soundManager) {
			soundManager = BaseSoundController.Instance;
		}
		if (!musicManager) {
			musicManager = BaseMusicController.Instance;
		}
	}

	public override void StartGame() {
		score = 0;
		time = 0;
		avoidAsteroid = 0;
		timePlay = 0;
		restartGame = false;
		gameOver = false;

		timerLevel = ScriptableObject.CreateInstance<TimerClass>();
		StartTimer ();

		//update Menu Info in window
		menuMenager.SetNamePlayer(playerManager.GetPlayerName());

		//update info in WindowInform
		UpdateMenuInfo ();
		UpdateMenuResult ();

		//show in start game
		WindowAdviceShowText ("For Exit or Pouse game click ESCAPE");
	}

	public void PauseGameInvert() {
		Paused = !Paused;

		if (Paused) {
			timerLevel.StopTimer ();
		} else {
			timerLevel.StartTimer ();
		}
	}

	public void PauseGame() {
		if (!Paused) {
			PauseGameInvert ();
		}
	}

	public void ResumeGame() {
		if (Paused) {
			PauseGameInvert ();
		}
	}

	//Menu Manager
	public void WindowAdviceShowText(string textVal, int timeVal = 0) {
		menuMenager.WindowAdviceSetText (textVal);
		if (timeVal != 0) {
			menuMenager.ShowWindowAdviceAtTime (timeVal);
		} else {
			if (textVal.Length <= 40) {
				menuMenager.ShowWindowAdviceAtTime (5);
			} else {
				menuMenager.ShowWindowAdviceAtTime (10);
			}
		}
	}

	// Player Manager
	public void SetNamePlayer(string val) {
		playerManager.SetPlayerName (val);
	}

	// level Manager
	public bool IsBoostSpeed() {
		return levelManager.IsSpeedBoost ();
	}

	public void SaveDataLevel() {
		if (needSaveData) {
			// set new records
			if (scoreBestNew) {
				playerManager.SetScoreBest (score);
			}
			if (timeBestNew) {
				playerManager.SetTimeBest (timePlay);
			}
			if (asteroidBestNew) {
				playerManager.SetAsteroidBest (avoidAsteroid);
			}

			//save new records
			playerManager.SavePrivateDataPlayer ();

			needSaveData = false;

			scoreBestNew = false;
			timeBestNew = false;
			asteroidBestNew = false;
		}
	}

	// Timer
	private void StartTimer() {
		timerLevel.StartTimer ();
	}

	private void UpdateTimer() {
		timerLevel.UpdateTimer ();
	}

	private void StopTimer() {
		timerLevel.StopTimer ();
	}

	private void ResetTimer() {
		timerLevel.ResetTimer ();
	}

	public int GetTime() {
		return timerLevel.GetTime ();
	}

	public string GetFormattedTime() {
		return timerLevel.GetFormattedTime ();
	}

	// Sound Manager
	public void UpdateMusicVolume() {
		musicManager.UpdateValume ();
	}

	public void UpdateSoundVolume() {
		soundManager.UpdateVolume ();
	}

	public void PlaySoundByIndex(int indexNumber, Vector3 aPosition) {
		soundManager.PlaySoundByIndex (indexNumber, aPosition);
	}

	public void PlayButtonSound() {
		PlaySoundByIndex (0, Vector3.zero);
	}

	// logic part
	void FinalAction() {
		if (restartGame)
		{
			if (Input.GetKeyDown(KeyCode.R))
			{
				RestartGame ();
			}
		}
		else
		{
			restartGame = true;
		}
	}

	void MainAction() {
		time += Time.deltaTime;

		if ((!startWaitPassed) && (time >= startWait)) startWaitPassed = true;
		if ((!waveDelayPassed) && (time >= waveDelay)) waveDelayPassed = true;
		if (((!objectDelayPassed) && ((time - timeSpown) >= objectDelay)) || (timeSpown == 0.0f)) objectDelayPassed = true;

		if (startWaitPassed)
		{
			if (waveDelayPassed)
			{
				if (objectDelayPassed) {
					levelManager.SpawnWave ();

					objectDelayPassed = false;
					timeSpown = time;
					waveCount++;

					if (waveCount >= spawnCount) {
						waveDelayPassed = false;
						waveCount = 0;
						timeSpown = 0.0f;
						time = 0.0f;
					}
				}
			}
		}
	}

	void BonusAction() {
		UpdateTimer ();

		timePlay = GetTime ();
		if (timePlay - timeOneSecond >= 1.0f) {
			timeOneSecond = timePlay;

			// add bonuses
			if (IsBoostSpeed ()) {
				AddScore (2);
			} else {
				AddScore (1);
			}
		}

		if ((!timeBestNew) && (timePlay > playerManager.GetTimeBest ())) {
			timeBestNew = true;
			needSaveData = true;

			if (playerManager.GetTimeBest () != 0) {
				WindowAdviceShowText ("You improve Time Best!");
			}
		}
	}

	void SpeedAction() {
		if (timePlay > lastIncreaseCopmlexity + secondIncreaseComplexity) {
			// speed Game
			if (globalSpeedGame > -30) {
				globalSpeedGame -= 1;
			}

			// decrease WaweDelay
			if (waveDelay > objectDelay) {
				waveDelay -= 0.5f;
			}

			// decrease ObjectDelay
			if (objectDelay > 0.5f) {
				objectDelay -= 0.1f;
			}

			lastIncreaseCopmlexity += secondIncreaseComplexity;

			WindowAdviceShowText ("Increased game speed. Good luck!");
		}
	}

	public void GameOver() {
		if (!gameOver) {
			soundManager.PlaySoundByIndex (3, Vector3.zero);

			// controll sheep reset
			//controlManager.canControl = false;

			// user interface
			WindowAdviceShowText ("Game Over. Press \"R\" key for Restart Game.");
			menuMenager.ActivateWindow (1);

			// save progress
			SaveDataLevel ();

			gameOver = true;
			StopTimer ();
		}
	}

	public void RestartGame() {
		RestartGameButtonPressed ();
	}

	public void AddPointsForAsteroid() {
		if (!gameOver) {
			score += countScoreForAsteroid;
			avoidAsteroid += 1;

			if ((!asteroidBestNew) && (avoidAsteroid > playerManager.GetAsteroidBest ())) {
				asteroidBestNew = true;
				needSaveData = true;

				if (playerManager.GetAsteroidBest () != 0) {
					WindowAdviceShowText ("You improve Avoid Asteroids Best!");
				}
			}
		}
	}

	public void AddScore(int countScoreAdd) {
		if (!gameOver) {
			score += countScoreAdd;

			if ((!scoreBestNew) && (score > playerManager.GetScoreBest ())) {
				scoreBestNew = true;
				needSaveData = true;

				if (playerManager.GetScoreBest () != 0) {
					WindowAdviceShowText ("You improve Score Best!");
				}
			}
		}
	}

	// menu Manager
	void UpdateMenuInfo () {
		string text = string.Format("{0}/{1}", score, playerManager.GetScoreBest());
		menuMenager.WindowInformSetText_1 (text);

		text = GetFormattedTime();
		menuMenager.WindowInformSetText_2 (text);

		text = string.Format ("{0}/{1}", avoidAsteroid, playerManager.GetAsteroidBest());
		menuMenager.WindowInformSetText_3 (text);
	}

	void UpdateMenuResult () {
		string text = string.Format("{0}/{1}", score, playerManager.GetScoreBest());
		menuMenager.WindowResultSetText (text, 0);

		text = string.Format ("{0}/{1}", avoidAsteroid, playerManager.GetAsteroidBest());
		menuMenager.WindowResultSetText (text, 1);

		text = string.Format ("{0}/{1}", GetFormattedTime (), timerLevel.GetFormattedTime (playerManager.GetTimeBest ()));
		menuMenager.WindowResultSetText (text, 2);
	}
}
