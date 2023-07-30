using UnityEngine;

[AddComponentMenu("Utility/Look At")]
public class LookAt : ExtendedCustomMonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private Transform followTarget;
	[SerializeField] private Vector3 targetOffset = Vector3.zero;
	[SerializeField] private float moveSpeed = 0f;

	private void LateUpdate()
	{
		if (!canControl) return;
		if (!followTarget) return;
		
		Quaternion targetRotate = Quaternion.LookRotation((followTarget.position + targetOffset) - myTransform.position);
		Quaternion myRotate = myTransform.rotation;

		if ((myRotate.eulerAngles - targetRotate.eulerAngles).magnitude > 0.1f)
		{
			if (moveSpeed == 0)
			{
				myTransform.LookAt(followTarget.position + targetOffset);
			}
			else
			{
				myTransform.rotation = Quaternion.Slerp(myRotate, targetRotate, moveSpeed * Time.deltaTime);
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
