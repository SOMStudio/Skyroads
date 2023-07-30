using UnityEngine;

[AddComponentMenu("Utility/Auto rotate rigid body")]
public sealed class AutoRotateRigidBody : ExtendedCustomMonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private Vector3 direction = Vector3.forward;
	[SerializeField] private float speed = 1f;
	
	public override void Init()
	{
		base.Init();

		canControl = true;
		
		UpdateVelocity();
	}

	public void UpdateVelocity()
	{
		if (canControl)
		{
			myBody.angularVelocity = direction * speed;
		}
	}

	public Vector3 Direction
	{
		get => direction;
		set => direction = value;
	}

	public float Speed
	{
		get => speed;
		set
		{
			speed = value;
			UpdateVelocity();
		}
	}
}
