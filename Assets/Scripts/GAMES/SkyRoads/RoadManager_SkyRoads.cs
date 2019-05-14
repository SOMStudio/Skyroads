using UnityEngine;

public class RoadManager_SkyRoads : MonoBehaviour {

	[SerializeField]
	private bool speedBoost = false;

	[Header("Settings")]
	[SerializeField]
	private int multiplierSpeed = 1;

	[Header("Managers")]
	[SerializeField]
	private BGScroller bgScroller;
	[SerializeField]
	private GameController_SkyRoads gameController;

	// main event
	void Start () {
		Init ();
	}

	void LateUpdate () {
		int globalSpeedGame = gameController.GlobalSpeedGame;

		// update speed
		if ((bgScroller.scrollSpeed * multiplierSpeed) != globalSpeedGame) {
			bgScroller.scrollSpeed = globalSpeedGame * multiplierSpeed;
		}

		// update speed boost
		bool speedBoostGC = gameController.IsBoostSpeed ();
		if (speedBoostGC) {
			if (!speedBoost) {
				bgScroller.scrollSpeed *= globalSpeedGame;
				speedBoost = true;
			}
		} else {
			if (speedBoost) {
				bgScroller.scrollSpeed /= globalSpeedGame;
				speedBoost = false;
			}
		}
	}

	// main logic
	void Init() {
		if (!gameController) {
			gameController = GameController_SkyRoads.Instance;

			if (!gameController) {
				Debug.Log ("Cannot find GameController!!");
			}
		}
	}
}
