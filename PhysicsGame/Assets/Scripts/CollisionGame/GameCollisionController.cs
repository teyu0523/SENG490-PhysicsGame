using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class GameCollisionController : GameController {

	public GameObject car_left;
	public GameObject car_right;

	//public GameObject car_right;
	public float speed_left;
	public float speed_right;
	public float acc_right;
	public float acc_left;

	private JSONNode question;

	private bool m_touch_started = false;

	private CarRightControl car_right_control;
	private CarLeftControl car_left_control;

	private Vector3 car_left_pos;
	private Vector3 car_right_pos;
	private bool is_collision = false;

	private JSONNode m_answer;


	public override void initializeGame(JSONNode question, JSONNode previous_answer){
		base.initializeGame(question, previous_answer);
		if(question != null){
			m_answer = previous_answer;
			this.question = question;

			if(car_left){
				car_left_control = car_left.GetComponent(typeof(CarLeftControl)) as CarLeftControl;

				if(car_left_control == null){
					Debug.LogWarning("Script not found: CarLeftControl");
				}	
			} else {
				Debug.LogWarning("GameObject not found: CarLeft");
			}
			if(car_right){
				car_right_control = car_right.GetComponent(typeof(CarRightControl)) as CarRightControl;
				if(car_right_control == null){
					Debug.LogWarning("Script not found: CarRightControl");
				}	
			} else {
				Debug.LogWarning("GameObject not found: CarRight");
			}
		}
	}

	public override void Awake() {
		base.Awake ();
	}

	/// <summary>
	/// Updates once every tick. Looks for touch input or keyboard input to display scenario information.
	/// </summary>
	public override void Update()
	{
		base.Update();
		/*car_left_control.updateSpeed(speed_left);
		car_right_control.updateSpeed(speed_right);
		car_left_control.updateAcc(acc_left);
		car_right_control.updateAcc(acc_right);*/

	}

	public override void OnMenuChanged(JSONNode answer){
		m_answer = answer;
		Debug.Log(m_answer);
	}

	public override void OnSubmit (){

	}

}
