using UnityEngine;

public class GameController_SkyRoads : BaseGameController
{
	[SerializeField] private bool developState;

	[Header("Main Settings")]
	[SerializeField] private int globalSpeedGame = 10;
	private const int globalSpeedGameLimit = 30;
	private const int globalSpeedGameIncreaseStep = 1;
	[SerializeField] private int secondIncreaseComplexity = 60;

	private int lastIncreaseComplexity;

	[Header("Spawn Settings")]
	[SerializeField] private int spawnCount = 5;
	[SerializeField] private float startWait = 3.0f;
	[SerializeField] private float waveDelay = 3.0f;
	private const float waveDelayDecreaseStep = 0.2f;
	[SerializeField] private float objectDelay = 1.5f;
	private const float objectDelayLimit = 0.5f;
	private const float objectDelayDecreseStep = 0.1f;
	[SerializeField] private float minDistanceBetweenObject = 2.0f;

	private float time;
	private float timeOneSecond;
	private float timeSpawn;
	private int waveCount;

	private bool startWaitPassed;
	private bool waveDelayPassed;
	private bool objectDelayPassed;

	[Header("Bonuses Settings")]
	[SerializeField] private int countScoreForAsteroid = 5;

	[SerializeField] private int countScoreForSecond = 1;
	[SerializeField] private int multScoreForBoost = 2;

	private int score;
	private int avoidAsteroid;
	private int timePlay;

	private bool scoreBestNew;
	private bool asteroidBestNew;
	private bool timeBestNew;

	[Header("Boost Settings")]
	[SerializeField] private int multForBoostSpeed = 2;

	private TimerClass timerLevel;

	private bool restartGame;
	private bool gameOver;
	private bool needSaveData;

	[System.NonSerialized] public static GameController_SkyRoads Instance;

	[Header("Managers")]
	[SerializeField] private MenuManager_SkyRoads menuManager;
	[SerializeField] private PlayerManager_SkyRoads playerManager;
	[SerializeField] private BaseTopDownSpaceShip controlManager;
	[SerializeField] private LevelManager_SkyRoads levelManager;
	[SerializeField] private BaseSoundController soundManager;
	[SerializeField] private BaseMusicController musicManager;

	private void Awake()
	{
		Init();
	}

	private void Start()
	{
		DontDestroyOnLoad(this.gameObject);
		
		playerManager.LoadPrivateDataPlayer();
		
		StartGame();
	}

	private void Update()
	{
		if (gameOver)
		{
			FinalAction();
		}
		else
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				levelManager.SpeedBoost();
			}

			MainAction();
		}
	}

	private void LateUpdate()
	{
		if (!gameOver)
		{
			BonusAction();
			SpeedAction();
			
			UpdateMenuInfo();
			UpdateMenuResult();
		}
	}
	
	private void Init()
	{
		if (Instance == null)
		{
			Instance = this;

			InitManagers();
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
	}

	private void InitManagers()
	{
		if (!menuManager)
		{
			menuManager = MenuManager_SkyRoads.Instance;
		}

		if (!playerManager)
		{
			playerManager = PlayerManager_SkyRoads.Instance;
		}

		if (!levelManager)
		{
			levelManager = LevelManager_SkyRoads.Instance;
		}

		if (!soundManager)
		{
			soundManager = BaseSoundController.Instance;
		}

		if (!musicManager)
		{
			musicManager = BaseMusicController.Instance;
		}
	}

	public virtual void StartGame()
	{
		score = 0;
		time = 0;
		avoidAsteroid = 0;
		timePlay = 0;
		restartGame = false;
		gameOver = false;

		timerLevel = ScriptableObject.CreateInstance<TimerClass>();
		StartTimer();
		
		levelManager.StartLevel();
		
		menuManager.SetNamePlayer(playerManager.GetPlayerName());
		
		UpdateMenuInfo();
		UpdateMenuResult();
		
		WindowAdviceShowText("For Exit or Pause game click ESCAPE");
	}

	public void PauseGameInvert()
	{
		Paused = !Paused;

		if (Paused)
		{
			timerLevel.StopTimer();
		}
		else
		{
			timerLevel.StartTimer();
		}
	}

	public void PauseGame()
	{
		if (!Paused)
		{
			PauseGameInvert();
		}
	}

	public void ResumeGame()
	{
		if (Paused)
		{
			PauseGameInvert();
		}
	}

	public bool DevelopState
	{
		get => developState;
		set => developState = value;
	}

	public int GlobalSpeedGame => globalSpeedGame;

	public float MultForBoostSpeed => multForBoostSpeed;

	public int CountScoreForAsteroid => countScoreForAsteroid;

	public float MinDistanceBetweenObject => minDistanceBetweenObject;

	#region MenuManager
	public void WindowAdviceShowText(string textVal, int timeVal = 0)
	{
		menuManager.WindowAdviceSetText(textVal);
		if (timeVal != 0)
		{
			menuManager.ShowWindowAdviceAtTime(timeVal);
		}
		else
		{
			if (textVal.Length <= 40)
			{
				menuManager.ShowWindowAdviceAtTime(5);
			}
			else
			{
				menuManager.ShowWindowAdviceAtTime(10);
			}
		}
	}
	#endregion

	#region PlayerManager
	public void SetNamePlayer(string val)
	{
		playerManager.SetPlayerName(val);
	}
	#endregion

	#region LevelManager
	public bool IsBoostSpeed()
	{
		return levelManager.IsSpeedBoost();
	}

	public void SaveDataLevel()
	{
		if (needSaveData)
		{
			if (scoreBestNew)
			{
				playerManager.SetScoreBest(score);
			}

			if (timeBestNew)
			{
				playerManager.SetTimeBest(timePlay);
			}

			if (asteroidBestNew)
			{
				playerManager.SetAsteroidBest(avoidAsteroid);
			}
			
			playerManager.SavePrivateDataPlayer();

			needSaveData = false;

			scoreBestNew = false;
			timeBestNew = false;
			asteroidBestNew = false;
		}
	}
	#endregion

	#region Timer
	private void StartTimer()
	{
		timerLevel.StartTimer();
	}

	private void UpdateTimer()
	{
		timerLevel.UpdateTimer();
	}

	private void StopTimer()
	{
		timerLevel.StopTimer();
	}

	private void ResetTimer()
	{
		timerLevel.ResetTimer();
	}

	public int GetTime()
	{
		return timerLevel.GetTime();
	}

	public string GetFormattedTime()
	{
		return timerLevel.GetFormattedTime();
	}
	#endregion

	#region Sound
	public void UpdateMusicVolume()
	{
		musicManager.UpdateVolume();
	}

	public void UpdateSoundVolume()
	{
		soundManager.UpdateVolume();
	}

	public void PlaySoundByIndex(int indexNumber, Vector3 aPosition)
	{
		soundManager.PlaySoundByIndex(indexNumber, aPosition);
	}

	public void PlayButtonSound()
	{
		PlaySoundByIndex(0, Vector3.zero);
	}
	
	private void FinalAction()
	{
		if (restartGame)
		{
			if (Input.GetKeyDown(KeyCode.R))
			{
				RestartGameButtonPressed();
			}
		}
		else
		{
			restartGame = true;
		}
	}
	#endregion
	
	private void MainAction()
	{
		time += Time.deltaTime * (IsBoostSpeed() ? multForBoostSpeed : 1);

		if (!startWaitPassed && time >= startWait) startWaitPassed = true;
		if (!waveDelayPassed && time >= waveDelay) waveDelayPassed = true;
		if ((!objectDelayPassed && (time - timeSpawn) >= objectDelay) || timeSpawn == 0.0f)
			objectDelayPassed = true;

		if (startWaitPassed)
		{
			if (waveDelayPassed)
			{
				if (objectDelayPassed)
				{
					levelManager.SpawnWave();

					objectDelayPassed = false;
					timeSpawn = time;
					waveCount++;

					if (waveCount >= spawnCount)
					{
						waveDelayPassed = false;
						waveCount = 0;
						timeSpawn = 0.0f;
						time = 0.0f;
					}
				}
			}
		}
	}

	private void BonusAction()
	{
		UpdateTimer();

		timePlay = GetTime();
		if (timePlay - timeOneSecond >= 1.0f)
		{
			timeOneSecond = timePlay;
			
			if (IsBoostSpeed())
			{
				AddScore(multScoreForBoost * countScoreForSecond);
			}
			else
			{
				AddScore(countScoreForSecond);
			}
		}

		if ((!timeBestNew) && (timePlay > playerManager.GetTimeBest()))
		{
			timeBestNew = true;
			needSaveData = true;

			if (playerManager.GetTimeBest() != 0)
			{
				WindowAdviceShowText("You improve Time Best!");
			}
		}
	}

	private void SpeedAction()
	{
		if (timePlay > lastIncreaseComplexity + secondIncreaseComplexity)
		{
			if (globalSpeedGame < globalSpeedGameLimit)
			{
				globalSpeedGame += globalSpeedGameIncreaseStep;
				
				levelManager.ChangeSpeedGame();
			}
			
			if (waveDelay > objectDelay)
			{
				waveDelay -= waveDelayDecreaseStep;
			}
			
			if (objectDelay > objectDelayLimit)
			{
				objectDelay -= objectDelayDecreseStep;
			}

			lastIncreaseComplexity += secondIncreaseComplexity;

			WindowAdviceShowText("Increased game speed. Good luck!");
		}
	}

	public void GameOver()
	{
		if (!gameOver)
		{
			soundManager.PlaySoundByIndex(3, Vector3.zero);

			WindowAdviceShowText("Game Over. Press \"R\" key for Restart Game.");
			menuManager.ActivateWindow(1);
			
			SaveDataLevel();

			gameOver = true;
			StopTimer();
		}
	}

	public override void RestartGameButtonPressed()
	{
		StartGame();
		
		menuManager.DisActivateWindow();
	}

	public void AddPointsForAsteroid()
	{
		if (!gameOver)
		{
			score += countScoreForAsteroid;
			avoidAsteroid += 1;

			if ((!asteroidBestNew) && (avoidAsteroid > playerManager.GetAsteroidBest()))
			{
				asteroidBestNew = true;
				needSaveData = true;

				if (playerManager.GetAsteroidBest() != 0)
				{
					WindowAdviceShowText("You improve Avoid Asteroids Best!");
				}
			}
		}
	}

	public void AddScore(int countScoreAdd)
	{
		if (!gameOver)
		{
			score += countScoreAdd;

			if ((!scoreBestNew) && (score > playerManager.GetScoreBest()))
			{
				scoreBestNew = true;
				needSaveData = true;

				if (playerManager.GetScoreBest() != 0)
				{
					WindowAdviceShowText("You improve Score Best!");
				}
			}
		}
	}
	
	private void UpdateMenuInfo()
	{
		string text = $"{score}/{playerManager.GetScoreBest()}";
		menuManager.WindowInformSetText_1(text);

		text = GetFormattedTime();
		menuManager.WindowInformSetText_2(text);

		text = $"{avoidAsteroid}/{playerManager.GetAsteroidBest()}";
		menuManager.WindowInformSetText_3(text);
	}

	private void UpdateMenuResult()
	{
		string text = $"{score}/{playerManager.GetScoreBest()}";
		menuManager.WindowResultSetText(text, 0);

		text = $"{avoidAsteroid}/{playerManager.GetAsteroidBest()}";
		menuManager.WindowResultSetText(text, 1);

		text = $"{GetFormattedTime()}/{timerLevel.GetFormattedTime(playerManager.GetTimeBest())}";
		menuManager.WindowResultSetText(text, 2);
	}
}
