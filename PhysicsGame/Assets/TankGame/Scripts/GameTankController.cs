using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SimpleJSON;

public class GameTankController : GameController {

	// publics for all game objects effected from question
	public GameObject Tank;
	public GameObject Target;

	private SideMenu side_menu;

	private StatsDisplayPanelController m_question_hint = null;

	private int m_expected_answer = 0;
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

		Vector3 newPosition;
		
		m_max_tries = question ["max_tries"].AsInt;

		//setting up game environment if it is a question
		if (question ["playable"].Value.Equals("false")) {

			//disable keyboard controls for tank game
			Tank.GetComponent<TankController>().DisableTankControls();
			
			//setting player distance
			if(!question["values"]["Player Distance"]["editable"].AsBool){
				newPosition = Tank.transform.position;
				newPosition.x = (question["values"]["Player Distance"]["value"].AsFloat)*(-1f);
				Tank.transform.position = newPosition;
			}
			//setting player's height level
			if(!question["values"]["Player Height"]["editable"].AsBool){
				newPosition = Tank.transform.position;
				newPosition.y = (question["values"]["Player Height"]["value"].AsFloat);
				Tank.transform.position = newPosition;
			}
			//setting player's angle
			if(!question["values"]["Player Angle"]["editable"].AsBool){
				Tank.GetComponent<TankController>().SetAngle(question["values"]["Player Angle"]["value"].AsFloat);
			}
			//setting player's projectile velocity
			if(!question["values"]["Player Velocity"]["editable"].AsBool){
				Tank.GetComponent<TankController>().SetVelocity(question["values"]["Player Velocity"]["value"].AsFloat);
			}
			//setting target's height level
			if(!question["values"]["Target Height"]["editable"].AsBool){
				newPosition = Target.transform.position;
				newPosition.y = (question["values"]["Player Height"]["value"].AsFloat);
				Target.transform.position = newPosition;
			}
			//setting ame's gravity
			if(!question["values"]["Gravity"]["editable"].AsBool){
				Physics.gravity = new Vector3(0, question["values"]["Gravity"]["value"].AsFloat, 0);
			}
		}

		//spawning info box to tank
		m_question_hint = (GameObject.Instantiate(m_stats_prefab) as GameObject).GetComponent<StatsDisplayPanelController>();
		m_question_hint.AddTextItem ("Tank Height", "Tank Height: " + Tank.transform.position.y.ToString() + " m");
		m_question_hint.AddTextItem ("Angle", "Tank Angle: " + Tank.GetComponent<TankController>().GetAngle().ToString() + " degrees");
		m_question_hint.AddTextItem ("Velocity", "Projectile Veloicty: " + Tank.GetComponent<TankController>().GetVelocity().ToString() + " m/s");
		m_question_hint.AddTextItem ("Distance", "Distance to Target: " + (Tank.transform.position.x*(-1f)).ToString() + " m");
		m_question_hint.AddTextItem ("Target Height", "Target Height: " + Target.transform.position.y.ToString() + " m");
		m_question_hint.Attach(Tank.gameObject, new Vector2(2.0f, 1.0f));


	}

	public override void Update()
	{
		base.Update();

		m_question_hint ["Tank Height"].text = "Tank Height: " + Tank.transform.position.y.ToString () + " m";
		m_question_hint ["Angle"].text = "Tank Angle: " + Tank.GetComponent<TankController> ().GetAngle ().ToString () + " degrees"; 
		m_question_hint ["Velocity"].text = "Projectile Veloicty: " + Tank.GetComponent<TankController> ().GetVelocity ().ToString () + " m/s";
		m_question_hint ["Distance"].text = "Distance to Target: " + (Tank.transform.position.x * (-1f)).ToString () + " m";
		m_question_hint ["Target Height"].text = "Target Height: " + Target.transform.position.y.ToString () + " m";
	}

	public void CheckGameStatus(){
		OnSubmitButtonPressed ();

	}
	
	
	public void OnSubmitButtonPressed()
	{
		bool targetStatus = Target.GetComponent<DestoryOnContact> ().IsTargetDead ();

		//if target was not hit
		if (!targetStatus) {
			//increament number of tries
			m_number_tries++;
			//update number of tries text

			Debug.Log(m_number_tries);
			Debug.Log(m_max_tries);
		}
		//else if target was hit or max tries reached
		if (targetStatus || m_number_tries == m_max_tries) {
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
	}

	public void setAnswer(JSONNode answer){
		m_answer = answer;
		Debug.Log(m_answer);
	}
	
}
