using UnityEngine;
using System.Collections;


public class LoginMenu : MonoBehaviour {
	private string username; 
	private string password;
	private string passwordMask;
	private Vector2 scrollPosition;
	private string loginFailer;
//	private string login_result;
//	private string login_error;

	// Use this for initialization
	void Start () {
		username = "Username";
		passwordMask = "Password";
		password = "";
		scrollPosition = Vector2.zero;
		loginFailer = "";
	}
	
	void OnGUI(){
		GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height));
		if(loginFailer != "")
		{
			if(loginFailer == "couldn't connect to host")
			{
				loginFailer = "Currently there are server technical difficulties, thank you for your patience.";
			}
//			else if(loginFailer == "")
//			{
//			
//			}
		}
		
		GUI.Label(
			new Rect(Screen.width/2-200, Screen.height/2-80,450,20),
			loginFailer);
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
			NetworkingController.Instance.Login(username, password, (login_result, login_error) => {
				if(login_result == "success") {
					//networkingController.GetLessons(networkingController.lessonsResult);
					Application.LoadLevel ("MainMenu"); 
				} else {
					loginFailer = login_error;
				}
			});
		}
		if (UnityEngine.Event.current.type == EventType.Repaint)
		{
			if (GUI.GetNameOfFocusedControl () == "password_val")
			{
				if (passwordMask == "Password") passwordMask = "";
			}
			else
			{
				if (passwordMask == "") passwordMask = "Password";
			}
			if (GUI.GetNameOfFocusedControl () == "username_val")
			{
				if (username == "Username") username = "";
			}
			else
			{
				if (username == "") username = "Username";
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
		if (passwordMask == "Password") 
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
