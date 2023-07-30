using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuManager_SkyRoads : BaseMenuController
{
	[SerializeField] private bool overMenuUI = true;

	[Header("Game Settings")]
	[SerializeField] private string namePlayerVal = "Anonim";

	[SerializeField] private InputField namePlayer;

	[Header("Window Result")]
	[SerializeField] private Text[] windowResultTextList;

	[Header("Developer")]
	[SerializeField] private GameObject[] developList;

	[Header("Game Controller Ref")]
	[SerializeField] private GameController_SkyRoads gameController;

	[System.NonSerialized] public static MenuManager_SkyRoads Instance;
	
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

	private void LateUpdate()
	{
		if (didInit)
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				ClickEscapeEvent();
			}
		}
	}
	
	protected override void RestoreOptionsPref()
	{
		base.RestoreOptionsPref();

		StartCoroutine(nameof(RestoreOptionsPrefGame_Coroutine));
	}

	private IEnumerator RestoreOptionsPrefGame_Coroutine()
	{
		didInit = false;

		yield return null;

		didInit = true;

		yield return null;
		
		HideDevelopList();

		yield return null;
	}


	private void ClickEscapeEvent()
	{
		if (consoleWindowActive == -1)
		{
			if (windowActive == -1)
			{
				ExitGameConsoleWindow();
			}
		}
	}

	protected override void ExitGame()
	{
		gameController.SaveDataLevel();

		gameController.PlayButtonSound();

		base.ExitGame();
	}

	public void ExitGameConsoleWindow()
	{
		ConsoleWinYesNo_ActionCloseGame();
		ActivateConsoleWindow(2);
	}

	protected override void SaveOptionsPrefs()
	{
		base.SaveOptionsPrefs();

		StartCoroutine(nameof(SaveOptionsPrefsGame_Coroutine));
	}

	private IEnumerator SaveOptionsPrefsGame_Coroutine()
	{
		yield return null;

		gameController.UpdateSoundVolume();
		gameController.UpdateMusicVolume();

		yield return null;
	}

	#region Events
	protected override void ActivateMenuEvent()
	{

	}

	protected override void DisActivateMenuEvent()
	{
		gameController.PlaySoundByIndex(2, transform.position);
	}

	protected override void ChangeMenuEvent(int number)
	{
		gameController.PlaySoundByIndex(1, transform.position);
	}

	protected override void ActivateWindowEvent()
	{
		gameController.PlaySoundByIndex(1, transform.position);
	}

	protected override void DisActivateWindowEvent()
	{
		gameController.PlaySoundByIndex(2, transform.position);

		if (windowActive == 1)
		{

		}
		else
		{
			gameController.PauseGame();
		}
	}

	protected override void ChangeWindowEvent(int number)
	{
		if (windowActive == 1)
		{

		}
		else
		{
			gameController.ResumeGame();
		}
	}

	protected override void ActivateConsoleWEvent()
	{
		gameController.PlaySoundByIndex(1, transform.position);

		gameController.PauseGame();
	}

	protected override void DisActivateConsoleWEvent()
	{
		gameController.PlaySoundByIndex(2, transform.position);

		gameController.ResumeGame();
	}

	protected override void ChangeConsoleWEvent(int number)
	{

	}
	#endregion
	
	private void OverUI()
	{
		overMenuUI = true;
	}

	private void OutUI()
	{
		overMenuUI = false;
	}

	private void ActivateMenu_Level()
	{
		ActivateMenu(1);
	}

	#region WindowSettings
	public void ChangeNamePlayer(string val)
	{
		namePlayerVal = val;

		gameController.SetNamePlayer(val);

		if (val == "TEST")
		{
			gameController.DevelopState = true;

			ShowDevelopList();
		}
		else
		{
			gameController.DevelopState = false;

			HideDevelopList();
		}
	}

	public void SetNamePlayer(string val)
	{
		namePlayerVal = val;

		if (namePlayer)
		{
			namePlayer.text = val;
		}
	}
	#endregion

	#region windowResult
	public void WindowResultSetText(string stAdvice, int numText)
	{
		if (numText < windowResultTextList.Length)
		{
			if (windowResultTextList[numText])
			{
				windowResultTextList[numText].text = ConvertSpecTextChar(stAdvice);
			}
		}
	}
	#endregion

	#region DevelopList
	private void HideDevelopList()
	{
		foreach (GameObject item in developList)
		{
			if (item)
			{
				if (item.activeSelf)
				{
					item.SetActive(false);
				}
			}
		}
	}

	private void ShowDevelopList()
	{
		foreach (GameObject item in developList)
		{
			if (item)
			{
				if (!item.activeSelf)
				{
					item.SetActive(true);
				}
			}
		}
	}
	#endregion
	
	private void ConsoleWinYesNo_ActionCloseGame()
	{
		ConsoleWinYesNo_SetTxt("Do you want to Exit?");
		ConsoleWinYesNo_SetYesAction(ExitGame);
	}
}
