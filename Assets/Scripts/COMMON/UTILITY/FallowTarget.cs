using UnityEngine;

[AddComponentMenu("Utility/Fallow Target")]
public class FallowTarget : ExtendedCustomMonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private Transform followTarget;
	[SerializeField] private Vector3 targetOffset = Vector3.zero;
	[SerializeField] private float moveSpeed = 2f;

	private void LateUpdate()
	{
		if (!canControl) return;
		if (!followTarget) return;
		
		if ((myTransform.position - (followTarget.position + targetOffset)).magnitude > 0.1f)
		{
			if (moveSpeed == 0)
			{
				myTransform.position = followTarget.position + targetOffset;
			}
			else
			{
				myTransform.position = Vector3.Lerp(myTransform.position, followTarget.position + targetOffset,
					moveSpeed * Time.deltaTime);
			}
		}
	}
	
	public override void Init()
	{
		base.Init();

		canControl = true;
	}

	public Transform FollowTarget
	{
		get => followTarget;
		set => followTarget = value;
	}

	public Vector3 TargetOffset
	{
		get => targetOffset;
		set => targetOffset = value;
	}

	public float MoveSpeed
	{
		get => moveSpeed;
		set => moveSpeed = value;
	}
}
