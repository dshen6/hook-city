using UnityEngine;
using System.Collections;

public class Roamer : MonoBehaviour {


	public float radius = 10;
	public float turnSpeed = 0.05f;
	public float turnAcceleration = 0.01f;
	public float seekMultiplyer = 5;
	private float rotationalVelocity = 0;
	private float heading = 0;
	private Vector2 position;
	private Vector2 origPos;
	
	void Awake () {
		origPos = transform.position;
		position = transform.position;
	}
	
	void Update () {
		Roam(position, origPos, radius);
	}

	private float AngleMod(float angle)
	{
		while (angle < 0) angle += (float)Mathf.PI * 2;
		while (angle > (float)Mathf.PI * 2) angle -= (float)Mathf.PI * 2;
		return angle;
	}

	private float PullAngle(float angle, float towards, float factor)
	{
		angle = AngleMod(angle);
		towards = AngleMod(towards);
		
		if (factor > 1) return towards;
		
		float towards_a = towards + (float)Mathf.PI * 2;
		float towards_b = towards - (float)Mathf.PI * 2;
		
		float dist_1 = Mathf.Abs(angle - towards);
		float dist_a = Mathf.Abs(angle - towards_a);
		float dist_b = Mathf.Abs(angle - towards_b);
		
		if (dist_1 < dist_a && dist_1 < dist_b) angle = angle * (1 - factor) + towards * factor;
		if (dist_a < dist_1 && dist_a < dist_b) angle = angle * (1 - factor) + towards_a * factor;
		if (dist_b < dist_1 && dist_b < dist_a) angle = angle * (1 - factor) + towards_b * factor;
		
		return angle;
	}

	public void Roam(Vector2 position, Vector2 home, float roamRadius)
	{
		rotationalVelocity += (Random.value * 2 - 1) * turnAcceleration;
		if (rotationalVelocity < -turnSpeed) rotationalVelocity = -turnSpeed;
		if (rotationalVelocity > turnSpeed) rotationalVelocity = turnSpeed;
		heading += rotationalVelocity;
		
		Vector2 homingVector = home - position;
		float homingAngle = (float)Mathf.Atan2(homingVector.y, homingVector.x);
		float homingFactor = homingVector.magnitude / roamRadius / 50;
		heading = PullAngle(heading, homingAngle, homingFactor);
	}

	public void Affectheading(Vector2 position, Vector2 target, float amount, float roamRadius)
	{
		Vector2 vector = target - position;
		float angle = (float)Mathf.Atan2(vector.y, vector.x);
		float factor = amount * vector.magnitude / roamRadius / 50 * seekMultiplyer;
		heading = PullAngle(heading, angle, factor);
	}
	
	public void Affectheading(Vector2 position, Vector2 target, float amount)
	{
		Vector2 vector = target - position;
		float angle = (float)Mathf.Atan2(vector.y, vector.x);
		heading = PullAngle(heading, angle, amount);
	}

	public Vector2 GetDirection()
	{
		return new Vector2(Mathf.Cos(heading) * Mathf.Rad2Deg, Mathf.Sin(heading) * Mathf.Rad2Deg).normalized;
	}

}
