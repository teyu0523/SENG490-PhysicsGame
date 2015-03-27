using UnityEngine;
using System.Collections;
using SimpleJSON;

public class SpringGameController : GameController {

	public EggController m_egg;
	public ShipController m_ship;

	private JSONNode m_question;
	private JSONNode m_answer;

	private int m_max_tries;
	private int m_number_tries;
	
	private bool m_fire_pressed = false;

	public override void initializeGame(JSONNode question, JSONNode previous_answer)
	{
		base.initializeGame(question, previous_answer);

		m_question = question;
		m_answer = previous_answer;

		m_max_tries = question["max_tries"].AsInt;
		side_menu.Tries = m_max_tries;
		m_number_tries = 0;

		m_egg.springConstant = question["values"]["Spring Constant"]["value"].AsFloat;
		m_egg.compressPct = question["values"]["Compression Distance"]["value"].AsFloat;
		m_egg.mass = question["values"]["Mass"]["value"].AsFloat;
		m_ship.transform.position = new Vector3(m_ship.transform.position.x, question["values"]["Target Height"]["value"].AsFloat, m_ship.transform.position.z);
		Physics2D.gravity = new Vector2(0.0f, question["values"]["Gravity"]["value"].AsFloat);
	}

	public override void Update()
	{
		base.Update();

		if(Input.GetAxisRaw("Vertical") > 0){
			m_egg.compressPct += Time.deltaTime*0.25f; //compress by 25% per second
			if(m_egg.compressPct > 1.0)
			{
				m_egg.compressPct = 1.0f;
			}
			side_menu.setString("Compression Distance", m_egg.compressPct.ToString());
		} else if(Input.GetAxisRaw("Vertical") < 0){
			m_egg.compressPct -= Time.deltaTime*0.25f; //compress by 25% per second
			if(m_egg.compressPct < 0.0)
			{
				m_egg.compressPct = 0.0f;
			}
			side_menu.setString("Compression Distance", m_egg.compressPct.ToString());
		} else if( (Input.GetAxisRaw("Jump") > 0 || m_fire_pressed) && !m_egg.launched){
			Input.ResetInputAxes();
			m_fire_pressed = false;
			m_egg.Launch(m_egg.compressPct);
			m_egg.compressPct = 0.0f;
		}
	}
	
	public override void OnSubmit (JSONNode answers) {
		m_egg.Launch(m_egg.compressPct);
	}

	public override void SetProperty(string name, string arg)
	{
		if(name == "Spring Constant") {
			m_egg.springConstant = float.Parse(arg);
		}
		else if(name == "Compression Distance") {
			m_egg.compressPct = float.Parse(arg);
		}
		else if(name == "Mass") {
			m_egg.mass = float.Parse(arg);
		}
		else if(name == "Target Height") {
			m_ship.transform.position = new Vector3(m_ship.transform.position.x, float.Parse(arg), m_ship.transform.position.z);
		}
		else if(name == "Gravity") {
			Physics2D.gravity = new Vector2(0.0f, float.Parse(arg));
		}
	}

	public void OnSuccess() {
		JSONNode answer_node = m_answer;
		answer_node["values"]["Compression Distance"]["value"].AsFloat = 0.35f;
		answer_node["total_tries"].AsInt = m_number_tries;

		completeGame(answer_node);
	}

	public void OnFailure() {
		m_number_tries++;
		side_menu.Tries = (m_max_tries - m_number_tries);
		if(m_number_tries >= m_max_tries) {
			OnSuccess();
		}
		else {
			m_egg.resetEgg();
		}
	}

	// Touch Input Functions
	public void OnFirePressed(bool down) {
		m_fire_pressed = down;
	}
	public void OnSliderChanged(float value) {
		m_egg.compressPct = value;
		side_menu.setString("Compression Distance", m_egg.compressPct.ToString());
	}
}
