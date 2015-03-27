﻿using UnityEngine;
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

	public GameObject m_up_button;
	public GameObject m_down_button;
	public GameObject m_left_button;
	public GameObject m_right_button;
	public GameObject m_fire_button;

	private bool m_up_pressed = false;
	private bool m_down_pressed = false;
	private bool m_left_pressed = false;
	private bool m_right_pressed = false;
	private bool m_fire_pressed = false;

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
			if (Input.GetButton ("Jump") || m_fire_pressed) {

				Fire ();
			}

			VelocityChange ();
		}

	}

	void FixedUpdate () {

		if (tankControls == true && !bullet) {
			float h = Input.GetAxisRaw ("Horizontal") + ((m_left_pressed) ? -1.0f : 0.0f) + ((m_right_pressed) ? 1.0f : 0.0f);
			float z = Input.GetAxisRaw ("Vertical") + ((m_up_pressed) ? 1.0f : 0.0f) + ((m_down_pressed) ? -1.0f : 0.0f);
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

	// Touch Input Functions
	public void OnUpPressed(bool down) {
		m_up_pressed = down;
	}
	public void OnDownPressed(bool down) {
		m_down_pressed = down;
	}
	public void OnLeftPressed(bool down) {
		m_left_pressed = down;
	}
	public void OnRightPressed(bool down) {
		m_right_pressed = down;
	}
	public void OnFirePressed(bool down) {
		m_fire_pressed = down;
	}
	public void OnSliderChanged(float value) {
		SetVelocity(15f + value*35f);
	}
}
