using UnityEngine;
using System.Collections;
using SimpleJSON;

public class SpringGameController : GameController {

	public EggController m_egg;
	public ShipController m_ship;

	private JSONNode m_answer;

	private int m_max_tries;
	private int m_number_tries;

	public override void initializeGame(JSONNode question, JSONNode previous_answer)
	{
		base.initializeGame(question, previous_answer);
		
		m_answer = previous_answer;
		m_max_tries = question["max_tries"].AsInt;
		side_menu.Tries = m_max_tries;
		m_number_tries = 0;

		//m_egg.
	}

	public override void Update()
	{
		base.Update();
	}
	
	// Shit to do with the overlay menu.
	public override void OnMenuChanged (JSONNode answer) {}
	
	public override void OnSubmit (JSONNode answers) {}

	public override void SetProperty(string name, string arg) {}
	// End shit.

	public void OnSuccess() {
		JSONNode answer_node = m_answer;
		answer_node["values"]["Compression Distance"]["value"].AsFloat = 0.35f;
		answer_node["total_tries"].AsInt = m_number_tries;

		completeGame(answer_node);
	}

	public void OnFailure() {
		m_number_tries++;
		if(m_number_tries >= m_max_tries) {
			OnSuccess();
		}
	}
}
