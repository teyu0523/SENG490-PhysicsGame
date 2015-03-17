using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class GameCollisionController : GameController {

	public GameObject m_stats_prefab = null;
	private LessonController m_assignment_controller = null;
	private static GameController m_instance = null;

	private List<StatsDisplayPanelController> m_stats_displays;
	private bool m_displaying_stats = false;
	private bool m_touch_started = false;

	/// <summary>
	/// Awake for this controller.
	/// </summary>
	public override void Awake() {
		GameObject assignment_controller_object = GameObject.FindGameObjectWithTag("LessonController");
		if(assignment_controller_object != null) 
		{
			m_assignment_controller = assignment_controller_object.GetComponent<LessonController>();
		}
		GameController.m_instance = this;
		m_stats_displays = new List<StatsDisplayPanelController>();
	}


	/// <summary>
	/// Initializes the game.
	/// </summary>
	/// <param name="question">The JSON data for the question.</param>
	/// <param name="previous_answer">The JSON data containing the user's previous answer.</param>
	public override void initializeGame(JSONNode question, JSONNode previous_answer){
		
	}


	/// <summary>
	/// Call to complete the currently running game.
	/// </summary>
	/// <param name="new_answer">The JSON data for the new answer.</param>
	public void completeGame(JSONNode new_answer)
	{
		if(m_assignment_controller != null)
		{
			m_assignment_controller.submitAnswer(new_answer);
		}
		else
		{

		}
		GameController.m_instance = null;
	}

	/// <summary>
	/// Adds a stats display to the listener list. When a gesture is detected displays will be swapped on and off.
	/// </summary>
	/// <param name="display">A Stats display object that hasn't subscribed to the game controller.</param>
	public void addStatsDisplay(StatsDisplayPanelController display) {
		m_stats_displays.Add(display);
	}

	/// <summary>
	/// Updates once every tick. Looks for touch input or keyboard input to display scenario information.
	/// </summary>
	public override void Update()
	{
		if (Input.GetKeyDown("i") || Input.touchCount > 2) {
			if(!m_touch_started) {
				m_displaying_stats = !m_displaying_stats;
				foreach(StatsDisplayPanelController display in m_stats_displays) {
					display.display(m_displaying_stats);
				}
			}

			m_touch_started = true;
		} else {
			m_touch_started = false;
		}
	}
}
