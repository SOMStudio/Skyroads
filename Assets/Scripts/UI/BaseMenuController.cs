﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BaseMenuController : MonoBehaviour
{
	public bool didInit;

	[Header("Base Settings")] public string gamePrefsName = "DefaultGame";
	
	[SerializeField] private float audioSFXSliderValue;
	[SerializeField] private Slider audioSFXSlider;
	[SerializeField] private float audioMusicSliderValue;

	[SerializeField] private Slider audioMusicSlider;
	
	[SerializeField] private float graphicsSliderValue;
	[SerializeField] private Slider graphicsSlider;
	[SerializeField] private int graphicsDefaultValue = -1;

	private int detailLevels = 6;
	private bool needSaveOptions;

	[Header("Main window list")]
	[SerializeField] private AnimationOpenClose[] panelAnimations;

	[SerializeField] private AnimationOpenClose[] windowAnimations;
	[SerializeField] private bool[] accessToPanel;

	[Header("DisActivate window")]
	[SerializeField] private AnimationOpenClose panelDisActivateAnimation;

	[SerializeField] private AnimationOpenClose windowDisActivateAnimation;
	[SerializeField] private AnimationOpenClose consoleWindowDisActivateAnimation;

	[Header("Menu Data")]
	[SerializeField] protected bool menuActive;
	[SerializeField] protected int panelActive = -1;
	[SerializeField] protected int windowActive = -1;
	[SerializeField] protected int consoleWindowActive = -1;

	[Header("StartGame window")]
	[SerializeField] protected bool windowStartActive = true;

	[SerializeField] private AnimationOpenClose windowStartGameAnimation;

	[Header("Advice window")]
	[SerializeField] private AnimationOpenClose windowAdviceAnimation;

	[SerializeField] private Text windowAdviceText;

	[Header("Inform window")]
	[SerializeField] private AnimationOpenClose windowInformAnimation;

	[SerializeField] private Text[] windowInformTextList;

	[Header("Console windows")]
	[SerializeField] private Text consoleWInformSmolTextHead;

	[SerializeField] private Text consoleWInformSmolTextInfo;
	[SerializeField] private GameObject consoleWInformSmolPanelImage;
	[SerializeField] private Image consoleWInformSmolImage;

	
	[SerializeField] private Text consoleWInformBigTextHead;
	[SerializeField] private Text consoleWInformBigTextInfo;
	[SerializeField] private GameObject consoleWInformBigPanelImage;

	[SerializeField] private Image consoleWInformBigImage;
	
	[SerializeField] private Text consoleWInYesNoTextHead;
	private readonly UnityEvent consoleWInYesNoActionYes = new UnityEvent();
	
	private void Start()
	{
		RestoreOptionsPref();
	}

	protected virtual void RestoreOptionsPref()
	{
		string stKey;
		
		stKey = $"{gamePrefsName}_SFXVol";
		if (PlayerPrefs.HasKey(stKey))
		{
			audioSFXSliderValue = PlayerPrefs.GetFloat(stKey);
		}
		else
		{
			audioSFXSliderValue = 1;
		}

		stKey = $"{gamePrefsName}_MusicVol";
		if (PlayerPrefs.HasKey(stKey))
		{
			audioMusicSliderValue = PlayerPrefs.GetFloat(stKey);
		}
		else
		{
			audioMusicSliderValue = 1;
		}

		stKey = $"{gamePrefsName}_GraphicsDetail";
		if (PlayerPrefs.HasKey(stKey))
		{
			graphicsSliderValue = PlayerPrefs.GetFloat(stKey);
		}
		else
		{
			if (graphicsDefaultValue == -1)
			{
				string[] names = QualitySettings.names;
				detailLevels = names.Length;

				switch (Application.platform)
				{
					case RuntimePlatform.Android:
					case RuntimePlatform.IPhonePlayer:
						graphicsSliderValue = 0;
						break;
					default:
						graphicsSliderValue = detailLevels;
						break;
				}
			}
			else
			{
				graphicsSliderValue = graphicsDefaultValue;
			}
		}

		Debug.Log("quality=" + graphicsSliderValue);
		
		QualitySettings.SetQualityLevel((int)graphicsSliderValue, true);
		
		if (audioSFXSlider != null)
		{
			audioSFXSlider.value = audioSFXSliderValue;
		}

		if (audioMusicSlider != null)
		{
			audioMusicSlider.value = audioMusicSliderValue;
		}

		if (graphicsSlider != null)
		{
			string[] namesQlt = QualitySettings.names;
			graphicsSlider.maxValue = namesQlt.Length - 1;

			graphicsSlider.value = graphicsSliderValue;
		}

		windowAdviceText.text = "";

		didInit = true;
	}

	protected virtual void SaveOptionsPrefs()
	{
		string stKey;

		stKey = $"{gamePrefsName}_SFXVol";
		PlayerPrefs.SetFloat(stKey, audioSFXSliderValue);
		stKey = $"{gamePrefsName}_MusicVol";
		PlayerPrefs.SetFloat(stKey, audioMusicSliderValue);
		stKey = $"{gamePrefsName}_GraphicsDetail";
		PlayerPrefs.SetFloat(stKey, graphicsSliderValue);
		
		QualitySettings.SetQualityLevel((int)graphicsSliderValue, true);
	}

	public void ChangeSFXVal(float val)
	{
		audioSFXSliderValue = val;

		if (didInit)
		{
			SaveOptionsPrefs();
		}
	}

	public void ChangeMusicVal(float val)
	{
		audioMusicSliderValue = val;

		if (didInit)
		{
			SaveOptionsPrefs();
		}
	}

	public void ChangeGraficVal(float val)
	{
		graphicsSliderValue = val;

		if (didInit)
		{
			SaveOptionsPrefs();
		}
	}

	protected virtual void ExitGame()
	{
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	#region Animations

	//panels
	private void PlayPanelAnim_Open(int number)
	{
		if (number < panelAnimations.Length)
		{
			AnimationOpenClose activeA = panelAnimations[number];

			if (activeA)
			{
				if (!activeA.IsOpen())
				{
					activeA.Open();
				}
			}
		}
	}

	private void PlayPanelAnim_Close(int number)
	{
		if (number < panelAnimations.Length)
		{
			AnimationOpenClose activeA = panelAnimations[number];

			if (activeA)
			{
				if (activeA.IsOpen())
				{
					activeA.Close();
				}
			}
		}
	}

	private void PlayPanelAnim_Hide(int number)
	{
		if (number < panelAnimations.Length)
		{
			AnimationOpenClose activeA = panelAnimations[number];

			if (activeA)
			{
				activeA.Hide();
			}
		}
	}

	private void PlayPanelAnim_Show(int number)
	{
		if (number < panelAnimations.Length)
		{
			AnimationOpenClose activeA = panelAnimations[number];

			if (activeA)
			{
				activeA.Show();
			}
		}
	}

	private void PlayWindowAnim_Open(int number)
	{
		if (number < windowAnimations.Length)
		{
			AnimationOpenClose activeA = windowAnimations[number];

			if (activeA)
			{
				if (!activeA.IsOpen())
				{
					activeA.Open();
				}
			}
		}
	}

	private void PlayWindowAnim_Close(int number)
	{
		if (number < windowAnimations.Length)
		{
			AnimationOpenClose activeA = windowAnimations[number];

			if (activeA)
			{
				if (activeA.IsOpen())
				{
					activeA.Close();
				}
			}
		}
	}

	private void PanelDisActivate_Open()
	{
		if (panelDisActivateAnimation)
		{
			panelDisActivateAnimation.Open();
		}
	}

	private void PanelDisActivate_Close()
	{
		if (panelDisActivateAnimation)
		{
			panelDisActivateAnimation.Close();
		}
	}

	//window disActivate
	private void WindowDisActivate_Open()
	{
		if (windowDisActivateAnimation)
		{
			windowDisActivateAnimation.Open();
		}
	}

	private void WindowDisActivate_Close()
	{
		if (windowDisActivateAnimation)
		{
			windowDisActivateAnimation.Close();
		}
	}

	//consoleWindows disActivate
	private void ConsoleWindowDisActivate_Open()
	{
		if (consoleWindowDisActivateAnimation)
		{
			consoleWindowDisActivateAnimation.Open();
		}
	}

	private void ConsoleWindowDisActivate_Close()
	{
		if (consoleWindowDisActivateAnimation)
		{
			consoleWindowDisActivateAnimation.Close();
		}
	}

	//startGame window
	private void PlayWindowStartGameAnim_Open()
	{
		if (windowStartGameAnimation)
		{
			AnimationOpenClose activeA = windowStartGameAnimation;

			if (activeA)
			{
				if (!activeA.IsOpen())
				{
					activeA.Open();
				}
			}
		}
	}

	private void PlayWindowStartGameAnim_Close()
	{
		if (windowStartGameAnimation)
		{
			AnimationOpenClose activeA = windowStartGameAnimation;

			if (activeA)
			{
				if (activeA.IsOpen())
				{
					activeA.Close();
				}
			}
		}
	}

	//advice window
	private void PlayWindowAdviceAnim_Open()
	{
		if (windowAdviceAnimation)
		{
			AnimationOpenClose activeA = windowAdviceAnimation;

			if (activeA)
			{
				if (!activeA.IsOpen())
				{
					activeA.Open();

					ActivateAdviceWEvent();
				}
			}
		}
	}

	private void PlayWindowAdviceAnim_Close()
	{
		if (windowAdviceAnimation)
		{
			AnimationOpenClose activeA = windowAdviceAnimation;

			if (activeA)
			{
				if (activeA.IsOpen())
				{
					activeA.Close();

					DisActivateAdviceWEvent();

					Invoke("WindowAdviceClearText", 0.2f);
				}
			}
		}
	}

	//inform window
	private void PlayWindowInformAnim_Open()
	{
		if (windowInformAnimation)
		{
			AnimationOpenClose activeA = windowInformAnimation;

			if (activeA)
			{
				if (!activeA.IsOpen())
				{
					activeA.Open();

					ActivateInformWEvent();
				}
			}
		}
	}

	private void PlayWindowInformAnim_Close()
	{
		if (windowInformAnimation)
		{
			AnimationOpenClose activeA = windowInformAnimation;

			if (activeA)
			{
				if (activeA.IsOpen())
				{
					activeA.Close();

					DisActivateInformWEvent();
				}
			}
		}
	}

	#endregion

	#region Events

	// Events for use
	protected virtual void ActivateMenuEvent()
	{

	}

	protected virtual void DisActivateMenuEvent()
	{

	}

	protected virtual void ChangeMenuEvent(int number)
	{

	}

	protected virtual void ActivateWindowEvent()
	{

	}

	protected virtual void DisActivateWindowEvent()
	{

	}

	protected virtual void ChangeWindowEvent(int number)
	{

	}

	protected virtual void ActivateConsoleWEvent()
	{

	}

	protected virtual void DisActivateConsoleWEvent()
	{

	}

	protected virtual void ChangeConsoleWEvent(int number)
	{

	}

	protected virtual void ActivateAdviceWEvent()
	{

	}

	protected virtual void DisActivateAdviceWEvent()
	{

	}

	protected virtual void ActivateInformWEvent()
	{

	}

	protected virtual void DisActivateInformWEvent()
	{

	}
	#endregion

	#region Panels
	public int PanelActive => panelActive;

	protected void ShowPanelMenu()
	{
		for (int i = 0; i < accessToPanel.Length; i++)
		{
			if (accessToPanel[i])
			{
				PlayPanelAnim_Show(i);
			}
		}
	}

	protected void HidePanelMenu()
	{
		for (int i = 0; i < accessToPanel.Length; i++)
		{
			if (accessToPanel[i])
			{
				PlayPanelAnim_Hide(i);
			}
		}
	}

	public void ShowMenu()
	{
		ShowPanelMenu();
	}

	public void HideMenu()
	{
		DisActivateMenu();

		HidePanelMenu();
	}

	public void ActivateMenu(int number)
	{
		if (panelActive == number)
		{
			DisActivateMenu();
		}
		else
		{
			if (panelActive > -1)
			{
				PlayPanelAnim_Close(panelActive);
				PlayWindowAnim_Close(panelActive);
			}

			PlayPanelAnim_Open(number);
			PlayWindowAnim_Open(number);

			if (panelActive == -1)
			{
				PanelDisActivate_Open();
				
				ActivateMenuEvent();
			}

			panelActive = number;
			menuActive = true;
			
			ChangeMenuEvent(number);
		}
	}

	public void DisActivateMenu()
	{
		if (panelActive > -1)
		{
			PlayPanelAnim_Close(panelActive);
			PlayWindowAnim_Close(panelActive);
			
			DisActivateMenuEvent();
		}

		panelActive = -1;
		menuActive = false;

		PanelDisActivate_Close();
		
		if (needSaveOptions)
		{
			SaveOptionsPrefs();

			needSaveOptions = !needSaveOptions;
		}
	}
	#endregion

	#region Windows
	public int WindowActive
	{
		get { return windowActive; }
	}

	public void ActivateWindow(int number)
	{
		if (windowActive == number)
		{
			DisActivateWindow();
		}
		else
		{
			if (windowActive > -1)
			{
				PlayWindowAnim_Close(windowActive);
			}

			PlayWindowAnim_Open(number);

			if (windowActive == -1)
			{
				WindowDisActivate_Open();
				
				ActivateWindowEvent();
			}

			windowActive = number;
			
			ChangeWindowEvent(number);
		}
	}

	public void DisActivateWindow()
	{
		if (windowActive > -1)
		{
			PlayWindowAnim_Close(windowActive);
			
			DisActivateWindowEvent();
		}

		windowActive = -1;

		WindowDisActivate_Close();
		
		if (needSaveOptions)
		{
			SaveOptionsPrefs();

			needSaveOptions = !needSaveOptions;
		}
	}
	#endregion

	#region ConsoleWindows
	public int ConsoleWindowActive
	{
		get { return consoleWindowActive; }
	}

	public void ActivateConsoleWindow(int number)
	{
		if (consoleWindowActive == number)
		{
			DisActivateConsoleWindow();
		}
		else
		{
			if (consoleWindowActive > -1)
			{
				PlayWindowAnim_Close(consoleWindowActive);
			}

			PlayWindowAnim_Open(number);

			if (consoleWindowActive == -1)
			{
				ConsoleWindowDisActivate_Open();
				
				ActivateConsoleWEvent();
			}

			consoleWindowActive = number;
			
			ChangeConsoleWEvent(number);
		}
	}

	public void DisActivateConsoleWindow()
	{
		if (consoleWindowActive > -1)
		{
			PlayWindowAnim_Close(consoleWindowActive);
		}

		consoleWindowActive = -1;

		ConsoleWindowDisActivate_Close();
		
		DisActivateConsoleWEvent();
		
		if (needSaveOptions)
		{
			SaveOptionsPrefs();

			needSaveOptions = !needSaveOptions;
		}
	}

	public void ConsoleWinInformationSmall_SetInf(string textHead, string textInformation, string pictureResource = "")
	{
		consoleWInformSmolTextHead.text = ConvertSpecTextChar(textHead);
		consoleWInformSmolTextInfo.text = ConvertSpecTextChar(textInformation);
		
		RectTransform rectTrans = consoleWInformSmolTextInfo.GetComponent<RectTransform>();
		if (rectTrans.localPosition.y != 0f)
		{
			rectTrans.localPosition = new Vector3(rectTrans.localPosition.x, 0f);
		}

		if (pictureResource == "")
		{
			consoleWInformSmolPanelImage.SetActive(false);
		}
		else
		{
			consoleWInformSmolPanelImage.SetActive(true);
			consoleWInformSmolImage.sprite = Resources.Load<Sprite>(pictureResource);
		}
	}

	public void ConsoleWinInformationBig_SetInf(string textHead, string textInformation, string pictureResource = "")
	{
		consoleWInformBigTextHead.text = ConvertSpecTextChar(textHead);
		consoleWInformBigTextInfo.text = ConvertSpecTextChar(textInformation);
		
		RectTransform rectTrans = consoleWInformBigTextInfo.GetComponent<RectTransform>();
		if (rectTrans.localPosition.y != 0f)
		{
			rectTrans.localPosition = new Vector3(rectTrans.localPosition.x, 0f);
		}

		if (pictureResource == "")
		{
			consoleWInformBigPanelImage.SetActive(false);
		}
		else
		{
			consoleWInformBigPanelImage.SetActive(true);
			consoleWInformBigImage.sprite = Resources.Load<Sprite>(pictureResource);
		}
	}
	#endregion

	#region StartGame
	public void ShowWindowStartGame()
	{
		PlayWindowStartGameAnim_Open();
		windowStartActive = true;

		if (IsInvoking(nameof(PlayWindowStartGameAnim_Close)))
		{
			CancelInvoke(nameof(PlayWindowStartGameAnim_Close));
		}
	}

	public void HideWindowStartGame()
	{
		if (IsInvoking(nameof(PlayWindowStartGameAnim_Close)))
		{

		}
		else
		{
			PlayWindowStartGameAnim_Close();
			windowStartActive = false;
		}
	}
	#endregion

	#region Advice
	public void ShowWindowAdvice()
	{
		PlayWindowAdviceAnim_Open();

		if (IsInvoking(nameof(PlayWindowAdviceAnim_Close)))
		{
			CancelInvoke(nameof(PlayWindowAdviceAnim_Close));
		}
	}

	public void HideWindowAdvice()
	{
		if (IsInvoking(nameof(PlayWindowAdviceAnim_Close)))
		{

		}
		else
		{
			PlayWindowAdviceAnim_Close();
		}
	}

	public void ShowWindowAdviceAtTime(float timeShow)
	{
		PlayWindowAdviceAnim_Open();

		if (IsInvoking(nameof(PlayWindowAdviceAnim_Close)))
		{
			CancelInvoke(nameof(PlayWindowAdviceAnim_Close));
		}

		Invoke(nameof(PlayWindowAdviceAnim_Close), timeShow);
	}

	public void WindowAdviceSetText(string stAdvice)
	{
		if (windowAdviceText)
		{
			string stText = windowAdviceText.text;
			string[] stRes = stText.Split(new[] { "\n", "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);

			if (stRes.Length > 2)
			{
				for (int i = 1; i < stRes.Length; i++)
				{
					if (i == 1)
					{
						windowAdviceText.text = stRes[i];
					}
					else
					{
						windowAdviceText.text = $"{windowAdviceText.text}\n{stRes[i]}";
					}
				}
			}

			if (stRes.Length == 0)
			{
				windowAdviceText.text = ConvertSpecTextChar(stAdvice);
			}
			else
			{
				windowAdviceText.text = $"{windowAdviceText.text}\n{ConvertSpecTextChar(stAdvice)}";
			}
		}
	}

	public void WindowAdviceClearText()
	{
		windowAdviceText.text = "";
	}
	#endregion

	#region inform
	public void ShowWindowInform()
	{
		PlayWindowInformAnim_Open();
	}

	public void HideWindowInform()
	{
		PlayWindowInformAnim_Close();
	}

	public void WindowInformSetText(string stAdvice, int numText)
	{
		if (numText < windowInformTextList.Length)
		{
			if (windowInformTextList[numText])
			{
				windowInformTextList[numText].text = ConvertSpecTextChar(stAdvice);
			}
		}
	}

	public void WindowInformSetText_1(string stAdvice)
	{
		WindowInformSetText(stAdvice, 0);
	}

	public void WindowInformSetText_2(string stAdvice)
	{
		WindowInformSetText(stAdvice, 1);
	}

	public void WindowInformSetText_3(string stAdvice)
	{
		WindowInformSetText(stAdvice, 2);
	}
	#endregion

	#region ConsoleYesNo
	protected void ConsoleWinYesNo_SetTxt(string val)
	{
		consoleWInYesNoTextHead.text = ConvertSpecTextChar(val);
	}

	protected void ConsoleWinYesNo_SetYesAction(UnityAction val)
	{
		consoleWInYesNoActionYes.AddListener(val);
	}

	protected void ConsoleWinYesNo_ClearYesAction()
	{
		consoleWInYesNoActionYes.RemoveAllListeners();
	}

	protected void ConsoleWinYesNo_ButtonYes()
	{
		consoleWInYesNoActionYes.Invoke();

		DisActivateConsoleWindow();

		ConsoleWinYesNo_ClearYesAction();
	}

	protected void ConsoleWinYesNo_ButtonNo()
	{
		DisActivateConsoleWindow();

		ConsoleWinYesNo_ClearYesAction();
	}
	#endregion

	#region convertSpecText
	protected virtual bool HasSpecKeyText(string st)
	{
		return false;
	}

	protected virtual string ConvertSpecKeyText(string st)
	{
		if (HasSpecKeyText(st))
		{
			
		}

		return st;
	}

	public string ConvertSpecTextChar(string st)
	{
		if (HasSpecKeyText(st))
		{
			st = ConvertSpecKeyText(st);
		}
		
		if (st.IndexOf("[c=") >= 0)
		{
			st = st.Replace("[c=red]", "<color=red>").Replace("[c=blue]", "<color=blue>")
				.Replace("[c=green]", "<color=green>").Replace("[c]", "</color>");
		}

		return st.Replace("[n]", "\n").Replace("[t]", "\t");
	}
	#endregion
}
