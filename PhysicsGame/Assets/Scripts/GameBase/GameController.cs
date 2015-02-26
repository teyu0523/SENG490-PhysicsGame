using UnityEngine;
using System.Collections;
using SimpleJSON;

public abstract class GameController : MonoBehaviour {

	private LessonController m_assignment_controller = null;
	private static GameController m_instance = null;


	/// <summary>
	/// Used to get the controller for the currently running game.
	/// </summary>
	/// <value>The instance.</value>
	public static GameController Instance {
		get{ return m_instance; }
	}


	/// <summary>
	/// Awake for this controller.
	/// </summary>
	public virtual void Awake() {
		GameObject assignment_controller_object = GameObject.FindGameObjectWithTag("LessonController");
		if(assignment_controller_object != null) 
		{
			m_assignment_controller = assignment_controller_object.GetComponent<LessonController>();
		}
		GameController.m_instance = this;
	}


	/// <summary>
	/// Initializes the game.
	/// </summary>
	/// <param name="question">The JSON data for the question.</param>
	/// <param name="previous_answer">The JSON data containing the user's previous answer.</param>
	public abstract void initializeGame(JSONNode question, JSONNode previous_answer);


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
}
