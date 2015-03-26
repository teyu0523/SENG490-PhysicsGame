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
			Debug.Log (question["values"]["Player Distance"]["editable"].Value);
			if(question["values"]["Player Distance"]["editable"].Value.Equals("false")){
				newPosition = Tank.transform.position;
				newPosition.x = (question["values"]["Player Distance"]["value"].AsFloat)*(-1f);
				Tank.transform.position = newPosition;
				Tank.GetComponent<TankController>().DisableMovementControls();
				Debug.Log ("SETTING PLAYER DISTANCE");

			}
			//checking player's height level
			Debug.Log (question["values"]["Player Height"]["editable"].Value);
			if(question["values"]["Player Height"]["editable"].Value.Equals("false")){
				newPosition = Tank.transform.position;
				newPosition.y = (question["values"]["Player Height"]["value"].AsFloat);
				Tank.transform.position = newPosition;
				Tank.GetComponent<TankController>().DisableMovementControls();
				Debug.Log ("SETTING PLAYER HEIGHT");
			}
			//checking player's angle
			Debug.Log (question["values"]["Player Angle"]["editable"].Value);
			if(question["values"]["Player Angle"]["editable"].Value.Equals("false")){
				Tank.GetComponent<TankController>().SetAngle(question["values"]["Player Angle"]["value"].AsFloat); //NOT WORKING
				Tank.GetComponent<TankController>().DisableAngleControls();
				Debug.Log ("SETTING ANGLE");
			}
			//checking player's projectile velocity
			Debug.Log (question["values"]["Player Velocity"]["editable"].Value);
			if(question["values"]["Player Velocity"]["editable"].Value.Equals("false")){
				Tank.GetComponent<TankController>().SetVelocity(question["values"]["Player Velocity"]["value"].AsFloat);
				Tank.GetComponent<TankController>().DisableVelocityControls();
			}
			//checking target's height level
			Debug.Log (question["values"]["Player Height"]["editable"].Value);
			if(question["values"]["Target Height"]["editable"].Value.Equals("false")){
				newPosition = Target.transform.position;
				newPosition.y = (question["values"]["Player Height"]["value"].AsFloat);
				Target.transform.position = newPosition;
			}
		}

		//spawning info box to tank
		m_question_hint = (GameObject.Instantiate(m_stats_prefab) as GameObject).GetComponent<StatsDisplayPanelController>();
		m_question_hint.AddTextItem ("Tank Y", "Tank Height");
		m_question_hint.AddTextItem ("Projectile speed", "Projectile Veloicty");
		m_question_hint.AddTextItem ("distance", "Distance to Target");
		m_question_hint.AddTextItem ("Target Y", "Target Height:");
		m_question_hint.Attach(Tank.gameObject, new Vector2(2.0f, 1.0f));


	}

	public override void Update()
	{
		base.Update();
	}


	public void OnSubmitButtonPressed()
	{

	}

}
