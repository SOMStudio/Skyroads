using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuManager_SkyRoads : BaseMenuController {

	public bool overMenuUI = true;

	[Header("Game Settings")]
	public string namePlayerVal = "Anonim";
	public InputField namePlayer;

	[Header("Window Result")]
	public Text[] windowResultTextList;

	[Header("Developer")]
	public GameObject[] developList;

	[Header("Game Controller Ref")]
	public GameController_SkyRoads gameController;

	[System.NonSerialized]
	public static MenuManager_SkyRoads Instance;

	void Awake() {
  		// activate instance
		if (Instance == null) {
			Instance = this;

			if (!gameController)
				gameController = GameController_SkyRoads.Instance;
		} else if (Instance != this) {
			Destroy (gameObject);
		}
	}

	void LateUpdate() {
		if (didInit) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				ClickEscapeEvent ();
			}
		}
	}

	public override void RestoreOptionsPref() {
		base.RestoreOptionsPref ();

		StartCoroutine ("RestoreOptionsPrefGame_Coroutine");
	}

	public IEnumerator RestoreOptionsPrefGame_Coroutine() {
		didInit = false;
		bool prefubInit = false;

		yield return null;

		didInit = true;

		yield return null;

		// save init parametrs
		if (prefubInit) {
			SaveOptionsPrefs ();
		}

		yield return null;

		//hide develop objects
		HideDevelopList ();

		yield return null;
	}


	private void ClickEscapeEvent() {
		if (consoleWindowActive == -1) {
			if (windowActive == -1) { //Main Menu
				ExitGameConsoleWindow ();
			}
		}
	}

	public override void ExitGame ()
	{
		gameController.SaveDataLevel ();

		gameController.PlayButtonSound ();
		
		base.ExitGame ();
	}

	public void ExitGameConsoleWindow() {
		ConsoleWinYesNo_ActionCloseGame ();
		ActivateConsoleWindow (2);
	}

	public override void SaveOptionsPrefs ()
	{
		base.SaveOptionsPrefs ();

		StartCoroutine ("SaveOptionsPrefsGame_Coroutine");
	}

	public IEnumerator SaveOptionsPrefsGame_Coroutine ()
	{
		yield return null;

		gameController.UpdateSoundVolume ();
		gameController.UpdateMusicVolume ();

		yield return null;
	}

	//event menu
	public override void ActivateMenuEvent() {
		
	}

	public override void DisActivateMenuEvent() {
		//play sound button-click
		gameController.PlaySoundByIndex (2, transform.position);
	}

	public override void ChancheMenuEvent(int number) {
		//play sound button-click
		gameController.PlaySoundByIndex (1, transform.position);
	}

	public override void ActivateWindowEvent() {
		//play sound button-click
		gameController.PlaySoundByIndex (1, transform.position);
	}

	public override void DisActivateWindowEvent() {
		//play sound button-click
		gameController.PlaySoundByIndex (2, transform.position);

		if (windowActive == 1) {

		} else {
			gameController.PauseGame ();
		}
	}

	public override void ChancheWindowEvent(int number) {
		if (windowActive == 1) {

		} else {
			gameController.ResumeGame ();
		}
	}

	public override void ActivateConsoleWEvent ()
	{
		//play sound button-click
		gameController.PlaySoundByIndex (1, transform.position);

		gameController.PauseGame ();
	}

	public override void DisActivateConsoleWEvent ()
	{
		//play sound button-click
		gameController.PlaySoundByIndex (2, transform.position);
		
		gameController.ResumeGame ();
	}

	public override void ChancheConsoleWEvent(int number) {

	}

	// main part
	public void OverUI() {
		overMenuUI = true;
	}

	public void OutUI() {
		overMenuUI = false;
	}

	public void ActivateMenu_Level() {
		ActivateMenu (1);
	}

	public void ChangeGraficVal_PostEffect(float val) {
//		if (gameController.CameraControlData != null) {
//			gameController.CameraControlData.CollorCorrectionOn ();
//			gameController.CameraControlData.BloomOn ();
//			gameController.CameraControlData.VignetteOn ();
//		}
	}

	// window Settings
	public void ChangeNamePlayer(string val) {
		namePlayerVal = val;

		gameController.SetNamePlayer (val);

		if (val == "TEST") {
			gameController.developState = true;

			ShowDevelopList ();
		} else {
			gameController.developState = false;

			HideDevelopList ();
		}
	}

	public void SetNamePlayer(string val) {
		namePlayerVal = val;

		if (namePlayer) {
			namePlayer.text = val;
		}
	}

	// window Result
	public void WindowResultSetText(string stAdvice, int numText) {
		if (numText < windowResultTextList.Length) {
			if (windowResultTextList [numText]) {
				windowResultTextList [numText].text = ConvertSpecTextChar (stAdvice);
			}
		}
	}

	// develop List
	private void HideDevelopList() {
		foreach (GameObject item in developList) {
			if (item) {
				if (item.activeSelf) {
					item.SetActive (false);
				}
			}
		}
	}

	private void ShowDevelopList() {
		foreach (GameObject item in developList) {
			if (item) {
				if (!item.activeSelf) {
					item.SetActive (true);
				}
			}
		}
	}

	// actions
	public void ConsoleWinYesNo_ActionCloseGame() {
		ConsoleWinYesNo_SetTxt ("Do you want to Exit?");
		ConsoleWinYesNo_SetYesAction (ExitGame);
	}
}
