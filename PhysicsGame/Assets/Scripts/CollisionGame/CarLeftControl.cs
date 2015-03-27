using UnityEngine;
using System.Collections;

public class CarLeftControl : MonoBehaviour {
	public Rigidbody2D rbody2d;
	private float acc_left = 0;
	private float speed_left = 0;
	private Vector3 car_A_pos;
	private bool hit;
	// Use this for initialization
	void Start () {
		hit = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!hit){
			car_A_pos = transform.position;
			transform.position = new Vector3(car_A_pos.x + (speed_left + (acc_left * Time.time)), car_A_pos.y, car_A_pos.z);
		} else {
			// do animation for collision
		}
	}

	void OnCollisionEnter2D(Collision2D col){
		if(col.transform.root.tag == "Cars"){
			hit = true;
		}
	}

	public void setStartLocation(Vector3 new_pos){
		transform.position = new_pos;
	}

	public void updateSpeed(float new_speed){
		rbody2d.angularVelocity = new_speed;
	}

	public void updateAcc(float new_acc){
		acc_left = new_acc;
	}

	public void setHit(bool new_hit){
		hit = new_hit;
	}

	public void setPosition(float position){
		transform.position = new Vector3(position, car_A_pos.y, car_A_pos.z);
	}
}
