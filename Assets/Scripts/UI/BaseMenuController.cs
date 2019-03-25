using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BaseMenuController : MonoBehaviour {

	public bool didInit = false;

	[Header("Base Settings")]
	public string gamePrefsName= "DefaultGame";
	//audio
	public float audioSFXSliderValue;
	public Slider audioSFXSlider;
	public float audioMusicSliderValue;
	public Slider audioMusicSlider;
	//graphic
	public float graphicsSliderValue;
	public Slider graphicsSlider;
	public int graphicsDefaultValue = -1;

	private int detailLevels= 6;
	private bool needSaveOptions = false;

	[Header("Main window list")]
	public AnimationOpenClose[] panelAnimations;
	public AnimationOpenClose[] windowAnimations;
	public bool[] accessToPanel;

	[Header("DisActivate window")]
	public AnimationOpenClose panelDisActivateAnimation;
	public AnimationOpenClose windowDisActivateAnimation;
	public AnimationOpenClose consoleWindowDisActivateAnimation;

	[Header("Menu Data")]
	public bool menuActive = false;
	public int panelActive = -1;
	public int windowActive = -1;
	public int consoleWindowActive = 5;

	[Header("StartGame window")]
	public bool windowStartActive = true;
	public AnimationOpenClose windowStartGameAnimation;

	[Header("Advice window")]
	public AnimationOpenClose windowAdviceAnimation;
	public Text windowAdviceText;

	[Header("Inform window")]
	public AnimationOpenClose windowInformAnimation;
	public Text[] windowInformTextList;

	[Header("Console windows")]
	//smol
	public Text consoleWInformSmolTextHead;
	public Text consoleWInformSmolTextInfo;
	public GameObject consoleWInformSmolPanelImage;
	public Image consoleWInformSmolImage;
	//Big
	public Text consoleWInformBigTextHead;
	public Text consoleWInformBigTextInfo;
	public GameObject consoleWInformBigPanelImage;
	public Image consoleWInformBigImage;
	//YesNo
	public Text consoleWInYesNoTextHead;
	private UnityEvent consoleWInYesNoActinYes = new UnityEvent();

	void Start()
	{
		RestoreOptionsPref ();
	}

	public virtual void RestoreOptionsPref()
	{
		string stKey = "";

		// set up default options, if they have been saved out to prefs already
		stKey = string.Format("{0}_SFXVol", gamePrefsName);
		if (PlayerPrefs.HasKey (stKey)) {
			audioSFXSliderValue = PlayerPrefs.GetFloat (stKey);
		} else {
			// if we are missing an SFXVol key, we won't got audio defaults set up
			audioSFXSliderValue = 1;
		}
		stKey = string.Format("{0}_MusicVol", gamePrefsName);
		if (PlayerPrefs.HasKey (stKey)) {
			audioMusicSliderValue = PlayerPrefs.GetFloat (stKey);
		} else {
			// defaults set up
			audioMusicSliderValue = 1;
		}
		stKey = string.Format("{0}_GraphicsDetail", gamePrefsName);
		if (PlayerPrefs.HasKey (stKey)) {
			graphicsSliderValue = PlayerPrefs.GetFloat (stKey);
		} else {
			// defaults set up
			if (graphicsDefaultValue == -1) {
				string[] names = QualitySettings.names;
				detailLevels = names.Length;

				switch (Application.platform) {
				case RuntimePlatform.Android:
				case RuntimePlatform.IPhonePlayer:
					graphicsSliderValue = 0;
					break;
				default:
					graphicsSliderValue = detailLevels;
					break;
				}
			} else {
				graphicsSliderValue = graphicsDefaultValue;
			}
		}

		Debug.Log ("quality=" + graphicsSliderValue);

		// set the quality setting
		QualitySettings.SetQualityLevel ((int)graphicsSliderValue, true);

		//set in UI
		if (audioSFXSlider != null) {
			audioSFXSlider.value = audioSFXSliderValue;
		}
		if (audioMusicSlider != null) {
			audioMusicSlider.value = audioMusicSliderValue;
		}
		if (graphicsSlider != null) {
			//chack max value
			string[] namesQlt = QualitySettings.names;
			graphicsSlider.maxValue = namesQlt.Length - 1;

			graphicsSlider.value = graphicsSliderValue;
		}

		windowAdviceText.text = "";

		didInit = true;
	}

	public virtual void SaveOptionsPrefs()
	{
		string stKey = "";

		stKey = string.Format("{0}_SFXVol", gamePrefsName);
		PlayerPrefs.SetFloat(stKey, audioSFXSliderValue);
		stKey = string.Format("{0}_MusicVol", gamePrefsName);
		PlayerPrefs.SetFloat(stKey, audioMusicSliderValue);
		stKey = string.Format("{0}_GraphicsDetail", gamePrefsName);
		PlayerPrefs.SetFloat(stKey, graphicsSliderValue);

		// set the quality setting
		QualitySettings.SetQualityLevel( (int)graphicsSliderValue, true);
	}

	public void ChangeSFXVal(float val) {
		audioSFXSliderValue = val;

		if (didInit) {
			SaveOptionsPrefs ();
		}
	}

	public void ChangeMusicVal(float val) {
		audioMusicSliderValue = val;

		if (didInit) {
			SaveOptionsPrefs ();
		}
	}

	public void ChangeGraficVal(float val) {
		graphicsSliderValue = val;

		if (didInit) {
			SaveOptionsPrefs ();
		}
	}

	public virtual void ExitGame()
	{
		#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}

	//MainMenu
	private void PlayPanelAnim_Open(int number) {
		if (number < panelAnimations.Length) {
			AnimationOpenClose activeA = panelAnimations [number];

			if (activeA) {
				if (!activeA.IsOpen()) {
					activeA.Open ();
				}
			}
		}
	}

	private void PlayPanelAnim_Close(int number) {
		if (number < panelAnimations.Length) {
			AnimationOpenClose activeA = panelAnimations [number];

			if (activeA) {
				if (activeA.IsOpen()) {
					activeA.Close ();
				}
			}
		}
	}

	private void PlayPanelAnim_Hide(int number) {
		if (number < panelAnimations.Length) {
			AnimationOpenClose activeA = panelAnimations [number];

			if (activeA) {
				activeA.Hide ();
			}
		}
	}

	private void PlayPanelAnim_Show(int number) {
		if (number < panelAnimations.Length) {
			AnimationOpenClose activeA = panelAnimations [number];

			if (activeA) {
				activeA.Show ();
			}
		}
	}

	private void PlayWindowAnim_Open(int number) {
		if (number < windowAnimations.Length) {
			AnimationOpenClose activeA = windowAnimations [number];

			if (activeA) {
				if (!activeA.IsOpen()) {
					activeA.Open ();
				}
			}
		}
	}

	private void PlayWindowAnim_Close(int number) {
		if (number < windowAnimations.Length) {
			AnimationOpenClose activeA = windowAnimations [number];

			if (activeA) {
				if (activeA.IsOpen()) {
					activeA.Close ();
				}
			}
		}
	}

	private void PanelDisActivate_Open() {
		if (panelDisActivateAnimation) {
			panelDisActivateAnimation.Open ();
		}
	}

	private void PanelDisActivate_Close() {
		if (panelDisActivateAnimation) {
			panelDisActivateAnimation.Close ();
		}
	}

	private void WindowDisActivate_Open() {
		if (windowDisActivateAnimation) {
			windowDisActivateAnimation.Open ();
		}
	}

	private void WindowDisActivate_Close() {
		if (windowDisActivateAnimation) {
			windowDisActivateAnimation.Close ();
		}
	}

	private void ConsoleWindowDisActivate_Open() {
		if (consoleWindowDisActivateAnimation) {
			consoleWindowDisActivateAnimation.Open ();
		}
	}

	private void ConsoleWindowDisActivate_Close() {
		if (consoleWindowDisActivateAnimation) {
			consoleWindowDisActivateAnimation.Close ();
		}
	}

	//advice window
	private void PlayWindowStartGameAnim_Open() {
		if (windowStartGameAnimation) {
			AnimationOpenClose activeA = windowStartGameAnimation;

			if (activeA) {
				if (!activeA.IsOpen()) {
					activeA.Open ();
				}
			}
		}
	}

	private void PlayWindowStartGameAnim_Close() {
		if (windowStartGameAnimation) {
			AnimationOpenClose activeA = windowStartGameAnimation;

			if (activeA) {
				if (activeA.IsOpen()) {
					activeA.Close ();
				}
			}
		}
	}

	//advice window
	private void PlayWindowAdviceAnim_Open() {
		if (windowAdviceAnimation) {
			AnimationOpenClose activeA = windowAdviceAnimation;

			if (activeA) {
				if (!activeA.IsOpen()) {
					activeA.Open ();
				}
			}
		}
	}

	private void PlayWindowAdviceAnim_Close() {
		if (windowAdviceAnimation) {
			AnimationOpenClose activeA = windowAdviceAnimation;

			if (activeA) {
				if (activeA.IsOpen()) {
					activeA.Close ();

					Invoke ("WindowAdviceClearText", 0.2f);
				}
			}
		}
	}

	public void WindowAdviceSetText(string stAdvice) {
		if (windowAdviceText) {
			string stText = windowAdviceText.text;
			string[] stRes = stText.Split (new string[] { "\n", "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);

			if (stRes.Length > 2) {
				for (int i = 1; i < stRes.Length; i++) {
					if (i==1) {
						windowAdviceText.text = stRes [i];
					} else {
						windowAdviceText.text = string.Format ("{0}\n{1}", windowAdviceText.text, stRes [i]);
					}
				}
			}

			if (stRes.Length == 0) {
				windowAdviceText.text = ConvertSpecTextChar (stAdvice);
			} else {
				windowAdviceText.text = string.Format ("{0}\n{1}", windowAdviceText.text, ConvertSpecTextChar (stAdvice));
			}
		}
	}

	public void WindowAdviceClearText() {
		windowAdviceText.text = "";
	}

	//inform window
	private void PlayWindowInformAnim_Open() {
		if (windowInformAnimation) {
			AnimationOpenClose activeA = windowInformAnimation;

			if (activeA) {
				if (!activeA.IsOpen()) {
					activeA.Open ();

					ActivateInformWEvent ();
				}
			}
		}
	}

	private void PlayWindowInformAnim_Close() {
		if (windowInformAnimation) {
			AnimationOpenClose activeA = windowInformAnimation;

			if (activeA) {
				if (activeA.IsOpen()) {
					activeA.Close ();

					DisActivateInformWEvent ();
				}
			}
		}
	}

	public void WindowInformSetText(string stAdvice, int numText) {
		if (numText < windowInformTextList.Length) {
			if (windowInformTextList [numText]) {
				windowInformTextList [numText].text = ConvertSpecTextChar (stAdvice);
			}
		}
	}

	public void WindowInformSetText_1(string stAdvice) {
		WindowInformSetText (stAdvice, 0);
	}

	public void WindowInformSetText_2(string stAdvice) {
		WindowInformSetText (stAdvice, 1);
	}

	public void WindowInformSetText_3(string stAdvice) {
		WindowInformSetText (stAdvice, 2);
	}

	// methods for use

	public virtual void ActivateMenuEvent() {
		
	}

	public virtual void DisActivateMenuEvent() {

	}

	public virtual void ChancheMenuEvent(int number) {

	}

	public virtual void ActivateWindowEvent() {

	}

	public virtual void DisActivateWindowEvent() {

	}

	public virtual void ChancheWindowEvent(int number) {

	}

	public virtual void ActivateConsoleWEvent() {

	}

	public virtual void DisActivateConsoleWEvent() {

	}

	public virtual void ChancheConsoleWEvent(int number) {

	}

	private void ShowPanelMenu() {
		for (int i = 0; i < accessToPanel.Length; i++) {
			if (accessToPanel [i] == true) {
				PlayPanelAnim_Show (i);
			}
		}
	}

	private void HidePanelMenu() {
		for (int i = 0; i < accessToPanel.Length; i++) {
			if (accessToPanel [i] == true) {
				PlayPanelAnim_Hide (i);
			}
		}
	}

	public void ShowMenu() {
		ShowPanelMenu ();
	}

	public void HideMenu() {
		DisActivateMenu ();

		HidePanelMenu ();
	}

	public void ActivateMenu(int number) {
		if (panelActive == number) {
			DisActivateMenu ();
		} else {
			if (panelActive > -1) {
				PlayPanelAnim_Close (panelActive);
				PlayWindowAnim_Close (panelActive);
			}

			PlayPanelAnim_Open (number);
			PlayWindowAnim_Open (number);

			if (panelActive == -1) {
				PanelDisActivate_Open ();

				//event
				ActivateMenuEvent ();
			}

			panelActive = number;
			menuActive = true;

			//event
			ChancheMenuEvent (number);
		}
	}

	public void DisActivateMenu() {
		if (panelActive > -1) {
			PlayPanelAnim_Close (panelActive);
			PlayWindowAnim_Close (panelActive);

			//event
			DisActivateMenuEvent ();
		}

		panelActive = -1;
		menuActive = false;

		PanelDisActivate_Close ();

		//have some change options save it
		if (needSaveOptions) {
			SaveOptionsPrefs ();

			needSaveOptions = !needSaveOptions;
		}
	}

	public void ActivateWindow(int number) {
		if (windowActive == number) {
			DisActivateWindow ();
		} else {
			if (windowActive > -1) {
				PlayWindowAnim_Close (windowActive);
			}

			PlayWindowAnim_Open (number);

			if (windowActive == -1) {
				WindowDisActivate_Open ();

				//event
				ActivateWindowEvent ();
			}

			windowActive = number;

			//event
			ChancheWindowEvent (number);
		}
	}

	public void DisActivateWindow() {
		if (windowActive > -1) {
			PlayWindowAnim_Close (windowActive);

			//event
			DisActivateWindowEvent ();
		}

		windowActive = -1;

		WindowDisActivate_Close ();

		//have some change options save it
		if (needSaveOptions) {
			SaveOptionsPrefs ();

			needSaveOptions = !needSaveOptions;
		}
	}

	public void ActivateConsoleWindow(int number) {
		if (consoleWindowActive == number) {
			DisActivateConsoleWindow ();
		} else {
			if (consoleWindowActive > -1) {
				PlayWindowAnim_Close (consoleWindowActive);
			}

			PlayWindowAnim_Open (number);

			if (consoleWindowActive == -1) {
				ConsoleWindowDisActivate_Open ();

				//event
				ActivateConsoleWEvent ();
			}

			consoleWindowActive = number;

			//event
			ChancheConsoleWEvent (number);
		}
	}

	public void DisActivateConsoleWindow() {
		if (consoleWindowActive > -1) {
			PlayWindowAnim_Close (consoleWindowActive);
		}

		consoleWindowActive = -1;

		ConsoleWindowDisActivate_Close ();

		//event
		DisActivateConsoleWEvent ();

		//have some change options save it
		if (needSaveOptions) {
			SaveOptionsPrefs ();

			needSaveOptions = !needSaveOptions;
		}
	}

	//advice
	public void ShowWindowStartGame() {
		PlayWindowStartGameAnim_Open ();
		windowStartActive = true;

		if (IsInvoking ("PlayWindowStartGameAnim_Close")) {
			CancelInvoke ("PlayWindowStartGameAnim_Close");
		}
	}

	public void HideWindowStartGame() {
		if (IsInvoking ("PlayWindowStartGameAnim_Close")) {

		} else {
			PlayWindowStartGameAnim_Close ();
			windowStartActive = false;
		}
	}

	//advice
	public void ShowWindowAdvice() {
		PlayWindowAdviceAnim_Open ();

		if (IsInvoking ("PlayWindowAdviceAnim_Close")) {
			CancelInvoke ("PlayWindowAdviceAnim_Close");
		}
	}

	public void HideWindowAdvice() {
		if (IsInvoking ("PlayWindowAdviceAnim_Close")) {

		} else {
			PlayWindowAdviceAnim_Close ();
		}
	}

	public void ShowWindowAdviceAtTime(float timeShow) {
		PlayWindowAdviceAnim_Open ();

		if (IsInvoking ("PlayWindowAdviceAnim_Close")) {
			CancelInvoke ("PlayWindowAdviceAnim_Close");
		}

		Invoke ("PlayWindowAdviceAnim_Close", timeShow);
	}

	//inform
	public virtual void ActivateInformWEvent() {

	}

	public virtual void DisActivateInformWEvent() {

	}

	public void ShowWindowInform() {
		PlayWindowInformAnim_Open ();
	}

	public void HideWindowInform() {
		PlayWindowInformAnim_Close ();
	}

	//console information
	public virtual bool HasSpecKeyText(string st) {
		return false;
	}

	public virtual string ConvertSpecKeyText(string st) {
		if (HasSpecKeyText(st)) {
			// in this override place set convert
		}

		return st;
	}

	public string ConvertSpecTextChar(string st) {
		// text key
		if (HasSpecKeyText (st)) {
			st = ConvertSpecKeyText (st);
		}

		// text color
		if (st.IndexOf ("[c=") >= 0) {
			st = st.Replace ("[c=red]", "<color=red>").Replace ("[c=blue]", "<color=blue>").Replace ("[c=green]", "<color=green>").Replace ("[c]", "</color>");
		}

		return st.Replace ("[n]", "\n").Replace ("[t]", "\t");
	}

	public void ConsoleWinInformationSmol_SetInf(string textHead, string textInformation, string pictureResors = "") {
		consoleWInformSmolTextHead.text = ConvertSpecTextChar(textHead);
		consoleWInformSmolTextInfo.text = ConvertSpecTextChar(textInformation);

		//scrole in up
		RectTransform rectTrans = consoleWInformSmolTextInfo.GetComponent<RectTransform> ();
		if (rectTrans.localPosition.y != 0f) {
			rectTrans.localPosition = new Vector3 (rectTrans.localPosition.x, 0f);
		}

		if (pictureResors == "") {
			consoleWInformSmolPanelImage.SetActive (false);
		} else {
			consoleWInformSmolPanelImage.SetActive (true);
			consoleWInformSmolImage.sprite = Resources.Load<Sprite>(pictureResors);
		}
	}

	public void ConsoleWinInformationBig_SetInf(string textHead, string textInformation, string pictureResors = "") {
		consoleWInformBigTextHead.text = ConvertSpecTextChar(textHead);
		consoleWInformBigTextInfo.text = ConvertSpecTextChar(textInformation);

		//scrole in up
		RectTransform rectTrans = consoleWInformBigTextInfo.GetComponent<RectTransform> ();
		if (rectTrans.localPosition.y != 0f) {
			rectTrans.localPosition = new Vector3 (rectTrans.localPosition.x, 0f);
		}

		if (pictureResors == "") {
			consoleWInformBigPanelImage.SetActive (false);
		} else {
			consoleWInformBigPanelImage.SetActive (true);
			consoleWInformBigImage.sprite = Resources.Load<Sprite>(pictureResors);
		}
	}

	//console YesNo
	public void ConsoleWinYesNo_SetTxt(string val) {
		consoleWInYesNoTextHead.text = ConvertSpecTextChar(val);
	}

	public void ConsoleWinYesNo_SetYesAction(UnityAction val) {
		consoleWInYesNoActinYes.AddListener (val);
	}

	public void ConsoleWinYesNo_ClearYesAction() {
		consoleWInYesNoActinYes.RemoveAllListeners ();
	}

	public void ConsoleWinYesNo_ButtonYes() {
		consoleWInYesNoActinYes.Invoke ();

		DisActivateConsoleWindow ();

		ConsoleWinYesNo_ClearYesAction ();
	}

	public void ConsoleWinYesNo_ButtonNo() {
		DisActivateConsoleWindow ();

		ConsoleWinYesNo_ClearYesAction ();
	}
}
