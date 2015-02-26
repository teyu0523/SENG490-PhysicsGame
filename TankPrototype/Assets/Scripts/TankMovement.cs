using UnityEngine;
using System.Collections;

public class TankMovement : MonoBehaviour {

	public float speed = 6f;

	Vector2 movement;
	Rigidbody2D tankRigidbody;

	void Awake(){
		tankRigidbody = GetComponent<Rigidbody2D> ();

	}
	
	void FixedUpdate () {

		float h = Input.GetAxisRaw ("Horizontal");
		Move (h);

	}

	void Move (float h){

		movement.Set (h, 0f);
		movement = movement.normalized * speed * Time.deltaTime;

		tankRigidbody.MovePosition (tankRigidbody.position + movement);


	}

}
