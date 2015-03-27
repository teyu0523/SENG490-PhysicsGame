using UnityEngine;
using System.Collections;

public class CarRightControl : MonoBehaviour {
	private float acc_right = 0;
	private float speed_right = 0;
	private Vector3 car_right_pos;
	private bool hit;
	// Use this for initialization
	void Start () {
		hit = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!hit){
			car_right_pos = transform.position;

			transform.position = new Vector3(car_right_pos.x + (speed_right + (acc_right * Time.time)), car_right_pos.y, car_right_pos.z);
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
		speed_right = new_speed;
	}

	public void updateAcc(float new_acc){
		acc_right = new_acc;
	}

	public void setHit(bool new_hit){
		hit = new_hit;
	}

	public void setPosition(float position){
		//car_right_pos.x = position;
		transform.position = new Vector3(position, car_right_pos.y, car_right_pos.z);
	}
}
