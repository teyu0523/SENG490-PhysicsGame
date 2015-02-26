using UnityEngine;
using System.Collections;

public class ProjectileShooting : MonoBehaviour {

	public float velocity;
	public Rigidbody2D barrelRigidbody;
	
	Rigidbody2D shotRigidbody;


	void Awake(){
		shotRigidbody = GetComponent<Rigidbody2D> ();
	

	}
	void Start () {
		shotRigidbody.velocity = (barrelRigidbody.rotation * Vector3.right) * velocity;
	}

}
