using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// attach to Empty Game Object
// set yourPrefubObject
// cahnge initObjectCount, isParentEnabled if need
public class ObjectPoolController : MonoBehaviour {

	private static readonly Dictionary<string, ObjectPoolController> namesOfObjects = new Dictionary<string, ObjectPoolController>();

	public static ObjectPoolController GetPoolByName(string name) {
		return namesOfObjects[name];
	}

	[SerializeField]
	private string nameOfYourPool = string.Empty;

	[SerializeField]
	private Transform yourPoolPrefab = null;

	[SerializeField]
	private int initialObjectCounter = 23;

	[SerializeField]
	private bool isParentEnabled = true;

	private readonly Queue<Transform> yourObjectsStack = new Queue<Transform>();

	//[Header("Game Controller Ref")]
	//public GameController gameController;

	void Start()
	{
		System.Diagnostics.Debug.Assert(yourPoolPrefab);
		namesOfObjects[nameOfYourPool] = this;

		for (int i = 0; i < initialObjectCounter; i++)
		{
			var t = Instantiate(yourPoolPrefab) as Transform;
			AdjustingYourObject(t);
			LiberationObject(t);

			if (isParentEnabled) {
				//t.position = gameController.placeCreate.transform.position;
			}
		}
	}

	public Transform GetObject(Vector3 position = new Vector3())
	{
		Transform t = null;

		if (yourObjectsStack.Count > 0) {
			t = yourObjectsStack.Dequeue ();
		} else {
			t = Instantiate (yourPoolPrefab) as Transform;
		}

		t.position = position;
		AdjustingYourObject (t);

		return t;
	}

	private void AdjustingYourObject(Transform obj)
	{
		if (isParentEnabled)
		{
			obj.parent = transform;
		}

		//obj.gameObject.SetActiveRecursively(true);
		//obj.gameObject.SetActive(true);
		obj.BroadcastMessage ("OnCreateEvent", this, SendMessageOptions.DontRequireReceiver);
	}

	public void LiberationObject(Transform obj)
	{
		obj.BroadcastMessage ("OnLiberationEvent", this, SendMessageOptions.DontRequireReceiver);
		//obj.gameObject.SetActiveRecursively(false);
		//obj.gameObject.SetActive(false);
		yourObjectsStack.Enqueue (obj);
	}
}

//exemple use in code
//public class YourPoolExampleUsage : MonoBehaviour {
//	void Start() {
//		ObjectPoolController pool = ObjectPoolController.GetPoolByName("Bang");
//		Transform obj = pool.GetObject(Vector3.zero);
//	}
//}

//exemple using event in pooled Object
//[RequireComponent(typeof(ParticleSystem))]
//public class YourPoolParticleSystem : MonoBehaviour
//{
//	private ObjectPoolController yourPoolClass;
//	void OnCreateEvent(ObjectPoolController ypc)
//	{
//		yourPoolClass = ypc;
//		particleSystem.renderer.enabled = true;
//		particleSystem.time = 0;
//		particleSystem.Clear(true);
//		particleSystem.Play(true);
//	}
//	void OnLiberationEvent()
//	{
//		particleSystem.Stop();
//		particleSystem.time = 0;
//		particleSystem.Clear(true);
//		particleSystem.renderer.enabled = false;
//	}
//	void Update()
//	{
//		if (!particleSystem.IsAlive(true) && particleSystem.renderer.
//			enabled)
//		{
//			yourPoolClass.LiberationObject(transform);
//		}
//	}
//}
