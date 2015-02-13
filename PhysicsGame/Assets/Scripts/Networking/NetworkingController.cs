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
		Debug.Log(m_instance);
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

	// Root for all server calls.
	public string server = "http://ggollmer.cloudapp.net";

	// Callback after completetion of each server call.
	public delegate void WWWDelegate(string result, string error);

	// Private authentication variables
	private string m_auth_token = null;

	// ========== EXAMPLE CODE ==========
	void Start() {

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

	// Example function to fulfill the WWWDelegate callback definition.
	public void lessonsResult(string result, string error) {
		// One arguemt will be null, the other wont. This is how you can detect success and failure.
		if(result != null) {
			Debug.Log(result);
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

		WWW www = new WWW(server + "/game/auth", login_form);
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
	/// Gets the logged in users list of lessons.
	/// </summary>
	/// <returns>A json structure representing the users lessons</returns>
	/// <param name="callback">The callback to be called on completion or error.</param>
	public void GetLessons(WWWDelegate callback) {
		StartCoroutine(lessons_coroutine(callback));
	}
	private IEnumerator lessons_coroutine(WWWDelegate callback) {
		WWW www = new WWW(server + "/game/lessons", null, generateAuthHeaders());
		yield return www;

		if (!string.IsNullOrEmpty(www.error)) {
			log(www.error);
			
			if(callback != null) {
				callback(null, www.error);
			}
		}
		else 
		{	
			log(www.text);
			
			if(callback != null) {
				callback(www.text, null);
			}
		}
	}

	private Dictionary<string, string> generateAuthHeaders() {
		Dictionary<string, string> headers = new Dictionary<string, string>();
		headers.Add("Authorization", "Token " + m_auth_token);
		return headers;
	}

	private void log(string data) {;
		if(debug) {
			Debug.Log(data);
		}
	}
}
