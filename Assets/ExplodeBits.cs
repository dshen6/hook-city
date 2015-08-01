using UnityEngine;
using System.Collections;

public class ExplodeBits : MonoBehaviour 
{
	public bool destroyChildren;    //should the children be detached (and kept alive) before object is destroyed?
	public float pushChildAmount;   //push children away from centre of parent
	
	void Start()
	{
		//get list of children
		Transform[] children = new Transform[transform.childCount];
		for (int i = 0; i < transform.childCount; i++)
			children[i] = transform.GetChild(i);
		
		//detach children
		if (!destroyChildren)
			transform.DetachChildren();
		
		//add force to children (and a bit of spin to make it interesting)
		foreach (Transform child in children)
		{
			if(child.rigidbody && pushChildAmount != 0)
			{
				Vector2 direction = child.position - transform.position;
				child.rigidbody.AddForce (direction * pushChildAmount, ForceMode.Impulse);
				child.rigidbody.AddTorque (Vector3.right);
			}
		}
		
	}
	
}