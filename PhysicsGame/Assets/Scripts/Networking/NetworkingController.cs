using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class NetworkingController : MonoBehaviour {

	// Singleton creation, allowing access from anywheres!
	private static NetworkingController m_instance = null;

	public static NetworkingController Instance {
		get{return m_instance;}
	}

	public void Awake() {
		// If this is the first networking controller it becomes the singleton
		// The first networking controller will not be destroyed on a scene change.
		if(m_instance == null) {
			m_instance = this;
			DontDestroyOnLoad(gameObject);
		} else {
			// If this is not the first instance. Destroy it.
			Object.Destroy(gameObject);
		}
	}

	// Set debug to log all server calls.
	public bool debug = false;
	public bool test = false;

	// Root for all server calls.
	public string server = "http://ggollmer.cloudapp.net";

	// Callback after completetion of each server call.
	public delegate void WWWDelegate(string result, string error);

	// Private authentication variables
	private string m_auth_token = null;

	// ========== EXAMPLE CODE ==========
	void Start() {
		if(test) {
			Debug.Log("Attempting connection...");
			// Using a lambda function as a callback for when the WWW request finishes.
			NetworkingController.Instance.Login("geoff", "pass", (login_result, login_error) => {
				if(login_result != null) {
					// Success will be returned as data for now, you do not need to know the token.
					// The authentication information will be stored in this controller.
					Debug.Log(login_result);
					// Calling a function on this object as the callback for when WWW finishes.
					NetworkingController.Instance.GetLessons(lessonsResult);
				} else {
					Debug.LogError(login_error);
				}
			});
		}
	}

	// Example function to fulfill the WWWDelegate callback definition.
	public void lessonsResult(string result, string error) {
		// One arguemt will be null, the other wont. This is how you can detect success and failure.
		if(result != null) {
			Debug.Log(result);

			// I can then convert the JSON object into a set of C# objects to work with via:
			JSONNode course_node = JSON.Parse(result);

			// This reads a specfic value ["x"] is an item from an object [x] is an item from an array.
			int lesson_id = course_node["courses"][0]["lessons"][0]["lesson_id"].AsInt;

			Debug.Log ("Testing Course: " + lesson_id);

			// Now I can get the lesson details.
			NetworkingController.Instance.GetLesson(lesson_id, (lesson_result, lesson_error) => {
				if(lesson_result != null) {
					Debug.Log(lesson_result);
					JSONNode lesson_node = JSON.Parse(lesson_result);
					Debug.Log(lesson_node.ToString());

					// Lets get the question id from the results.
					int question_id = lesson_node["questions"][0]["id"].AsInt;
					Debug.Log("Testing Question: " + question_id);

					// And see if we have answered it before.
					NetworkingController.Instance.GetPreviousAnswer(question_id, (question_result, question_error) => {
						if(question_result != null) {
							Debug.Log(question_result);
						} else {
							Debug.LogError(question_error);
						}
					});

				} else {
					Debug.LogError(lesson_error);
				}
			});

		} else {
			Debug.LogError(error);
		}
	}
	// ========== END EXAMPLE CODE ==========


	/// <summary>
	/// The function called by a login co-routine to hit our endpoint for authenticaion
	/// </summary>
	/// <param name="username">The user's username.</param>
	/// <param name="password">The user's password in plaintext.</param>
	/// <param name="callback">A callback to be called on completion or error.</param>
	public void Login(string username, string password, WWWDelegate callback) {
		StartCoroutine(login_coroutine(username, password, callback));
	}
	private IEnumerator login_coroutine(string username, string password, WWWDelegate callback) {

		WWWForm login_form = new WWWForm();
		login_form.AddField("username", username);
		login_form.AddField("password", password);

		Debug.Log (System.String.Format("{0}/game/auth", server));
		WWW www = new WWW(System.String.Format("{0}/game/auth", server), login_form);
		yield return www;

		/*foreach(string key in www.responseHeaders.Keys) {
			Debug.Log(key + ": " + www.responseHeaders[key]);
		}*/

		if (!string.IsNullOrEmpty(www.error)) {
			log(www.error);

			if(callback != null) {
				callback(null, www.error);
			}
		}
		else 
		{
			JSONNode result = SimpleJSON.JSON.Parse(www.text);
			m_auth_token = result["token"].Value;

			log(www.text);

			if(callback != null) {
				callback("success", null);
			}
		}
	}


	/// <summary>
	/// Gets the logged in user's list of lessons.
	/// </summary>
	/// <param name="callback">The callback to be called on completion or error.</param>
	public void GetLessons(WWWDelegate callback) {
		StartCoroutine(lessons_coroutine(callback));
	}
	private IEnumerator lessons_coroutine(WWWDelegate callback) {
		WWW www = new WWW(System.String.Format("{0}/game/lessons", server), null, generateAuthHeaders());
		yield return www;

		executeCallback(www, callback);
	}


	/// <summary>
	/// Gets the logged in user's lesson specific data.
	/// </summary>
	/// <param name="lesson_id">Lesson_id.</param>
	/// <param name="callback">Callback.</param>
	public void GetLesson(int lesson_id, WWWDelegate callback) {
		StartCoroutine(lesson_coroutine(lesson_id, callback));
	}
	private IEnumerator lesson_coroutine(int lesson_id, WWWDelegate callback) {
		log(System.String.Format("{0}/game/lesson/{1}/", server, lesson_id));
		WWW www = new WWW(System.String.Format("{0}/game/lesson/{1}/", server, lesson_id), null, generateAuthHeaders());
		yield return www;

		executeCallback(www, callback);
	}


	/// <summary>
	/// Gets the logged in user's previous answer to the specific question. An answer will be generated if none has been given.
	/// </summary>
	/// <param name="question_id">The id of the question.</param>
	/// <param name="callback">The callback to be called on completion or error</param>
	public void GetPreviousAnswer(int question_id, WWWDelegate callback) {
		StartCoroutine(get_answer_coroutine(question_id, callback));
	}
	private IEnumerator get_answer_coroutine(int question_id, WWWDelegate callback) {
		WWW www = new WWW(System.String.Format("{0}/game/lesson/answer/{1}/", server, question_id), null, generateAuthHeaders());
		yield return www;
		
		executeCallback(www, callback);
	}


	/// <summary>
	/// Submits an answer to the server for a specific question. The server should validate the provided answer.
	/// </summary>
	/// <param name="question_id">The id of the question..</param>
	/// <param name="answer_json">The string representation of the JSON answer format.</param>
	/// <param name="callback">The callback to be called on completion or error.</param>
	public void SubmitAnswer(int question_id, string answer_json, WWWDelegate callback) {
		StartCoroutine(set_answer_coroutine(question_id, answer_json, callback));
	}
	private IEnumerator set_answer_coroutine(int question_id, string answer_json, WWWDelegate callback) {
		WWW www = new WWW(System.String.Format("{0}/game/lesson/answer/{1}/", server, question_id), System.Text.Encoding.UTF8.GetBytes(answer_json), generateJSONPostHeaders());
		yield return www;
		
		executeCallback(www, callback);
	}


	/// <summary>
	/// Generates auth headers for a WWW request.
	/// </summary>
	/// <returns>The generated auth headers.</returns>
	private Dictionary<string, string> generateAuthHeaders() {
		Dictionary<string, string> headers = new Dictionary<string, string>();
		headers.Add("Authorization", "Token " + m_auth_token);
		return headers;
	}

	/// <summary>
	/// Generates json content type auth headers for a WWW request.
	/// </summary>
	/// <returns>The generated headers.</returns>
	private Dictionary<string, string> generateJSONPostHeaders() {
		Dictionary<string, string> headers = generateAuthHeaders();
		headers.Add("Content-Type", "application/json");
		return headers;
	}

	/// <summary>
	/// Correctly fills in a callback with no extra processing.
	/// </summary>
	/// <param name="www_result">A correctly run WWW object</param>
	/// <param name="callback">A Callback to call with the results of the www object.</param>
	private void executeCallback(WWW www_result, WWWDelegate callback) {
		if (!string.IsNullOrEmpty(www_result.error)) {
			log(www_result.error);
			
			if(callback != null) {
				callback(null, www_result.error);
			}
		}
		else 
		{	
			log(www_result.text);
			
			if(callback != null) {
				callback(www_result.text, null);
			}
		}
	}


	/// <summary>
	/// A quick helper to allow easily disabling of logging on the networking controller.
	/// </summary>
	/// <param name="data">Data.</param>
	private void log(string data) {;
		if(debug) {
			Debug.Log(data);
		}
	}
}
