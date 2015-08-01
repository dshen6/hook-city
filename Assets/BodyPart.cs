using UnityEngine;
using System.Collections;

public class BodyPart : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		Vector3 fixPos = transform.position;
		if(fixPos.y < -2.6f)
		{
			fixPos.y = -2.6f;

		}

		transform.position = fixPos;
	}
}
