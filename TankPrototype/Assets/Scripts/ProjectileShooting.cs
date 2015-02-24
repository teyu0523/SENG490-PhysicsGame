using UnityEngine;
using System.Collections;

public class ProjectileShooting : MonoBehaviour {

	public float velocity;

	Vector2 shooting;
	Rigidbody2D shotRigidbody;
	Transform shotSpawn;


	void Awake(){
		shotRigidbody = GetComponent<Rigidbody2D> ();
		shotSpawn = GetComponent<Transform> ();
	

	}
	void Start () {
		shooting.Set (velocity, 0f);
		shotRigidbody.velocity = shooting;
	}

}
