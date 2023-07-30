using UnityEngine;

[AddComponentMenu("Common/Timer class")]
public class TimerClass : ScriptableObject
{
	public bool isTimerRunning;

	private float timeElapsed;
	private float currentTime;
	private float lastTime;
	private const float TimeScaleFactor = 1.0f;

	private string timeString;
	private string hour;
	private string minutes;
	private string seconds;
	private string mills;

	private int aHour;
	private int aMinute;
	private int aSecond;
	private int aMillis;
	private int tmp;
	private int aTime;

	private GameObject callback;

	public void UpdateTimer()
	{
		timeElapsed = Mathf.Abs(Time.realtimeSinceStartup - lastTime);
		
		if (isTimerRunning)
		{
			currentTime += timeElapsed * TimeScaleFactor;
		}
		
		lastTime = Time.realtimeSinceStartup;
	}
	
	public void StartTimer()
	{
		isTimerRunning = true;
		lastTime = Time.realtimeSinceStartup;
	}
	
	public void StopTimer()
	{
		isTimerRunning = false;
		
		UpdateTimer();
	}
	
	public void ResetTimer()
	{
		timeElapsed = 0.0f;
		lastTime = 0.0f;
		currentTime = 0.0f;
		lastTime = Time.realtimeSinceStartup;
		
		UpdateTimer();
	}
	
	public string GetFormattedTime(float val)
	{
		aHour = (int)val / 3600;
		aHour = aHour % 24;
		
		aMinute = (int)val / 60;
		aMinute = aMinute % 60;
		
		aSecond = (int)val % 60;
		
		aMillis = (int)(val * 100) % 100;
		
		tmp = aSecond;
		seconds = tmp.ToString();
		if (seconds.Length < 2) seconds = $"0{seconds}";

		tmp = aMinute;
		minutes = tmp.ToString();
		if (minutes.Length < 2) minutes = $"0{minutes}";

		tmp = aHour;
		hour = tmp.ToString();
		if (hour.Length < 2) hour = $"0{hour}";

		tmp = aMillis;
		mills = tmp.ToString();
		if (mills.Length < 2) mills = $"0{mills}";
		
		timeString = $"{minutes}:{seconds}:{mills}";

		return timeString;
	}
	
	public string GetFormattedTime()
	{
		UpdateTimer();

		return GetFormattedTime(currentTime);
	}
	
	public int GetTime()
	{
		return (int)(currentTime);
	}
}