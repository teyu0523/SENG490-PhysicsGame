using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class GameCollisionController : GameController {

	public GameObject car_left;
	public GameObject car_right;
	public GameObject canvas;

	//public GameObject car_right;
	public float speed_left;
	public float speed_right;
	public float acc_right;
	public float acc_left;

	private JSONNode question;

	private bool m_touch_started = false;

	private CarRightControl car_right_control;
	private CarLeftControl car_left_control;
	private SideMenu side_menu;

	private Vector3 car_left_pos;
	private Vector3 car_right_pos;
	private bool is_collision = false;

	private JSONNode m_answer;


	public override void initializeGame(JSONNode question, JSONNode previous_answer){
		if(question != null){
			m_answer = previous_answer;
			this.question = question;
			if(canvas){
				side_menu = canvas.GetComponent(typeof(SideMenu)) as SideMenu;
				if(side_menu == null){
					Debug.LogWarning("Script not found: SideMenu");
				} else {
					side_menu.parseJSON(question, previous_answer);
				}
			} else {
				Debug.LogWarning("GameObject not found: canvas");
			}

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

		/*if (Input.GetKeyDown(KeyCode.P) ){
			side_menu.pause();

		}*/
		if (Input.GetKeyDown(KeyCode.P) || Input.touchCount == 4) {
			if(!m_touch_started) {
				side_menu.pause();
			}
			
			m_touch_started = true;
		} else {
			m_touch_started = false;
		}
	}

	public void setAnswer(JSONNode answer){
		m_answer = answer;
		Debug.Log(m_answer);
	}

	public override void OnSubmit (){

	}

}
