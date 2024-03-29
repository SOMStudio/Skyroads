using UnityEngine;

[AddComponentMenu("Base/Input Controller")]
public class BaseInputController : MonoBehaviour
{
	[Header("Directional")]
	[SerializeField] protected bool Up;

	[SerializeField] protected bool Down;
	[SerializeField] protected bool Left;
	[SerializeField] protected bool Right;

	[Header("Fire")]
	[SerializeField] protected bool Fire1;

	[Header("Weapon Slot")]
	[SerializeField] protected bool Slot1;

	[SerializeField] protected bool Slot2;
	[SerializeField] protected bool Slot3;
	[SerializeField] protected bool Slot4;
	[SerializeField] protected bool Slot5;
	[SerializeField] protected bool Slot6;
	[SerializeField] protected bool Slot7;
	[SerializeField] protected bool Slot8;
	[SerializeField] protected bool Slot9;

	[Header("Shift dir")]
	[SerializeField] protected float vert;
	[SerializeField] protected float horz;
	[SerializeField] protected bool shouldRespawn;
	
	protected virtual void CheckInput()
	{
		horz = Input.GetAxis("Horizontal");
		vert = Input.GetAxis("Vertical");
	}

	public virtual float GetHorizontal()
	{
		return horz;
	}

	public virtual float GetVertical()
	{
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
		
		return res;
	}
}
