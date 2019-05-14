using UnityEngine;

public class PlayerManager_SkyRoads : BasePlayerManager {

	public UserManager_SkyRoads DataManager_SkyRoads {
		get { return (UserManager_SkyRoads)DataManager; }
		set { DataManager = value; }
	}

	[System.NonSerialized]
	public static PlayerManager_SkyRoads Instance;

	// main event
	void Start () {
		// keep this object alive
		DontDestroyOnLoad (this.gameObject);
	}

	// main logic
	public override void Init ()
	{
		// activate instance
		if (Instance == null) {
			Instance = this;

			if (!didInit)
				Init ();
		} else if (Instance != this) {
			Destroy (gameObject);
		}
	}

	public override void GameFinished ()
	{
		DataManager_SkyRoads.SetIsFinished (true);
	}

	public override void GameStart ()
	{
		DataManager_SkyRoads.SetIsFinished (false);
	}

	//
	public void SetPlayerDefaultData() {
		if (!didInit)
			Init ();

		DataManager_SkyRoads.GetDefaultData ();
	}

	public void SetPlayerName(string stVal) {
		DataManager_SkyRoads.SetName (stVal);
	}

	public string GetPlayerName() {
		return DataManager_SkyRoads.GetName ();
	}

	public void SetLevel(int itVal) {
		DataManager_SkyRoads.SetLevel (itVal);
	}

	public int GetLevel() {
		return DataManager_SkyRoads.GetLevel ();
	}

	public void SetScore(int itVal) {
		DataManager_SkyRoads.SetScore (itVal);
	}

	public int GetScore() {
		return DataManager_SkyRoads.GetScore ();
	}

	//============

	public int GetScoreBest()
	{
		return DataManager_SkyRoads.GetScoreBest ();
	}

	public void SetScoreBest(int num)
	{
		DataManager_SkyRoads.SetScoreBest (num);
	}

	public int GetTimeBest()
	{
		return DataManager_SkyRoads.GetTimeBest ();
	}

	public void SetTimeBest(int num)
	{
		DataManager_SkyRoads.SetTimeBest (num);
	}

	public int GetAsteroidBest()
	{
		return DataManager_SkyRoads.GetAsteroidBest ();
	}

	public void SetAsteroidBest(int num)
	{
		DataManager_SkyRoads.SetAsteroidBest (num);
	}

	//============

	public void LoadPrivateDataPlayer() {
		DataManager_SkyRoads.LoadPrivateDataPlayer ();
	}

	public void SavePrivateDataPlayer() {
		DataManager_SkyRoads.SavePrivateDataPlayer ();
	}

	public void ClosePrivateDataPlayerFile() {
		DataManager_SkyRoads.ClosePlayerDateFile ();
	}
}
