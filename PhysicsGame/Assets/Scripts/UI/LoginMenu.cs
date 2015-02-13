using UnityEngine;
using System.Collections;


public class LoginMenu : MonoBehaviour {
	private string username; 
	private string password;
	private string passwordMask;
	private Vector2 scrollPosition;
	
	// Use this for initialization
	void Start () {
		username = "username";
		passwordMask = "password";
		password = "";
		scrollPosition = Vector2.zero;
	}
	
	void OnGUI(){
		scrollPosition = GUI.BeginScrollView(
			new Rect(0,0, Screen.width, Screen.height), 
			scrollPosition, 
			new Rect(0, 0, Screen.width, Screen.height));
		GUI.Label (
			new Rect(Screen.width/2-35-50, Screen.height/2, 70, 20), 
			"Username:"); // top -= width/2 to get to center
		GUI.Label (
			new Rect (Screen.width/2-35-50, Screen.height/2+60, 70, 20), 
		    "Password:");
		GUI.SetNextControlName ("username_val");
		username = GUI.TextField (
			new Rect (Screen.width/2-35+50, Screen.height/2, 100, 20), 
			username);
		GUI.SetNextControlName ("password_val");
		passwordMask = GUI.TextField (
			new Rect (Screen.width/2-35+50, Screen.height/2+60, 100, 20), 
			passwordMask);

		maskPass ();
		if (GUI.Button (
			new Rect (Screen.width / 2 - 30, Screen.height / 2 + 130, 60, 25), 
		    "Login")) 
		{
			print (password);
			NetworkingController.Instance.Login(username, password, null);
		}


		if (UnityEngine.Event.current.type == EventType.Repaint)
		{
			if (GUI.GetNameOfFocusedControl () == "password_val")
			{
				if (passwordMask == "password") passwordMask = "";
			}
			else
			{
				if (passwordMask == "") passwordMask = "password";
			}
			if (GUI.GetNameOfFocusedControl () == "username_val")
			{
				if (username == "username") username = "";
			}
			else
			{
				if (username == "") username = "username";
			}
		}
		GUI.EndScrollView ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void maskPass()
	{
		if (passwordMask == "password") 
		{
			return;
		}
		if(passwordMask.Length == password.Length+1)
		{
			password += passwordMask[passwordMask.Length-1];
		}
		else if (passwordMask.Length != password.Length) 
		{
			password = passwordMask;
		}
		passwordMask = "";
		for (int i=0; i<password.Length;i++) {
			passwordMask += "*";
		}
	}
}
