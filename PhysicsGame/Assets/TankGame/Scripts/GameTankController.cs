using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SimpleJSON;

public class GameTankController : GameController {

	// publics for all game objects effected from question
	public Transform Tank;
	public Transform Target;
	public Transform LeftMarker;
	public BoxCollider2D Boundary;

	//
	public Button m_submit_button;

	private StatsDisplayPanelController m_question_hint = null;

	private int m_expected_answer = 0;
	private int m_max_tries = 0;

	private JSONNode m_answer;

	public override void initializeGame(JSONNode Setup, JSONNode previous_answer)
	{
		m_answer = previous_answer;

		Debug.Log (Setup);

		//setting up game environment based on recieved JSONNode
		// if not practice

			//if JSON distance
				//move tank -x
				//move left marker -x - some margin
				//move Boundary x accordingly
			//if JSON tank y
				//move tank y

			//if JSON target y
				//move target y
			
			//if tank y > target y
				//move Boundary y accordingly 

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
