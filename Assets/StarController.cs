using UnityEngine;
using System.Collections;

public class StarController : MonoBehaviour {

	private Roamer roamer;
	private CharacterController controller;
	public float speed;

	private Vector3 collisionVector;
	private Vector3 lastFrameVelocity;
	private bool shieldBroken = false;
	// Use this for initialization
	void Awake () {
		shieldBroken = false;
		roamer = GetComponent<Roamer>();
		controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 roamDirection = roamer.GetDirection();
		roamDirection += collisionVector * 1.24f;

		controller.Move(roamDirection * Time.deltaTime * speed);
		Vector2 fixPos = transform.position;
		transform.position = fixPos;
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.tag.Contains("Hook")) {
			if (!shieldBroken) { shieldBroken = true; }
			else {
				collider.GetComponent<HookController>().poweredUp = true;
			}
		}
		collisionVector = (collider.transform.position - transform.position).normalized;
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {
		collisionVector = -hit.moveDirection;
	}

}
