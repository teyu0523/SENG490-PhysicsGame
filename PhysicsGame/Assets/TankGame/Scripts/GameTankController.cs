﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SimpleJSON;

public class GameTankController : GameController {

	// publics for all game objects effected from question
	public GameObject Tank;
	public GameObject Target;

	private StatsDisplayPanelController m_question_hint = null;
	
	private int m_max_tries = 0;
	private int m_number_tries = 0;


	private JSONNode m_answer;

	public override void Awake ()
	{
		base.Awake ();
	}
	
	public override void initializeGame(JSONNode question, JSONNode previous_answer)
	{
		base.initializeGame(question, previous_answer);

		m_answer = previous_answer;

		Debug.Log (question);
		Debug.Log (previous_answer);
		
		m_max_tries = question ["max_tries"].AsInt;
		side_menu.Tries = m_max_tries - m_number_tries;


		if (question ["playable"].Value.Equals("false")) {
			Debug.Log ("DISABLING EVERTHING");
			Tank.GetComponent<TankController> ().DisableAllControls ();
		} else {
			Debug.Log ("DISABLING SOME EVERTHINGS");
			if(!question["values"]["Player Distance"]["editable"].AsBool){
				Tank.GetComponent<TankController>().DisableMoveControls();
			}
			if(!question["values"]["Player Height"]["editable"].AsBool){
				//not implemented
			}
			if(!question["values"]["Player Angle"]["editable"].AsBool){
				Tank.GetComponent<TankController>().DisableAngleControls();
			}
			if(!question["values"]["Player Velocity"]["editable"].AsBool){
				Tank.GetComponent<TankController>().DisableVelocityControls();
			}
			if(!question["values"]["Target Height"]["editable"].AsBool){
				//Not implemented
			}
			if(!question["values"]["Gravity"]["editable"].AsBool){
				//Not implemented
			}	
		}

		//setting player distance and height level
		Tank.transform.position = new Vector3 ((question ["values"] ["Player Distance"] ["value"].AsFloat) * (-1f),
		                                       (question["values"]["Player Height"]["value"].AsFloat -0.9f), Tank.transform.position.z);
		Debug.Log (question ["values"] ["Player Height"] ["value"].Value);
		if (question ["values"] ["Player Height"] ["value"].AsFloat <= 0.0f) {
			Debug.Log ("disabling rockets.");
			Tank.GetComponent<TankController>().DisableRockets();
		}

		//setting player's angle
		Tank.GetComponent<TankController>().SetAngle(question["values"]["Player Angle"]["value"].AsFloat);

		//setting player's projectile velocity
		Tank.GetComponent<TankController>().SetVelocity(question["values"]["Player Velocity"]["value"].AsFloat);

		//setting target's height level
		Target.transform.position = new Vector3 (Target.transform.position.x,
		                                        (question ["values"] ["Target Height"] ["value"].AsFloat), Target.transform.position.z);

		//setting ame's gravity
		Physics.gravity = new Vector3(0, question["values"]["Gravity"]["value"].AsFloat, 0);
		
//		//setting up game environment if it is a question
//		if (question ["playable"].Value.Equals("false")) {
//
//			//disable keyboard controls for tank game
//			Tank.GetComponent<TankController>().DisableTankControls();
//			
//			//setting player distance
//			if(!question["values"]["Player Distance"]["editable"].AsBool){
//				newPosition = Tank.transform.position;
//				newPosition.x = (question["values"]["Player Distance"]["value"].AsFloat)*(-1f);
//				Tank.transform.position = newPosition;
//			}
//			//setting player's height level
//			if(!question["values"]["Player Height"]["editable"].AsBool){
//				newPosition = Tank.transform.position;
//				newPosition.y = (question["values"]["Player Height"]["value"].AsFloat);
//				Tank.transform.position = newPosition;
//			}
//			//setting player's angle
//			if(!question["values"]["Player Angle"]["editable"].AsBool){
//				Tank.GetComponent<TankController>().SetAngle(question["values"]["Player Angle"]["value"].AsFloat);
//			}
//			//setting player's projectile velocity
//			if(!question["values"]["Player Velocity"]["editable"].AsBool){
//				Tank.GetComponent<TankController>().SetVelocity(question["values"]["Player Velocity"]["value"].AsFloat);
//			}
//			//setting target's height level
//			if(!question["values"]["Target Height"]["editable"].AsBool){
//				newPosition = Target.transform.position;
//				newPosition.y = (question["values"]["Target Height"]["value"].AsFloat);
//				Target.transform.position = newPosition;
//			}
//			//setting ame's gravity
//			if(!question["values"]["Gravity"]["editable"].AsBool){
//				Physics.gravity = new Vector3(0, question["values"]["Gravity"]["value"].AsFloat, 0);
//			}
//		}

		//spawning info box to tank
		m_question_hint = (GameObject.Instantiate(m_stats_prefab) as GameObject).GetComponent<StatsDisplayPanelController>();
		m_question_hint.AddTextItem ("Tank Height", "Tank Height: " + (Tank.transform.position.y).ToString() + " m");
		m_question_hint.AddTextItem ("Angle", "Tank Angle: " + Tank.GetComponent<TankController>().GetAngle().ToString() + " degrees");
		m_question_hint.AddTextItem ("Velocity", "Projectile Veloicty: " + Tank.GetComponent<TankController>().GetVelocity().ToString() + " m/s");
		m_question_hint.AddTextItem ("Distance", "Distance to Target: " + (Tank.transform.position.x*(-1f)).ToString() + " m");
		m_question_hint.AddTextItem ("Target Height", "Target Height: " + Target.transform.position.y.ToString() + " m");
		m_question_hint.AddTextItem ("Gravity", "Gravity: " + Physics.gravity.y.ToString() + " (m/s)/s");

		m_question_hint.Attach(Tank.gameObject, new Vector2(-4.0f, 1.0f));

	}

	public override void Update()
	{
		base.Update();
		/**********************************************
		// may need to add some if statement to determine if button pressed 
		// or new position is different from old position
		side_menu.setString(name, arg); // this well set whatever is change on game to pause menu;
		***********************************************/
		m_question_hint ["Tank Height"].text = "Tank Height: " + (Tank.GetComponent<TankController>().projectileSpawn.position.y).ToString () + " m";
		m_question_hint ["Angle"].text = "Tank Angle: " + Tank.GetComponent<TankController> ().GetAngle ().ToString () + " degrees"; 
		m_question_hint ["Velocity"].text = "Projectile Veloicty: " + Tank.GetComponent<TankController> ().GetVelocity ().ToString () + " m/s";
		m_question_hint ["Distance"].text = "Distance to Target: " + (Tank.transform.position.x * (-1f)).ToString () + " m";
		m_question_hint ["Target Height"].text = "Target Height: " + Target.transform.position.y.ToString () + " m";
		m_question_hint ["Gravity"].text = "Gravity: " + Physics.gravity.y.ToString () + " (m/s)/s";

		if (Input.GetKeyDown ("p")) {
			side_menu.setString("Player Distance", (Tank.transform.position.x * (-1f)).ToString ());
			side_menu.setString("Player Height", (Tank.GetComponent<TankController>().projectileSpawn.position.y).ToString ());
			side_menu.setString("Player Angle", Tank.GetComponent<TankController> ().GetAngle ().ToString ());
			side_menu.setString("Player Velocity", Tank.GetComponent<TankController> ().GetVelocity ().ToString ());
			side_menu.setString("Target Height", Target.transform.position.y.ToString ());
			side_menu.setString("Gravity", Physics.gravity.y.ToString ());


		}
	}


	
	/* Set the property on tank game from input values on pause menu */
	public override void SetProperty(string name, string arg){

		Vector3 newPosition;

		if (name == "Player Distance") {
			newPosition = Tank.transform.position;
			newPosition.x = (float.Parse(arg))*(-1f);
			Tank.transform.position = newPosition;

		}
		if (name == "Player Height") {
			newPosition = Tank.transform.position;
			newPosition.y = float.Parse(arg) -0.9f;;
			Tank.transform.position = newPosition;
			if (float.Parse(arg) <= 0.0f) {
				Debug.Log ("disabling rockets.");
				Tank.GetComponent<TankController>().DisableRockets();
			}
			else{
				Tank.GetComponent<TankController>().EnableRockets();
			}
		}
		if (name == "Player Angle") {
			Tank.GetComponent<TankController>().SetAngle(float.Parse(arg));
		}
		if (name == "Target Velocity") {
			Tank.GetComponent<TankController>().SetVelocity(float.Parse(arg));
		}
		if (name == "Target Height") {
			newPosition = Target.transform.position;
			newPosition.y = (float.Parse(arg));
			Target.transform.position = newPosition;
		}
		if (name == "Gravity") {
			Physics.gravity = new Vector3(0, float.Parse(arg), 0);
		}

	}

	public void CheckGameStatus(){

		bool targetStatus = Target.GetComponent<DestoryOnContact> ().IsTargetDead ();
		//if target was not hit
		if (!targetStatus) {
			//increament number of tries
			m_number_tries++;
			side_menu.Tries = m_max_tries - m_number_tries;
			//update number of tries text
			
			Debug.Log(m_number_tries);
			Debug.Log(m_max_tries);
		}
		//if target was hit or max tries reached
		if (targetStatus || m_number_tries == m_max_tries) {
			StartCoroutine(Delay());
			GameDone ();
		}
		
	}
	
	public override void OnSubmit(JSONNode answer){
		Tank.GetComponent<TankController> ().Fire ();
	}
	
	public void GameDone()
	{

		JSONNode answer_node = m_answer;

		// save game data to answer json
		answer_node["total_tries"].AsInt = m_number_tries;
		answer_node["values"]["Player Distance"]["value"].AsFloat = Tank.transform.position.x * (-1f);
		answer_node["values"]["Gravity"]["value"].AsFloat = Physics.gravity.y;
		answer_node["values"]["Player Angle"]["value"].AsFloat = Tank.GetComponent<TankController>().GetAngle();
		answer_node["values"]["Target Height"]["value"].AsFloat = Target.transform.position.y;
		answer_node["values"]["Player Velocity"]["value"].AsFloat = Tank.GetComponent<TankController>().GetVelocity();
		answer_node["values"]["Player Height"]["value"].AsFloat = Tank.transform.position.y;
		//call complete game with answer json
		completeGame(answer_node);

	}

	IEnumerator Delay(){
		yield return new WaitForSeconds(2f);
	}


	
}
