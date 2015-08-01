using UnityEngine;
using System.Collections;

public class SpeedPower : MonoBehaviour, IPowerUp {
	private int timesUsed = 0;
	private int maxTimes = 3;
	public void UsePower() {
		if (timesUsed == maxTimes) { return; }
		timesUsed++;
		GetComponent<PlayerController>().hookController.SpeedUp();
	}

	public void Reset() {
		timesUsed = 0;
	}
}
