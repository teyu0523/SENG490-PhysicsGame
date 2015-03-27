using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using  System.Globalization;

public class GameCollisionController : GameController {

	public GameObject car_A; // A
	public GameObject car_B; // B
	public Camera cam;

	//public GameObject car_B;
	public float speed_left;
	public float speed_right;
	public float acc_right;
	public float acc_left;
 	public float velocity_b;
 	public float velocity_a;
	public float pos_b;
	public float pos_a;
	public float velocity_after_a;
	public float velocity_after_b;
	public float mass_b;
	public float mass_a;
	public float momentum_a;
	public float momentum_b;
	public float momentum_net;
	public float momentum_net_user;
	private bool wrong; // check for incorrect sumbission
	private bool submitted = false;
	private Vector3 velocity = Vector3.zero;
	private JSONNode question;

	private bool m_touch_started = false;

	private CarRightControl car_B_control;
	private CarLeftControl car_A_control;

	private Vector3 car_A_pos;
	private Vector3 car_B_pos;
	private bool is_collision = false;

	private JSONNode m_answer;


	public override void initializeGame(JSONNode question, JSONNode previous_answer){
		base.initializeGame(question, previous_answer);
		if(question != null){
			m_answer = previous_answer;
			this.question = question;

			if(car_A){
				car_A_control = car_A.GetComponent(typeof(CarLeftControl)) as CarLeftControl;

				if(car_A_control == null){
					Debug.LogWarning("Script not found: CarLeftControl");
				}	
			} else {
				Debug.LogWarning("GameObject not found: CarLeft");
			}
			if(car_B){
				car_B_control = car_B.GetComponent(typeof(CarRightControl)) as CarRightControl;
				if(car_B_control == null){
					Debug.LogWarning("Script not found: CarRightControl");
				}	
			} else {
				Debug.LogWarning("GameObject not found: CarRight");
			}
		}
		Debug.Log(question["values"]);
		Debug.Log(question["values"]["Car A Position"]["value"].Value);
		Debug.Log(float.Parse(question["values"]["Car A Position"]["value"].Value));

		pos_a = float.Parse(question["values"]["Car A Position"]["value"].Value);
		velocity_a = float.Parse(question["values"]["Car A Velocity"]["value"].Value);
		mass_a = float.Parse(question["values"]["Car A Mass"]["value"].Value);


		pos_b = float.Parse(question["values"]["Car B Position"]["value"].Value);
		velocity_b = float.Parse(question["values"]["Car B Velocity"]["value"].Value);
		mass_b = float.Parse(question["values"]["Car B Mass"]["value"].Value);

		car_A_control.setPosition(pos_a);
		car_A_control.setMass(mass_a);
		car_B_control.setPosition(pos_b);
		car_B_control.setMass(mass_b);
		momentum_b = mass_b * velocity_b;
		momentum_a = mass_a * velocity_a;

        adjustCamToFit();     
    }

	public void adjustCamToFit(){
		Vector3 old_vec3 = cam.transform.position;
		cam.transform.position = Vector3.SmoothDamp(
			new Vector3(
				old_vec3.x, 
				old_vec3.y, 
				old_vec3.z
				), 
			new Vector3(
				(pos_a+pos_b)/2,
				old_vec3.y,
				old_vec3.z),
			ref velocity,
			0.3F
			);
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
		if (car_A_control.Hit == true && car_B_control.Hit == true){
			Debug.Log("im here");
		}
		/*car_A_control.updateSpeed(speed_left);
		car_B_control.updateSpeed(speed_right);
		car_A_control.updateAcc(acc_left);
		car_B_control.updateAcc(acc_right);*/
	}

	public override void SetProperty(string name, string arg){
		if(name == "Car A Position") {
			pos_a = float.Parse(arg, CultureInfo.InvariantCulture);
			car_A_control.setPosition(pos_a);
		} else if (name == "Car B Position") {
			pos_b = float.Parse(arg);
			car_B_control.setPosition(pos_b);
		} else if (name == "Car B Velocity") {
			velocity_b = float.Parse(arg);
			momentum_b = mass_b * velocity_b;
			momentum_b = mass_b * velocity_b;
		} else if (name == "Car A Velocity") {
			velocity_a = float.Parse(arg);
			momentum_a = mass_a * velocity_a;
		} else if (name == "Car A Mass") {
			mass_a = float.Parse(arg);
			car_A_control.setMass(mass_a);
			momentum_a = mass_a * velocity_a;
		} else if (name == "Car B Mass") {
			mass_b = float.Parse(arg);
			car_B_control.setMass(mass_b);
			momentum_b = mass_b * velocity_b;
		} else if (name == "Car A Velocity After") {
			velocity_after_a = float.Parse(arg);
			car_A_control.VelocityAfter = velocity_after_a;
		} else if (name == "Car B Velocity After") {
			velocity_after_b = float.Parse(arg);
			car_B_control.VelocityAfter = velocity_after_b;
		} else if (name == "Result Momentum") {
			momentum_net_user = float.Parse(arg);
		}
		momentum_net = momentum_b + momentum_a;
	}

	public override void OnMenuChanged(JSONNode answer){
		m_answer = answer;
		Debug.Log(m_answer);
	}

	public override void OnSubmit(JSONNode answers){
		m_answer = answers;
		
		if(question["values"]["Result Momentum"]["editable"].Value.Equals("true")){
			if(momentum_net_user != momentum_net){
				wrong = true;
			} 
		} else {//if (question["values"]["Car A Velocity"]["editable"].Value.Equals("true")){
			if(momentum_net != float.Parse(question["values"]["Result Momentum"]["value"])){
				wrong = true;
			}
		}
		if(wrong){
			side_menu.Tries -= 1;
			side_menu.pause();
			if(side_menu.Tries == 0){
				completeGame(m_answer);
			}
			wrong = false;
			// return;
		}
		car_A_control.updateSpeed(velocity_a);
		car_B_control.updateSpeed(velocity_b);
		adjustCamToFit();
		submitted = true;
	}

}
