using UnityEngine;
using System.Collections;

public class TankController : MonoBehaviour {
	//speed constant for tank and barrel movenment
	public float speed = 6f;

	//need a public variable for distance from enemy tank
	public float currentBarrelAngle;

	public GameObject projectile;
	public Rigidbody2D barrelRigidbody;

	Vector2 movement;
	Rigidbody2D tankRigidbody;

	void Awake(){
		tankRigidbody = GetComponent<Rigidbody2D> ();
		
	}

	void Update(){
		
		if(Input.GetButton("Jump")){
			
			Instantiate(projectile, barrelRigidbody.position, barrelRigidbody.transform.rotation);
		}
	}

	void FixedUpdate () {
		
		float h = Input.GetAxisRaw ("Horizontal");
		float z = Input.GetAxisRaw ("Vertical");
		Move (h);
		Rotate (z);
		
	}

	//move tank left or right
	void Move (float h){
		
		movement.Set (h, 0f);
		movement = movement.normalized * speed * Time.deltaTime;
		tankRigidbody.MovePosition (tankRigidbody.position + movement);
		
		
	}

	//move tank berrel
	void Rotate(float z){
		currentBarrelAngle = barrelRigidbody.rotation;
		
		barrelRigidbody.MoveRotation (currentBarrelAngle + z * speed * Time.deltaTime);
		
	}

}
