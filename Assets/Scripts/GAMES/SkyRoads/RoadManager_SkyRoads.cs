using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager_SkyRoads : MonoBehaviour {

	[SerializeField]
	private bool speedBoost = false;

	[Header("Settings")]
	public int multiplierSpeed = 1;

	[Header("Managers")]
	public BGScroller bgScroller;
	public GameController_SkyRoads gameController;

	void Start () {
		Init ();
	}

	void LateUpdate () {
		// update speed
		if ((bgScroller.scrollSpeed * multiplierSpeed) != gameController.globalSpeedGame) {
			bgScroller.scrollSpeed = gameController.globalSpeedGame * multiplierSpeed;
		}

		// update speed boost
		bool speedBoostGC = gameController.IsBoostSpeed ();
		if (speedBoostGC) {
			if (!speedBoost) {
				bgScroller.scrollSpeed *= gameController.multForBoostSpeed;
				speedBoost = true;
			}
		} else {
			if (speedBoost) {
				bgScroller.scrollSpeed /= gameController.multForBoostSpeed;
				speedBoost = false;
			}
		}
	}

	void Init() {
		if (!gameController) {
			gameController = GameController_SkyRoads.Instance;

			if (!gameController) {
				Debug.Log ("Cannot find GameController!!");
			}
		}
	}
}
