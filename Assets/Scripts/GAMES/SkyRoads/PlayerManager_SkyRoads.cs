using UnityEngine;
using System.Collections;

public class PlayerManager_SkyRoads : BasePlayerManager {

	public UserManager_SkyRoads DataManager_New;

	[System.NonSerialized]
	public static PlayerManager_SkyRoads Instance;

	// override
	public override void Awake ()
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

	public void Start () {
		if (!didInit)
			Init ();
	}

	public override void Init ()
	{
		// cache ref to our user manager
		DataManager_New= gameObject.GetComponent<UserManager_SkyRoads>();

		if(DataManager_New==null)
			DataManager_New= gameObject.AddComponent<UserManager_SkyRoads>();

		// do play init things in this function
		didInit= true;
	}

	public override void GameFinished ()
	{
		DataManager_New.SetIsFinished (true);
	}

	public override void GameStart ()
	{
		DataManager_New.SetIsFinished (false);
	}

	//
	public void SetPlayerDefaultData() {
		if (!didInit)
			Init ();

		DataManager_New.GetDefaultData ();
	}

	public void SetPlayerName(string stVal) {
		DataManager_New.SetName (stVal);
	}

	public string GetPlayerName() {
		return DataManager_New.GetName ();
	}

	public void SetLevel(int itVal) {
		DataManager_New.SetLevel (itVal);
	}

	public int GetLevel() {
		return DataManager_New.GetLevel ();
	}

	public void SetScore(int itVal) {
		DataManager_New.SetScore (itVal);
	}

	public int GetScore() {
		return DataManager_New.GetScore ();
	}

	//============

	public int GetScoreBest()
	{
		return DataManager_New.GetScoreBest ();
	}

	public void SetScoreBest(int num)
	{
		DataManager_New.SetScoreBest (num);
	}

	public int GetTimeBest()
	{
		return DataManager_New.GetTimeBest ();
	}

	public void SetTimeBest(int num)
	{
		DataManager_New.SetTimeBest (num);
	}

	public int GetAsteroidBest()
	{
		return DataManager_New.GetAsteroidBest ();
	}

	public void SetAsteroidBest(int num)
	{
		DataManager_New.SetAsteroidBest (num);
	}

	//============

	public void LoadPrivateDataPlayer() {
		DataManager_New.LoadPrivateDataPlayer ();
	}

	public void SavePrivateDataPlayer() {
		DataManager_New.SavePrivateDataPlayer ();
	}

	public void ClosePrivateDataPlayerFile() {
		DataManager_New.ClosePlayerDateFile ();
	}
}
