using UnityEngine;
using System.Collections;

public class PlayerAnimator : MonoBehaviour {

	public Transform mySprite;
	Animator anim;
//	PlayerController controller;


	void Start ()
	{
		anim = mySprite.GetComponent<Animator> ();
		//controller = GetComponent<PlayerController> ();

	}

	public void SetAim ( int aimIndex ) // Sets the aim direction in the animator
	{
		anim.SetInteger ("AimDirection", aimIndex);
	}

	void Update ()
	{
		//mySprite.position = Vector3.zero; // Hack to keep sprite in place
	}

	public void SetFacing(bool right) // Flips the sprite depending on facing direction
	{
		Vector3 scale = mySprite.localScale;

		if(right)
			scale.x = 2.5f;	
		else
			scale.x = -2.5f;

		mySprite.localScale = scale;
	}

	public void SetSheathing(bool sheathing, Vector2 aimDir)
	{
		anim.SetBool ("Sheathing", sheathing);

	
	}

	public void TriggerShooting ()
	{
		anim.SetTrigger ("Shoot");
	}

	public void SetGrounded (bool isGrounded)
	{
		anim.SetBool ("Grounded", isGrounded);
	}
}
