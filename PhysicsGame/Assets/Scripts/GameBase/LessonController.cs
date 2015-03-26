using UnityEngine;
using System.Collections;
using SimpleJSON;

public class LessonController : MonoBehaviour {

	private bool m_running = false;

	private int m_course_id = 0;
	private int m_lesson_id = 0;
	private JSONNode m_lesson = null;
	private JSONNode m_result = null;
	private int m_question_index = 0;

	private JSONNode m_question = null;
	private JSONNode m_previous_answer = null;


	/// <summary>
	/// Called by the main menu to launch an assignment.
	/// </summary>
	/// <returns><c>true</c>, if assignment was started, <c>false</c> otherwise.</returns>
	/// <param name="lesson_id">The id of lesson to start.</param>
	public bool startLesson(int course_id, int lesson_id)
	{
		if(m_running)
		{
			return false;
		}
		m_running = true;
		m_lesson_id = lesson_id;
		m_course_id = course_id;
		DontDestroyOnLoad(gameObject);

		LoadingController.Instance.show ();
		NetworkingController.Instance.GetLesson(course_id, lesson_id, lessonDetailCallback);

		return m_running;
	}


	/// <summary>
	/// Called when the lesson details have been retrieved from the server. Should launch the first question.
	/// </summary>
	/// <param name="result">The result of the WWW call if one exists.</param>
	/// <param name="error">An error message if the WWW call fails.</param>
	private void lessonDetailCallback(string result, string error)
	{
		LoadingController.Instance.hide();
		if(result != null)
		{
			m_lesson = JSON.Parse(result);
			Debug.Log (m_lesson);
			m_question_index = 0;

			prepareNextQuestion();
		}
		else
		{
			Debug.LogError("Request lesson does not exist!");
			Debug.LogError(error);
			m_running = false;
		}
	}


	/// <summary>
	/// Loads the users previous answer for the next question. If all questions have been answered, loads the results screen.
	/// </summary>
	private void prepareNextQuestion() {
		LoadingController.Instance.show ();
		if(m_question_index >= m_lesson["questions"].AsArray.Count) {
			NetworkingController.Instance.GetLessonResults(m_course_id, m_lesson_id, displayLessonResults);
		}
		else 
		{
			int question_id = m_lesson["questions"][m_question_index]["id"].AsInt;

			NetworkingController.Instance.GetPreviousAnswer(m_course_id, question_id, launchNextQuestion);
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
			LoadingController.Instance.hide();
			Debug.LogError(error);
			Debug.LogError("Request question does not exist!");
			m_question_index++;
			prepareNextQuestion();
		}
	}

	/// <summary>
	/// Called when a level completes loading. Deals with populating the new scene with lesson data.
	/// </summary>
	public void OnLevelWasLoaded()
	{
		LoadingController.Instance.hide ();
		if(m_question_index < m_lesson["questions"].AsArray.Count) {
			GameObject controller = GameObject.FindGameObjectWithTag("GameController");
			if(controller != null && controller.GetComponent<GameController>() != null)
				controller.GetComponent<GameController>().initializeGame(m_question, m_previous_answer);
			else
				prepareNextQuestion();
		}
		else if(m_question_index == m_lesson["questions"].AsArray.Count) {
			GameObject controller = GameObject.FindGameObjectWithTag("GameController");
			if(controller != null && controller.GetComponent<LessonReviewController>() != null)
			{
				controller.GetComponent<LessonReviewController>().populateResults(m_result);
				Object.Destroy(gameObject);
			}
			else
			{
				Debug.Log("Lesson Results scene not properly loaded!");
				m_question_index++;
				Application.LoadLevel("MainMenu");
				Object.Destroy(gameObject);
			}
		}
		else
			Object.Destroy(gameObject);
	}

	/// <summary>
	/// Submits an answer to a question to the server.
	/// </summary>
	/// <param name="new_answer">A Json object to submit.</param>
	public void submitAnswer(JSONNode new_answer) {
		LoadingController.Instance.show ();
		string answer_string = new_answer.ToString();
		int question_id = m_lesson["questions"][m_question_index]["id"].AsInt;
		NetworkingController.Instance.SubmitAnswer(m_course_id, question_id, answer_string, (result, error) => {
			LoadingController.Instance.hide ();
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
	private void displayLessonResults(string result, string error) {
		LoadingController.Instance.hide ();
		if(result != null)
		{
			m_result = JSON.Parse(result);
			LoadingController.Instance.show ();
			Application.LoadLevel("LessonReview");
		}
		else
		{
			Debug.Log("Lesson Results do not exist!");
			m_question_index++;
			Application.LoadLevel("MainMenu");
			Object.Destroy(gameObject);
		}
	}
}
