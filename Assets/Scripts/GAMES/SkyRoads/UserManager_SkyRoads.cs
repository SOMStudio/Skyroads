using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class UserManager_SkyRoads : BaseUserManager {
	
	private int scoreBest;
	private int timeBest;
	private int asteroidBest;

	// main logic
	public override void GetDefaultData()
	{
		base.GetDefaultData ();

		scoreBest = 0;
		timeBest = 0;
		asteroidBest = 0;
	}

	public int GetScoreBest()
	{
		return scoreBest;
	}

	public void SetScoreBest(int num)
	{
		scoreBest = num;
	}

	public int GetTimeBest()
	{
		return timeBest;
	}

	public void SetTimeBest(int num)
	{
		timeBest = num;
	}

	public int GetAsteroidBest()
	{
		return asteroidBest;
	}

	public void SetAsteroidBest(int num)
	{
		asteroidBest = num;
	}

	//save data
	private FileStream filePlayerData;

	private void OpenPlayerDataFileForWrite() {
		filePlayerData = File.Create(Application.persistentDataPath + "/playerinfo.dat");
	}

	private void OpenPlayerDataFileForRead() {
		filePlayerData = File.Open (Application.persistentDataPath + "/playerinfo.dat", FileMode.Open);
	}

	public void ClosePlayerDateFile() {
		filePlayerData.Close ();
	}

	/// <summary>
	/// save player data in file with cripting, not use for Web-application (we can't write file)
	/// </summary>
	public void SavePrivateDataPlayer()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/playerinfo.dat");

		PlayerData_SkyRoads data = new PlayerData_SkyRoads();
		data.playerName = playerName;

		data.score = GetScore();
		data.level = GetLevel();
		data.helth = GetHealth();

		data.scoreBest = scoreBest;
		data.timeBest = timeBest;
		data.asteroidBest = asteroidBest;

		bf.Serialize(file, data);
		file.Close();
	}

	/// <summary>
	/// restore player data from cripting file.
	/// </summary>
	public void LoadPrivateDataPlayer()
	{
		if (File.Exists (Application.persistentDataPath + "/playerinfo.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/playerinfo.dat", FileMode.Open);

			PlayerData_SkyRoads data = (PlayerData_SkyRoads)bf.Deserialize (file);
			playerName = data.playerName;

			SetScore (data.score);
			SetLevel (data.level);
			SetHealth (data.helth);

			scoreBest = data.scoreBest;
			timeBest = data.timeBest;
			asteroidBest = data.asteroidBest;

			file.Close ();
		} else {
			GetDefaultData ();
		}
	}
}

[System.Serializable]
class PlayerData_SkyRoads
{
	public string playerName;

	public int score;
	public int level;
	public int helth;

	public int scoreBest;
	public int timeBest;
	public int asteroidBest;
}
