using UnityEngine;
using System.Collections;

public class TankController : MonoBehaviour {
	//speed constant for tank and barrel movenment
	public float speed = 6f;
	public float velocity = 100f;

	//need a public variable for distance from enemy tank
	public float currentBarrelAngle;

	public Transform projectileSpawn;
	public GameObject projectile;
	public Rigidbody2D barrelRigidbody;

	Vector2 movement;
	Rigidbody2D tankRigidbody;
	GameObject bullet;


	void Awake(){
		tankRigidbody = GetComponent<Rigidbody2D> ();
		
	}

	void Update(){
		
		if(Input.GetButton("Jump")){

			if (bullet == null){
				bullet = (GameObject) Instantiate(projectile, projectileSpawn.position, projectileSpawn.transform.rotation);
				bullet.rigidbody2D.velocity = (Quaternion.Euler(0, 0, currentBarrelAngle) * Vector3.right) * velocity;
			}
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

		if(currentBarrelAngle + z < 0){
			barrelRigidbody.MoveRotation (0);
		}
		else{
			barrelRigidbody.MoveRotation (currentBarrelAngle + z * speed * Time.deltaTime);
		}
	}

}
