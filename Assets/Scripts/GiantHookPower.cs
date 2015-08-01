using UnityEngine;
using System.Collections;

public class GiantHookPower : MonoBehaviour, IPowerUp {
	private int timesUsed = 0;
	private int maxTimes = 3;

	public void UsePower() {
		if (GetComponent<PlayerController>().hookController.state == HookController.HookState.Attached) { return; }
		if (timesUsed == maxTimes) { return; }
		timesUsed++;
		GetComponent<PlayerController>().hookController.Grow();
	}

	public void Reset() {
		timesUsed = 0;
	}
}
