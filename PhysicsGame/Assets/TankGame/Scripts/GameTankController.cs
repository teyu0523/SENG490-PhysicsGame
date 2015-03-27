using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SimpleJSON;

public class GameTankController : GameController {

	// publics for all game objects effected from question
	public GameObject Tank;
	public GameObject Target;
	public Button m_submit_button;
	public GameObject canvas;

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
		m_answer = previous_answer;

		Debug.Log (question);
		Debug.Log (previous_answer);

		Vector3 newPosition;

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
			//setting game's gravity
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
		/**********************************************
		// may need to add some if statement to determine if button pressed 
		// or new position is different from old position
		side_menu.setString(name, arg); // this well set whatever is change on game to pause menu;
		***********************************************/
		m_question_hint ["Tank Height"].text = "Tank Height: " + Tank.transform.position.y.ToString () + " m";
		m_question_hint ["Angle"].text = "Tank Angle: " + Tank.GetComponent<TankController> ().GetAngle ().ToString () + " degrees"; 
		m_question_hint ["Velocity"].text = "Projectile Veloicty: " + Tank.GetComponent<TankController> ().GetVelocity ().ToString () + " m/s";
		m_question_hint ["Distance"].text = "Distance to Target: " + (Tank.transform.position.x * (-1f)).ToString () + " m";
		m_question_hint ["Target Height"].text = "Target Height: " + Target.transform.position.y.ToString () + " m";

		if (Input.GetKeyDown(KeyCode.P) ){
			side_menu.pause();
			
		}
	}
	
	/* Set the property on tank game from input values on pause menu */
	public override void SetProperty(string name, string arg){

	}



	public void OnSubmitButtonPressed()
	{
		//increament number of tries 
		m_number_tries++;

		//if target was hit or max tries reached
			
			

		
	}

	public void setAnswer(JSONNode answer){
		m_answer = answer;
		Debug.Log(m_answer);
	}
	
}
