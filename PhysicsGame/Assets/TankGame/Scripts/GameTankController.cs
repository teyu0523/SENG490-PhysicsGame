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

	public override void initializeGame(JSONNode question, JSONNode previous_answer)
	{
		m_answer = previous_answer;

		Debug.Log (question);
		Debug.Log (previous_answer);

		Vector3 newPosition;
		
		//setting up game environment based on recieved JSONNode
		// if not playable
		if (question ["playable"] == "false") {

			//checking player distance
			if(question["values"]["player_distance"]["editable"] == "false"){
				newPosition = Tank.transform.position;
				newPosition.x = (question["values"]["player_distance"]["value"].AsFloat)*(-1f);
				Tank.transform.position = newPosition;
				Tank.GetComponent<TankController>().DisableMovementControls();
			}
			//checking player's height level
			if(question["values"]["player_pos_y"]["editable"] == "false"){
				newPosition = Tank.transform.position;
				newPosition.y = (question["values"]["player_distance"]["value"].AsFloat);
				Tank.transform.position = newPosition;
				Tank.GetComponent<TankController>().DisableMovementControls();
			}
			//checking player's angle
			if(question["values"]["player_angle"]["editable"] == "false"){
				Tank.GetComponent<TankController>().SetAngle(question["values"]["player_angle"]["value"].AsFloat); //NOT WORKING
				Tank.GetComponent<TankController>().DisableAngleControls();
			}
			//checking player's projectile velocity
			if(question["values"]["player_velocity"]["editable"] == "false"){
				Tank.GetComponent<TankController>().SetVelocity(question["values"]["player_velocity"]["value"].AsFloat);
				Tank.GetComponent<TankController>().DiableVelocityControls();
			}
			//checking target's height level
			if(question["values"]["target_pos_y"]["editable"] == "false"){
				newPosition = Target.transform.position;
				newPosition.y = (question["values"]["player_distance"]["value"].AsFloat);
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
