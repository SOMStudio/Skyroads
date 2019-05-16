using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuManager_SkyRoads : BaseMenuController {

	[SerializeField]
	private bool overMenuUI = true;

	[Header("Game Settings")]
	[SerializeField]
	private string namePlayerVal = "Anonim";
	[SerializeField]
	private InputField namePlayer;

	[Header("Window Result")]
	[SerializeField]
	private Text[] windowResultTextList;

	[Header("Developer")]
	[SerializeField]
	private GameObject[] developList;

	[Header("Game Controller Ref")]
	[SerializeField]
	private GameController_SkyRoads gameController;

	[System.NonSerialized]
	public static MenuManager_SkyRoads Instance;

	// main event
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

	// main logic
	protected override void RestoreOptionsPref() {
		base.RestoreOptionsPref ();

		StartCoroutine ("RestoreOptionsPrefGame_Coroutine");
	}

	private IEnumerator RestoreOptionsPrefGame_Coroutine() {
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

	protected override void ExitGame ()
	{
		gameController.SaveDataLevel ();

		gameController.PlayButtonSound ();
		
		base.ExitGame ();
	}

	public void ExitGameConsoleWindow() {
		ConsoleWinYesNo_ActionCloseGame ();
		ActivateConsoleWindow (2);
	}

	protected override void SaveOptionsPrefs ()
	{
		base.SaveOptionsPrefs ();

		StartCoroutine ("SaveOptionsPrefsGame_Coroutine");
	}

	private IEnumerator SaveOptionsPrefsGame_Coroutine ()
	{
		yield return null;

		gameController.UpdateSoundVolume ();
		gameController.UpdateMusicVolume ();

		yield return null;
	}

	#region Events
	protected override void ActivateMenuEvent() {
		
	}

	protected override void DisActivateMenuEvent() {
		//play sound button-click
		gameController.PlaySoundByIndex (2, transform.position);
	}

	protected override void ChancheMenuEvent(int number) {
		//play sound button-click
		gameController.PlaySoundByIndex (1, transform.position);
	}

	protected override void ActivateWindowEvent() {
		//play sound button-click
		gameController.PlaySoundByIndex (1, transform.position);
	}

	protected override void DisActivateWindowEvent() {
		//play sound button-click
		gameController.PlaySoundByIndex (2, transform.position);

		if (windowActive == 1) {

		} else {
			gameController.PauseGame ();
		}
	}

	protected override void ChancheWindowEvent(int number) {
		if (windowActive == 1) {

		} else {
			gameController.ResumeGame ();
		}
	}

	protected override void ActivateConsoleWEvent ()
	{
		//play sound button-click
		gameController.PlaySoundByIndex (1, transform.position);

		gameController.PauseGame ();
	}

	protected override void DisActivateConsoleWEvent ()
	{
		//play sound button-click
		gameController.PlaySoundByIndex (2, transform.position);
		
		gameController.ResumeGame ();
	}

	protected override void ChancheConsoleWEvent(int number) {

	}
	#endregion

	// main part
	private void OverUI() {
		overMenuUI = true;
	}

	private void OutUI() {
		overMenuUI = false;
	}

	private void ActivateMenu_Level() {
		ActivateMenu (1);
	}

	private void ChangeGraficVal_PostEffect(float val) {
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
			gameController.DevelopState = true;

			ShowDevelopList ();
		} else {
			gameController.DevelopState = false;

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
	private void ConsoleWinYesNo_ActionCloseGame() {
		ConsoleWinYesNo_SetTxt ("Do you want to Exit?");
		ConsoleWinYesNo_SetYesAction (ExitGame);
	}
}
