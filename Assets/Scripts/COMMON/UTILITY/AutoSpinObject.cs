using UnityEngine;

[AddComponentMenu("Utility/Auto spin object")]
public sealed class AutoSpinObject : ExtendedCustomMonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private Vector3 spinVector = new Vector3(1, 0, 0);

	private void LateUpdate()
	{
		if (canControl)
		{
			myTransform.Rotate(spinVector * Time.deltaTime);
		}
	}

	public override void Init()
	{
		base.Init();

		canControl = true;
	}

	public Vector3 SpinVector
	{
		get => spinVector;
		set => spinVector = value;
	}
}
