using UnityEngine;

public class AsteroidManager_SkyRoads : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private float multiplierSpeed = 1f;

	[Header("Spec effects")]
	[SerializeField] private GameObject Explosion;
	[SerializeField] private GameObject ExplosionPlayer;

	[Header("Managers")] [SerializeField] private AutoMoveRigidBody moveManager;
	[SerializeField] private AutoRotateRigidBody rotateManager;
	[SerializeField] private LevelManager_SkyRoads levelManager;
	
	private void Start()
	{
		Init();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			levelManager.GameOver(this);
		}
		else
		{
			return;
		}
		
		Destroy(gameObject);
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Floor"))
		{
			levelManager.AddPointsForAsteroid(this);
		}
		else
		{
			return;
		}

		Destroy(gameObject);
	}
	
	private void Init()
	{
		if (!levelManager)
		{
			levelManager = LevelManager_SkyRoads.Instance;

			if (!levelManager)
			{
				Debug.Log("Cannot find LevelManager!!");
			}
		}
	}

	public void SetSpeed(float val)
	{
		moveManager.Speed = val * multiplierSpeed;
	}
}
