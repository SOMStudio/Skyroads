using UnityEngine;
using System.Collections;

[AddComponentMenu("Base/Vehicles/Top Down Space Ship")]

public class BaseTopDownSpaceShip : ExtendedCustomMonoBehaviour
{
	[Header("Move settings")]
	[SerializeField]
	protected float moveXSpeed = 5f;
	[SerializeField]
	protected float moveZSpeed = 5f;

	[Header("Limit settings")]
	[SerializeField]
	protected float limitX = 15f;
	[SerializeField]
	protected float limitZ = 15f;
	[SerializeField]
	protected float rotateZAmount = 15f;
	protected float originZ;

	[Header("Technic value")]
	[SerializeField]
	protected float horizontal_input;
	[SerializeField]
	protected float vertical_input;
	
	[Header("Technic referance")]
	[SerializeField]
	protected Keyboard_Input default_input;

	// main event
	void Update ()
	{
		UpdateShip ();
	}

	// main logic

	/// <summary>
	/// Init main instance (base: myTransform, myGO, myBody cur: KeybordInput), def. in Start.
	/// </summary>
	public override void Init ()
	{	
		// cache refs to our transform and gameObject
		base.Init ();

		didInit = false;

		// add default keyboard input
		default_input = myGO.AddComponent<Keyboard_Input> ();
		
		// grab the starting Z position to use as a baseline for Z position limiting
		originZ = myTransform.localPosition.z;
		
		// set a flag so that our Update function knows when we are OK to use
		didInit = true;
	}

	/// <summary>
	/// Games the start (canContrl = true).
	/// </summary>
	protected virtual void GameStart ()
	{
		// we are good to go, so let's get moving!
		canControl = true;
	}

	/// <summary>
	/// Gets the input (horizontal move, vertical move).
	/// </summary>
	protected virtual void GetInput ()
	{
		// this is just a 'default' function that (if needs be) should be overridden in the glue code
		horizontal_input = default_input.GetHorizontal ();
		vertical_input = default_input.GetVertical ();
	}

	/// <summary>
	/// Updates the ship with GetInput and rotate with rotateZAmount.
	/// </summary>
	protected virtual void UpdateShip ()
	{
		// don't do anything until Init() has been run
		if (!didInit)
			return;
		
		// check to see if we're supposed to be controlling the player before moving it
		if (!canControl)
			return;
		
		GetInput ();
		
		// calculate movement amounts for X and Z axis
		float moveXAmount = horizontal_input * Time.deltaTime * moveXSpeed;
		float moveZAmount = vertical_input * Time.deltaTime * moveZSpeed;
		
		Vector3 tempRotation = myTransform.eulerAngles;
		tempRotation.z = horizontal_input * -rotateZAmount;
		myTransform.eulerAngles = tempRotation;
		
		// move our transform to its updated position
		myTransform.localPosition += new Vector3 (moveXAmount, 0, moveZAmount);
		
		// check the position to make sure that it is within boundaries
		if (myTransform.localPosition.x <= -limitX || myTransform.localPosition.x >= limitX) {
			float thePos = Mathf.Clamp (myTransform.localPosition.x, -limitX, limitX);
			myTransform.localPosition = new Vector3 (thePos, myTransform.localPosition.y, myTransform.localPosition.z);
		}
		
		// we also check the Z position to make sure that it is within boundaries
		if (myTransform.localPosition.z <= originZ || myTransform.localPosition.z >= limitZ) {
			float thePos = Mathf.Clamp (myTransform.localPosition.z, originZ, limitZ);
			myTransform.localPosition = new Vector3 (myTransform.localPosition.x, myTransform.localPosition.y, thePos);
		}
	}
	
}




