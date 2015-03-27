using UnityEngine;
using System.Collections;

public class CarRightControl : MonoBehaviour {
	public Rigidbody2D rbody2d;
	private float acc_right = 0;
	private float speed_right = 0;
	private Vector3 car_B_pos;
	private bool _hit;
	private float _velocityAfter = 0;
	// Use this for initialization
	void Start () {
		_hit = false;
	}
	
	// Update is called once per frame
	void Update () {
		/*if(!hit){
			car_B_pos = transform.position;
			transform.position = new Vector3(car_B_pos.x + (speed_right + (acc_right * Time.time)), car_B_pos.y, car_B_pos.z);
		} else {
			// do animation for collision
		}*/
	}

	public bool Hit{
		get
		{
		    return this._hit;
		}
	}

	void OnCollisionEnter2D(Collision2D col){
		if(col.transform.root.tag == "Cars"){
			_hit = true;
		}
	}

	public void setStartLocation(Vector3 new_pos){
		transform.position = new_pos;
	}

	public void updateSpeed(float new_speed){
		rbody2d.velocity = new Vector2(new_speed, rbody2d.velocity.y);
	}

	public void updateAcc(float new_acc){
		acc_right = new_acc;
	}

	public float VelocityAfter{
		get
		{
			return this._velocityAfter;
		}
		set
		{
			this._velocityAfter = value;
		}

	}


	public void setPosition(float position){
		//car_B_pos.x = position;
		transform.position = new Vector3(position, transform.position.y, transform.position.z);
	}

	public void setMass(float mass){
		rbody2d.mass = mass;
	}
}
