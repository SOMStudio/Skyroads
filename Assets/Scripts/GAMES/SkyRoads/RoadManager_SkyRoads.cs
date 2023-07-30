using UnityEngine;

public class RoadManager_SkyRoads : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private float multiplierSpeed = 1f;

	[Header("Managers")]
	[SerializeField] private BGScroller bgScroller;
	
	public void SetSpeed(float val)
	{
		bgScroller.ScrollSpeed = val * multiplierSpeed;
	}
}
