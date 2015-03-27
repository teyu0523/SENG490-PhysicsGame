using UnityEngine;
using System.Collections;

public class TankController : MonoBehaviour {
	//speed constant for tank and barrel movenment
	public float speed = 6f;

	//need a public variable for distance from enemy tank
	public float currentBarrelAngle;

	public Transform projectileSpawn;
	public GameObject projectile;
	public Rigidbody2D barrelRigidbody;
	public Transform barrelTransform;

	private float velocity = 25f;
	private bool tankControls = true;


	Vector2 movement;
	Rigidbody2D tankRigidbody;
	Transform tankTransform;
	GameObject bullet;


	void Awake(){
		tankRigidbody = GetComponent<Rigidbody2D> ();
		tankTransform = GetComponent<Transform> ();
	}

	void Update(){

		if (tankControls == true && !bullet) {
			if (Input.GetButton ("Jump")) {

				Fire ();
			}

			VelocityChange ();
		}

	}

	void FixedUpdate () {

		if (tankControls == true && !bullet) {
			float h = Input.GetAxisRaw ("Horizontal");
			float z = Input.GetAxisRaw ("Vertical");
			Move (h);
			Rotate (z);
		}
	}


	void VelocityChange (){
		if(Input.GetKeyDown ("e")){
			if (velocity < 100f){
				velocity+= 1;
			}
		}
		if(Input.GetKeyDown ("q")){
			if (velocity > 10f){
				velocity-= 1;
			}
		}
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

		if (currentBarrelAngle + z < 0) {
			barrelRigidbody.MoveRotation (0);
		} else if (currentBarrelAngle + z > 90) {
			barrelRigidbody.MoveRotation (90);
		}
		else{
			barrelRigidbody.MoveRotation (currentBarrelAngle + z * speed * Time.deltaTime);
		}
	}

	public void Fire(){
		if (bullet == null) {
			bullet = (GameObject)Instantiate (projectile, projectileSpawn.position, projectileSpawn.transform.rotation);
			bullet.rigidbody2D.velocity = (Quaternion.Euler (0, 0, currentBarrelAngle) * Vector3.right) * velocity;
		}
	}

	public void DisableTankControls(){
		tankControls = false;
	} 

	public void SetVelocity (float v){
		velocity = v;
	}

	public float GetVelocity (){
		return velocity;
	}

	public void SetAngle (float a){
		currentBarrelAngle = a;
		barrelTransform.rotation = Quaternion.Euler (0, 0, currentBarrelAngle);

	}

	public float GetAngle(){
		return currentBarrelAngle;
	}

	public void SetDistance(float d){

		Vector3 newPosition = tankTransform.position;

		newPosition.x = (d) * (-1f);
		tankTransform.position = newPosition;


	}


}
