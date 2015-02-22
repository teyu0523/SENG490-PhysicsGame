﻿using UnityEngine;
using System.Collections;


public class LoginMenu : MonoBehaviour {
	public NetworkingController netControl;
	private string username; 
	private string password;
	private string passwordMask;
	private Vector2 scrollPosition;
//	private string login_result;
//	private string login_error;

	// Use this for initialization
	void Start () {
		username = "username";
		passwordMask = "password";
		password = "";
		scrollPosition = Vector2.zero;

	}
	
	void OnGUI(){
		GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height));

//		scrollPosition = GUILayout.BeginScrollView (
//			scrollPosition,
//			GUILayout.Width(Screen.width), 
//			GUILayout.Height(Screen.height)
//			);
		GUI.Label (
			new Rect(Screen.width/2-100, Screen.height/2-30, 70, 20), 
			"Username:"); // top -= width/2 to get to center
		GUI.Label (
			new Rect (Screen.width/2-100, Screen.height/2+30, 70, 20), 
		    "Password:");
		GUI.SetNextControlName ("username_val");
		username = GUI.TextField (
			new Rect (Screen.width/2-20, Screen.height/2-30, 120, 20), 
			username);
		GUI.SetNextControlName ("password_val");
		passwordMask = GUI.TextField (
			new Rect (Screen.width/2-20, Screen.height/2+30, 120, 20), 
			passwordMask);

		maskPass ();
		if (GUI.Button (
			new Rect (Screen.width/2-30, Screen.height/2+100, 60, 25), 
		    "Login")) 
		{
			//netControl.Login("geoff", "pass", (login_result, login_error) => {
			netControl.Login(username, password, (login_result, login_error) => {
				if(login_result != "success") {
					Application.LoadLevel ("MainMenu"); 
				} else {
					print(login_error);
				}
			});
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
//		GUILayout.EndScrollView ();
		GUILayout.EndArea ();
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
