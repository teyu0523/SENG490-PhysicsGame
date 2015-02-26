using UnityEngine;
using System.Collections;
using SimpleJSON;

public class LessonController : MonoBehaviour {

	private bool m_running = false;

	private JSONNode m_lesson = null;
	private int m_question_index = 0;

	private JSONNode m_question = null;
	private JSONNode m_previous_answer = null;


	/// <summary>
	/// Called by the main menu to launch an assignment.
	/// </summary>
	/// <returns><c>true</c>, if assignment was started, <c>false</c> otherwise.</returns>
	/// <param name="lesson_id">The id of lesson to start.</param>
	public bool startLesson(int lesson_id)
	{
		if(m_running)
		{
			return false;
		}
		m_running = true;
		DontDestroyOnLoad(gameObject);

		NetworkingController.Instance.GetLesson(lesson_id, lessonDetailCallback);

		return m_running;
	}


	/// <summary>
	/// Called when the lesson details have been retrieved from the server. Should launch the first question.
	/// </summary>
	/// <param name="result">The result of the WWW call if one exists.</param>
	/// <param name="error">An error message if the WWW call fails.</param>
	private void lessonDetailCallback(string result, string error)
	{
		if(result != null)
		{
			m_lesson = JSON.Parse(result);
			m_question_index = 0;

			prepareNextQuestion();
		}
		else
		{
			Debug.LogError("Request lesson does not exist!");
			m_running = false;
		}
	}


	/// <summary>
	/// Loads the users previous answer for the next question. If all questions have been answered, loads the results screen.
	/// </summary>
	private void prepareNextQuestion() {
		if(m_question_index >= m_lesson["questions"].AsArray.Count) {
			displayLessonResults();
		}
		else 
		{
			int question_id = m_lesson["questions"][m_question_index]["id"].AsInt;

			NetworkingController.Instance.GetPreviousAnswer(question_id, launchNextQuestion);
		}
	}


	/// <summary>
	/// Launches the next question once all data has been gathered from the server.
	/// </summary>
	/// <param name="result">The result of the WWW call if one exists.</param>
	/// <param name="error">An error message if the WWW call fails.</param>
	private void launchNextQuestion(string result, string error) {
		if(result != null)
		{
			m_question = m_lesson["questions"][m_question_index];
			m_previous_answer = JSON.Parse(result);
			
			Application.LoadLevel(m_question["type"]+"Game");
		}
		else
		{
			Debug.LogError(error);
			Debug.LogError("Request question does not exist!");
			m_question_index++;
			prepareNextQuestion();
		}
	}

	public void OnLevelWasLoaded()
	{
		GameObject controller = GameObject.FindGameObjectWithTag("GameController");
		if(controller != null && controller.GetComponent<GameController>() != null)
			controller.GetComponent<GameController>().initializeGame(m_question, m_previous_answer);
		else
			prepareNextQuestion();
	}

	/// <summary>
	/// Submits an answer to a question to the server.
	/// </summary>
	/// <param name="new_answer">A Json object to submit.</param>
	public void submitAnswer(JSONNode new_answer) {
		string answer_string = new_answer.ToString();
		int question_id = m_lesson["questions"][m_question_index]["id"].AsInt;
		NetworkingController.Instance.SubmitAnswer(question_id, answer_string, (result, error) => {
			if(error != null) {
				Debug.LogError("Error Submitting Answer: " + error);
			}

			m_question_index++;
			prepareNextQuestion();
		});
	}


	/// <summary>
	/// Launches the lesson results screen for the user to review.
	/// </summary>
	private void displayLessonResults() {
		Debug.Log("Displaying Lesson Results");
		// TODO
		//Application.LoadLevel("ResultsScreen");
	}
}
