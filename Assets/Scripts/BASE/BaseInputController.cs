using UnityEngine;

[AddComponentMenu("Base/Input Controller")]

public class BaseInputController : MonoBehaviour {
	
	[Header("Directional")]
	public bool Up;
	public bool Down;
	public bool Left;
	public bool Right;

	[Header("Fire")]
	public bool Fire1;

	[Header("Weapon Slot")]
	public bool Slot1;
	public bool Slot2;
	public bool Slot3;
	public bool Slot4;
	public bool Slot5;
	public bool Slot6;
	public bool Slot7;
	public bool Slot8;
	public bool Slot9;

	[Header("Shift dir")]
	public float vert;
	public float horz;
	public bool shouldRespawn;
	
	protected virtual void CheckInput ()
	{	
		// override with your own code to deal with input
		horz = Input.GetAxis ("Horizontal");
		vert = Input.GetAxis ("Vertical");
	}
	
	public virtual float GetHorizontal()
	{
		// returns our cached horizontal input axis value
		return horz;
	}
	
	public virtual float GetVertical()
	{
		// returns our cached vertical input axis value
		return vert;
	}
	
	public virtual bool GetFire()
	{
		return Fire1;
	}
	
	public bool GetRespawn()
	{
		return shouldRespawn;	
	}
	
	public virtual Vector3 GetMovementDirectionVector()
	{
		var res = Vector3.zero;
		
		res.x = horz;
		res.y = vert;

		// return the movement vector
		return res;
	}

}
