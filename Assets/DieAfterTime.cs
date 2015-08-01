using UnityEngine;
using System.Collections;

public class DieAfterTime : MonoBehaviour {
	
	public float lifeTime;
	
	void Start () {
		StartCoroutine (Suicide ());
	}
	
	IEnumerator Suicide ()
	{
		yield return new WaitForSeconds (lifeTime);
		Destroy (gameObject);
	}
}