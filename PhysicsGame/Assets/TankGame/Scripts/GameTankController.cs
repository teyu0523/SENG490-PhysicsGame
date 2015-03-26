using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SimpleJSON;

public class GameTankController : GameController {

	// publics for all game objects effected from question
	public GameObject Tank;
	public GameObject Target;

	//
	public Button m_submit_button;

	private StatsDisplayPanelController m_question_hint = null;

	private int m_expected_answer = 0;
	private int m_max_tries = 0;

	private JSONNode m_answer;

	public override void Awake ()
	{
		base.Awake ();
	}
	
	public override void initializeGame(JSONNode question, JSONNode previous_answer)
	{
		m_answer = previous_answer;

		Debug.Log (question);
		Debug.Log (previous_answer);

		Vector3 newPosition;
		
		//setting up game environment based on recieved JSONNode
		// if not playable
		if (question ["playable"].Value.Equals("false")) {

			//checking player distance
			if(!question["values"]["Player Distance"]["editable"].AsBool){
				newPosition = Tank.transform.position;
				newPosition.x = (question["values"]["Player Distance"]["value"].AsFloat)*(-1f);
				Tank.transform.position = newPosition;
				Tank.GetComponent<TankController>().DisableMovementControls();
			}
			//checking player's height level
			if(!question["values"]["Player Height"]["editable"].AsBool){
				newPosition = Tank.transform.position;
				newPosition.y = (question["values"]["Player Height"]["value"].AsFloat);
				Tank.transform.position = newPosition;
				Tank.GetComponent<TankController>().DisableMovementControls();
			}
			//checking player's angle
			if(!question["values"]["Player Angle"]["editable"].AsBool){
				Tank.GetComponent<TankController>().SetAngle(question["values"]["Player Angle"]["value"].AsFloat);
				Tank.GetComponent<TankController>().DisableAngleControls();
			}
			//checking player's projectile velocity
			if(!question["values"]["Player Velocity"]["editable"].AsBool){
				Tank.GetComponent<TankController>().SetVelocity(question["values"]["Player Velocity"]["value"].AsFloat);
				Tank.GetComponent<TankController>().DisableVelocityControls();
			}
			//checking target's height level
			if(!question["values"]["Target Height"]["editable"].AsBool){
				newPosition = Target.transform.position;
				newPosition.y = (question["values"]["Player Height"]["value"].AsFloat);
				Target.transform.position = newPosition;
			}
			//checking Game's gravity
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


	public void OnSubmitButtonPressed()
	{
		
	}

}
