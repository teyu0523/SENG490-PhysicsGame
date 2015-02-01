using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class NetworkingController : MonoBehaviour {

	// Singleton creation, allowing access from anywheres!
	private NetworkingController m_instance;

	public NetworkingController instance {
		get{return m_instance;}
	}

	public void Awake() {
		m_instance = this;
	}

	// Set debug to log all server calls.
	public bool debug = false;

	// Root for all server calls.
	public string server = "http://ggollmer.cloudapp.net";

	// Callback after completetion of each server call.
	public delegate void WWWDelegate(string result, string error);

	// Private authentication variables
	private string m_auth_token = null;
		
	IEnumerator Start() {
		yield return StartCoroutine(login("geoff", "pass", null));
		yield return StartCoroutine(getLessons(null));
	}

	public IEnumerator login(string username, string password, WWWDelegate callback) {

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

	public IEnumerator getLessons(WWWDelegate callback) {
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
