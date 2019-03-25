using System.Collections;
using UnityEngine;

public class MoverTo : MonoBehaviour {

	public Transform moveTarget;
	public Vector3 targetOffset;
	public float moveTime = 2f;
	public AnimationCurve speedCurve;

	private Transform myTransform;

	private Vector3 startPos;
	private Vector3 vectorMove;
	private Vector3 directionMove;
	private float lengthMove = 0;

	void Start ()
	{
		if (!myTransform) {
			myTransform = transform;
		}
	}
	
	public void SetTarget( Transform aTransform )
	{
		moveTarget = aTransform;
	}

	public void SetPosition( Vector3 val )
	{
		myTransform.position = val;
	}

	public void StartMove() {
		startPos = myTransform.position;
		vectorMove = moveTarget.position - myTransform.position + targetOffset;
		directionMove = vectorMove.normalized;
		lengthMove = vectorMove.magnitude;

		if (speedCurve.keys.Length == 0) {
			StartCoroutine ("MoveLine");
		} else {
			StartCoroutine ("MoveCurve");
		}
	}

	private IEnumerator MoveLine() {
		float _step = 0;

		while (_step < moveTime)
		{
			_step += Time.deltaTime;

			if (_step < moveTime) {
				float curLength = _step * lengthMove / moveTime;
				transform.localPosition = startPos + directionMove * curLength;
			} else {
				transform.localPosition = moveTarget.position + targetOffset;
			}

			yield return null;
		}
	}

	private IEnumerator MoveCurve() {
		float _step = 0;

		while (_step < moveTime)
		{
			_step += Time.deltaTime;

			if (_step < moveTime) {
				float curLength = lengthMove * speedCurve.Evaluate(_step / moveTime);
				transform.localPosition = startPos + directionMove * curLength;
			} else {
				transform.localPosition = moveTarget.position + targetOffset;
			}

			yield return null;
		}
	}
}
