using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {
	public bool isFacingRight;
	public Vector2 aimDir;

	PlayerController controller;
	float horizontal;
	float vertical;
	PlayerAnimator anim;
	Vector3 newRot;

	private string A_INPUT = "A_Keyboard_";
	private string B_INPUT = "B_Keyboard_";
	private string A_GAMEPAD = "A_";
	private string B_GAMEPAD = "B_";
	private string X_GAMEPAD = "X_";
	private string Horizontal_INPUT = "L_XAxis_Keyboard_";
	private string Vertical_INPUT = "L_YAxis_Keyboard_";
	private string Horizontal_GAMEPAD = "L_XAxis_";
	private string Vertical_GAMEPAD = "L_YAxis_";
	private string Start_INPUT = "Start_";


	// Use this for initialization
	void Start () {
		controller = GetComponent<PlayerController> ();

		int pID = controller.PLAYER_ID;
		A_INPUT += pID;
		B_INPUT += pID;
		Horizontal_INPUT += pID;
		Vertical_INPUT += pID;
		A_GAMEPAD += pID;
		B_GAMEPAD += pID;
		X_GAMEPAD += pID;
		Horizontal_GAMEPAD += pID;
		Vertical_GAMEPAD += pID;
		Start_INPUT += pID;

		aimDir = Vector2.zero;
		anim = GetComponent<PlayerAnimator> ();
		newRot = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
		HandleInput ();
	}


	void HandleInput() {
		horizontal = Input.GetAxisRaw (Horizontal_INPUT) + Input.GetAxisRaw (Horizontal_GAMEPAD);
		vertical = Input.GetAxisRaw (Vertical_INPUT) + Input.GetAxisRaw (Vertical_GAMEPAD);
		aimDir.x = horizontal;
		aimDir.y = vertical;

		if (horizontal > 0)
			isFacingRight = true;
		else if (horizontal < 0)
			isFacingRight = false;

		if(!controller.sheathing)
			HandleFlipping ();

		if (aimDir==Vector2.zero)
		{
			if(isFacingRight)
				aimDir = Vector2.right;
			else
				aimDir = -Vector2.right;
		}

		HandleAiming ();

		//print (aimDir);

		if (Input.GetButtonDown(A_INPUT) || Input.GetButtonDown(B_GAMEPAD)) {
			if (controller.hookController.state == HookController.HookState.Sheathed) { // put cooldown logic here
				anim.TriggerShooting ();
				controller.Shoot();
				HandleSheathing();
				anim.SetSheathing(true, aimDir);
			}
			else if (controller.sheathing) {
				controller.Fall();
				anim.SetSheathing(false, aimDir);
				
				newRot.z = 0;
				transform.localEulerAngles = newRot;
				
			}
		}

		if (Input.GetButtonDown(B_INPUT) || Input.GetButtonDown(A_GAMEPAD)) {
			controller.UseYPower();
		}

		if (Input.GetButtonDown(X_GAMEPAD)) {
			controller.UseXPower();
		}
		
		if (Input.GetButtonDown(Start_INPUT))
		{
			controller._gc.RestartGame();
		}

	}

	void HandleSheathing ()
	{
		if(aimDir == Vector2.up && isFacingRight)
			newRot.z = 90;
		else if(aimDir == -Vector2.up && isFacingRight)
			newRot.z = -90;
		else if(aimDir == Vector2.up)
			newRot.z = -90;
		else if(aimDir == -Vector2.up)
			newRot.z = 90;
		else if (aimDir.x == 1 && aimDir.y == 1)
			newRot.z = 45;
		else if (aimDir.x == -1 && aimDir.y == 1)
			newRot.z = 315;
		else if (aimDir.x == 1 && aimDir.y == -1)
			newRot.z = 315;
		else if (aimDir.x == -1 && aimDir.y == -1)
			newRot.z = 45;
		else
			newRot.z = 0;
		
		transform.localEulerAngles = newRot;
	}

	void HandleAiming ()
	{
		controller.hookController.trajectory = aimDir;
		controller.hookController.grappleOffset = new Vector2(aimDir.x/2, aimDir.y/2-0.2f);

		if(aimDir == Vector2.up)
			anim.SetAim(1);
		else if (aimDir.y != 0)
			anim.SetAim(2);
		else
			anim.SetAim(0);
	}

	void HandleFlipping ()
	{
		if(isFacingRight)
			anim.SetFacing(true);
		else
			anim.SetFacing(false);
	}
}
