using UnityEngine;
using System.Collections;


public class PlayerController : MonoBehaviour {
	
	public int PLAYER_ID;
	public Transform p1;
	public Transform p2;
	public GameObject hook;
	public PlayerAnimator anim;
	private CharacterController characterController;
	public GameObject bodyParts;
	public bool sheathing = false;
	private bool reachedGrapple = false;

	public HookController hookController;
	LineRenderer grapple;

	float explosionForce;
	bool isExploding;
	

	public IPowerUp xPower;
	public IPowerUp yPower;

	private float gravity = 10f;
	private float terminalVelocity = 15f;
	private float groundMultiplier = .9f;
	private float airXMultiplier = .95f;
	private float airYMultiplier = .92f;
	
	Vector3 velocity;
	Vector3 lastFrameHookVelocity = Vector3.zero;

	public GameController _gc;

	void Awake() {
		characterController = GetComponent<CharacterController>();
		anim = GetComponent<PlayerAnimator> ();
		xPower = gameObject.AddComponent<SpeedPower>();
		yPower = gameObject.AddComponent<GiantHookPower>();
	}

	void Start () {
		_gc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>();
		hookController = hook.GetComponent<HookController>();
		grapple = GetComponent<LineRenderer>();
		grapple.SetColors(Color.gray, Color.gray);
		grapple.SetWidth(0.05f, 0.05f);
		grapple.SetVertexCount(2);
	}
	
	void Update () {

		if (hookController.state == HookController.HookState.Attached && !sheathing) {
			sheathing = true;
		}

		if (!sheathing) {
			anim.SetGrounded(characterController.isGrounded);
		}

		velocity = GetHookVelocity();
		velocity += GetGravity();
		if (reachedGrapple) { velocity = Vector3.zero; }
		characterController.Move(velocity * Time.deltaTime);

		DrawGrapple();

		Vector2 fixPos = transform.position;
		transform.position = fixPos;
	}


	void OnTriggerEnter(Collider other) {
		if (other.tag.Equals("Hook" + PLAYER_ID)) {
			hookController.resetSpeed();
		}
	}

	
	Vector3 GetHookVelocity() {
		Vector3 hookVelocity = Vector3.zero;

		if (sheathing) {
			Vector3 distance = hook.transform.position - transform.position;
			if (distance.magnitude < .5f) {
				reachedGrapple = true;
			}
			else {
				hookVelocity = distance.normalized * hookController.sheathSpeed;
			}
		} 
		else if (!characterController.isGrounded) {
			hookVelocity = new Vector3(lastFrameHookVelocity.x * airXMultiplier, lastFrameHookVelocity.y * airYMultiplier, 0);
			
		}
		else if (characterController.isGrounded) {
			hookVelocity = new Vector3(lastFrameHookVelocity.x * groundMultiplier, 0, 0);
		}

		lastFrameHookVelocity = hookVelocity;
		return hookVelocity;
	}

	Vector3 GetGravity() {
		float y_velocity = velocity.y - gravity;
		y_velocity = Mathf.Min(y_velocity, terminalVelocity);
		return new Vector3(0 ,y_velocity, 0);
	}

	public void Shoot() {
		hookController.Fire();
	}

	public void Fall() {
		reachedGrapple = false;
		//Debug.Log ("falling");
		sheathing = false;
		hookController.state = HookController.HookState.Sheathed;
		anim.SetSheathing (false, Vector2.zero);
		transform.localEulerAngles = Vector3.zero;
		hookController.resetSize();
	}

	void DrawGrapple() {
		grapple.SetPosition(0, new Vector2(transform.position.x, transform.position.y - 0.1f));
		grapple.SetPosition(1, hook.transform.position);
	}

	public void KillPlayer (int killedBy)
	{
		if(bodyParts)
			Instantiate(bodyParts, transform.position, Quaternion.identity);
		grapple.enabled = false;
		transform.position = new Vector2 (99, 99);
		_gc.EndGame (killedBy);
		//print ("killed by player " + killedBy);
	}


	public void UseXPower() {
		if (xPower != null) {
			xPower.UsePower();
		}
	}

	public void UseYPower() {
		if (yPower != null) {
			yPower.UsePower();
		}
	}

}


//startTime = Time.time;
//radius = Vector3.Distance(transform.position, hookController.transform.position);
//float localTheta = Vector3.Angle(Vector3.down, transform.position - hookController.transform.position) * Mathf.Deg2Rad;
//
////			maxTheta = localTheta * Mathf.Sin(gravity/radius * Time.time);
//period = 2 * Mathf.PI * Mathf.Sqrt(radius/gravity);
////			Debug.Log("MaxTheta is " + localTheta * Mathf.Rad2Deg);
//			Debug.Log("period is " + period);

//		if (sheathing) {
//			radius = Vector3.Distance(transform.position, hookController.transform.position);
//			float localTheta = Vector3.Angle(Vector3.down, transform.position - hookController.transform.position) * Mathf.Deg2Rad;
//			float height = radius - radius * Mathf.Cos(-localTheta);
//			Debug.Log("height " + height);
//			swingVelocity = Mathf.Sqrt(2 * gravity * radius + lastFrameSwingVelocity * lastFrameSwingVelocity * .8f);
//			lastFrameSwingVelocity = swingVelocity;

// get period, then slerp angle
//			float theta = Mathf.Lerp(-maxTheta, maxTheta, Time.time);
//			Debug.Log(theta);
//			Vector3 target = (transform.position - hookController.transform.position);
//			velocity = new Vector3(swingVelocity, swingVelocity, 0);
//		}
