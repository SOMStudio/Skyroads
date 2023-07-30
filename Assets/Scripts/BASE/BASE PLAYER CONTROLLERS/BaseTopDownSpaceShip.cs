using UnityEngine;

[AddComponentMenu("Base/Vehicles/Top Down Space Ship")]
public class BaseTopDownSpaceShip : ExtendedCustomMonoBehaviour
{
	[Header("Move settings")]
	[SerializeField] protected float moveXSpeed = 5f;
	[SerializeField] protected float moveZSpeed = 5f;

	[Header("Limit settings")]
	[SerializeField] protected float limitX = 15f;
	[SerializeField] protected float limitZ = 15f;
	[SerializeField] protected float rotateZAmount = 15f;
	protected float originZ;

	[Header("Technic value")]
	[SerializeField] protected float horizontal_input;
	[SerializeField] protected float vertical_input;

	[Header("Technic reference")]
	[SerializeField] protected Keyboard_Input default_input;
	
	private void Update()
	{
		UpdateShip();
	}

	public override void Init()
	{
		base.Init();

		didInit = false;
		
		default_input = myGO.AddComponent<Keyboard_Input>();
		
		originZ = myTransform.localPosition.z;
		
		didInit = true;
	}
	
	protected virtual void GameStart()
	{
		canControl = true;
	}
	
	protected virtual void GetInput()
	{
		horizontal_input = default_input.GetHorizontal();
		vertical_input = default_input.GetVertical();
	}
	
	protected virtual void UpdateShip()
	{
		if (!didInit)
			return;
		
		if (!canControl)
			return;

		GetInput();
		
		float moveXAmount = horizontal_input * Time.deltaTime * moveXSpeed;
		float moveZAmount = vertical_input * Time.deltaTime * moveZSpeed;

		Vector3 tempRotation = myTransform.eulerAngles;
		tempRotation.z = horizontal_input * -rotateZAmount;
		myTransform.eulerAngles = tempRotation;
		
		myTransform.localPosition += new Vector3(moveXAmount, 0, moveZAmount);
		
		if (myTransform.localPosition.x <= -limitX || myTransform.localPosition.x >= limitX)
		{
			float thePos = Mathf.Clamp(myTransform.localPosition.x, -limitX, limitX);
			myTransform.localPosition = new Vector3(thePos, myTransform.localPosition.y, myTransform.localPosition.z);
		}
		
		if (myTransform.localPosition.z <= originZ || myTransform.localPosition.z >= limitZ)
		{
			float thePos = Mathf.Clamp(myTransform.localPosition.z, originZ, limitZ);
			myTransform.localPosition = new Vector3(myTransform.localPosition.x, myTransform.localPosition.y, thePos);
		}
	}
}




