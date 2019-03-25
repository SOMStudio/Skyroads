using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomMy : MonoBehaviour {

	/// <summary>
	/// Gets the random int.
	/// </summary>
	/// <returns>The random int.</returns>
	/// <param name="minNum">Min number.</param>
	/// <param name="maxNum">Max number.</param>
	/// <param name="multNum">Multiplayer number.</param>
	public int GetRandomInt(int minNum, int maxNum, int multNum = 1) {
		int resNumber = minNum;

		if (multNum <= 1) {
			resNumber = Random.Range (minNum, maxNum + 1);
		} else {
			resNumber = Random.Range (minNum, maxNum * multNum + 1);
			resNumber = Mathf.RoundToInt(resNumber / multNum);

			if (resNumber < minNum)
				resNumber = minNum;
		}

		return resNumber;
	}

	/// <summary>
	/// Procents the random.
	/// </summary>
	/// <returns>The random int.</returns>
	/// <param name="procList">Proc list.</param>
	private int ProcentRandom(int[] procList) {
		int countElement = procList.Count ();
		int res = Random.Range (1, procList.Sum () + 1);

		for (int i = 0; i < countElement; i++) {
			int[] firtElement = procList.Take (i + 1).ToArray ();
			int sumFirsElement = firtElement.Sum ();

			if (res <= sumFirsElement)
				return i;
		}

		return countElement - 1;
	}

	/// <summary>
	/// Randoms the near number.
	/// </summary>
	/// <returns>The near number int.</returns>
	/// <param name="number">Number.</param>
	/// <param name="procentShift">Procent shift.</param>
	/// <param name="multNum">Mult number.</param>
	private int RandomNearNumber(int number, int procentShift, int multNum = 1) {
		int resNumber = number;

		if (multNum > 1) {
			resNumber *= multNum;
		}

		int minRes = Mathf.RoundToInt(resNumber - (resNumber * procentShift / 2 / 100));
		int maxRes = Mathf.RoundToInt(resNumber + (resNumber * procentShift / 2 / 100));

		resNumber = Random.Range (minRes, maxRes + 1);

		if (multNum > 1) {
			resNumber = Mathf.RoundToInt (resNumber / multNum);
		}

		return resNumber;
	}
}
