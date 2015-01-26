using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class NetworkingController : MonoBehaviour {
	public string server = "http://ggollmer.cloudapp.net";

	private string m_auth_token = null;
		
	IEnumerator Start() {
		/*Debug.Log("Making Test Call");
		WWW www = new WWW(url);
		yield return www;
		Debug.Log(www.text);*/

		yield return StartCoroutine(login());
		yield return StartCoroutine(test());
	}

	IEnumerator login() {
		Debug.Log("Making Auth Call");
		WWWForm login_form = new WWWForm();
		login_form.AddField("username", "geoff");
		login_form.AddField("password", "pass");

		WWW www = new WWW(server + "/game/auth", login_form);
		yield return www;

		if (!string.IsNullOrEmpty(www.error)) {
			Debug.Log(www.error);
		}
		else 
		{
			JSONNode result = SimpleJSON.JSON.Parse(www.text);
			m_auth_token = result["token"].Value;
		}

	}

	IEnumerator test() {
		Debug.Log("Making Auth Test Call");
		WWW www = new WWW(server + "/game/test", null, generateAuthHeaders());
		yield return www;

		if (!string.IsNullOrEmpty(www.error)) {
			Debug.Log(www.error);
		}
		else 
		{

		}
		Debug.Log(www.text);
	}

	private Dictionary<string, string> generateAuthHeaders() {
		Dictionary<string, string> headers = new Dictionary<string, string>();
		headers.Add("Authorization", "Token " + m_auth_token);
		return headers;
	}
}
