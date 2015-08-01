using UnityEngine;
using System.Collections;

public class HookController : MonoBehaviour {

	public enum HookState {
		Sheathed, // Player can fire
		Traveling, // Player can't fire
		Attached // Player can fall(instant refresh of hook, stops being pulled by hook)
	}

	public HookState state;
	public GameObject player;
	private Transform playerTransform;

	public int HOOK_ID;
	public float fireSpeed = 10f;
	public float sheathSpeed = 10f;
	public Vector3 trajectory = new Vector3(1,1,0).normalized;
	private Vector3 velocity;
	public Vector3 grappleOffset;

	private bool spedUp = false;
	private bool grew = false;
	public bool poweredUp = false;

	void Start () {
		playerTransform = player.transform;
		state = HookState.Sheathed;
		transform.position = playerTransform.position + grappleOffset;
	}
	
	void Update () {
		if (spedUp) {
			sheathSpeed *= .84f;
			sheathSpeed = Mathf.Max(sheathSpeed, 10f);
		}
		if (poweredUp) {
			Grow();
			poweredUp = false;
		}

		if (state == HookState.Traveling) {
			transform.position += velocity * Time.deltaTime * fireSpeed;
		}
		else if (state == HookState.Sheathed) {
			transform.position = playerTransform.position + grappleOffset;
			RotateHook();
		}
	}

	void RotateHook ()
	{
		Vector2 aimDir = playerTransform.GetComponent<PlayerInput>().aimDir;
		//print (aimDir);
		Vector3 newRot = Vector3.zero;

		if(aimDir == Vector2.up)
			newRot.z = 90;
		else if(aimDir == -Vector2.up)
			newRot.z = -90;
		else if(aimDir == Vector2.up)
			newRot.z = -90;
		else if(aimDir == -Vector2.up)
			newRot.z = 90;
		else if (aimDir.x == 1 && aimDir.y == 1)
			newRot.z = 45;
		else if (aimDir.x == -1 && aimDir.y == 1)
			newRot.z = 135;
		else if (aimDir.x == 1 && aimDir.y == -1)
			newRot.z = 315;
		else if (aimDir.x == -1 && aimDir.y == -1)
			newRot.z = 45;
		else if (aimDir == -Vector2.right)
			newRot.z = 180;
		else
			newRot.z = 0;
		
		transform.localEulerAngles = newRot;
	}

	void OnTriggerEnter(Collider other) {
		if (state != HookState.Traveling) {
			return; 
		}

		string otherTag = other.tag;

		if (otherTag == "Terrain") {
			//Debug.Log("hook hit wall");
			Attach(); 
		}

		else if (otherTag == "Player") {
			//check to make sure it isnt the owner of this hook
			PlayerController enemy = other.GetComponent<PlayerController>();
			if (HOOK_ID != enemy.PLAYER_ID){
				enemy.KillPlayer(HOOK_ID);
			}
		}

		if (otherTag.Contains("hook")) {
//			print ("hit hook wit hhook");
		}

	}

	public void Attach() {
		velocity = Vector3.zero;
		state = HookState.Attached;
	}

	public void Fire() {
		if (spedUp) {
			resetSpeed();
		}
		// HACK, we can return immediately if outside bounds of arena
		if (GameController.outOfBounds(transform.position)){
			Debug.Log("out of bounds");
			return;
		}

		state = HookController.HookState.Traveling;
		transform.position = playerTransform.position + grappleOffset;
		velocity = trajectory;
	}

	public void Grow() {
		if (grew) { return; }
		grew = true;
		transform.localScale += new Vector3(3, 3, 0);
	}

	public void resetSize() {
		transform.localScale = new Vector3(2, 2, 2);
		grew = false;
	}

	public void SpeedUp() {
		sheathSpeed += 30f;
		spedUp = true;
	}

	public void resetSpeed() {
		if (spedUp) {
			sheathSpeed = 10f;
			spedUp = false;
		}
	}
}
