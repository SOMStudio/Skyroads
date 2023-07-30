using UnityEngine;

[AddComponentMenu("Base/Player Manager")]
public class BasePlayerManager : MonoBehaviour
{
	[SerializeField] protected bool didInit = false;
	
	[SerializeField] protected BaseUserManager DataManager;
	
	private void Awake()
	{
		Init();
	}
	
	public virtual void Init()
	{
		if (!DataManager)
		{
			DataManager = gameObject.GetComponent<BaseUserManager>();

			if (!DataManager)
				DataManager = gameObject.AddComponent<BaseUserManager>();
		}
		
		DataManager.GetDefaultData();

		didInit = true;
	}

	public BaseUserManager GetDataManager()
	{
		return DataManager;
	}

	public virtual void GameFinished()
	{
		DataManager.SetIsFinished(true);
	}

	public virtual void GameStart()
	{
		DataManager.SetIsFinished(false);
	}
}
