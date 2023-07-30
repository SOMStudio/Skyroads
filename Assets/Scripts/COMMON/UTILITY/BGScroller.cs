using UnityEngine;

[AddComponentMenu("Utility/Back ground scroller")]
public class BGScroller : ExtendedCustomMonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private Vector3 direction = Vector3.back;
	[SerializeField] private float scrollSpeed;
	[SerializeField] private float tileSizeZ;

	private Vector3 startPosition;

	private void Update()
	{
		if (canControl)
		{
			float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSizeZ);
			transform.position = startPosition + direction * newPosition;
		}
	}
	
	public override void Init()
	{
		base.Init();

		canControl = true;
		
		startPosition = transform.position;
	}

	public float ScrollSpeed
	{
		get => scrollSpeed;
		set => scrollSpeed = value;
	}
}
