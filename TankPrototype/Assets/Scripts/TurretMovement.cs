using UnityEngine;
using System.Collections;

public class TurretMovement : MonoBehaviour {

	public float speed = 6f;
	public float currentAngle;

	public GameObject shot;
	public Transform shotSpawn;

	Rigidbody2D turretRigidbody;


	void Awake(){
		turretRigidbody = GetComponent<Rigidbody2D> ();
		//shotSpawnRigidbody = GetComponentInChildren<Rigidbody2D> ();
		
	}

	void Update(){

		if(Input.GetButton("Jump")){

			Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
		}
	}

	void FixedUpdate () {
		
		float z = Input.GetAxisRaw ("Vertical");
		Rotate (z);

	}

	void Rotate(float z){
		currentAngle = turretRigidbody.rotation;

		turretRigidbody.MoveRotation (currentAngle + z * speed * Time.deltaTime);

	}

}
