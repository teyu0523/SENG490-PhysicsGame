using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LoginMenu : MonoBehaviour {
	private string username; 
	private string password;
	private string passwordMask;
	public Text error_message;
	//private Vector2 scrollPosition;
	private string loginFailer;
	private bool draw_gui = true;
	public GUIStyle guiStyle;
	public GUISkin mySkin = null;// = new GUISkin("areaStyle");

	

	public float native_width = 480;
	public float native_height = 320;

  	private float scale_width;
 	private float scale_height;



	// Use this for initialization
	void Start () {

		username = "";
		password = "";
		//scrollPosition = Vector2.zero;
		loginFailer = "";


	}

	public void login(){
		draw_gui = false;
		LoadingController.Instance.show();
		Debug.Log(password);
		Debug.Log(username);
		NetworkingController.Instance.Login(username, password, (login_result, login_error) => {
			LoadingController.Instance.hide();
			if(login_result == "success") {
				Application.LoadLevel ("MainMenu"); 
			} else {
				draw_gui = true;
				loginFailer = login_error;
			}
			Debug.Log(loginFailer);
		});
	}

	public void setUsername(InputField username_field){
		username = username_field.text;
	}

	public void setPassword(InputField password_field){
		password = password_field.text;
	}
 
	void Update () {
		if(loginFailer != "")
		{
			if(loginFailer == "couldn't connect to host")
			{
				loginFailer = "Currently there are server technical difficulties, thank you for your patience.";
			}
			else if(loginFailer == "400 BAD REQUEST")
			{
			
				loginFailer = "Incorrect username or password";
			}
		}
		error_message.text = loginFailer;
	}


	/*public void Awake()
	{
		float targetWidth = (float)Screen.width;
	    float targetHeight = (float)Screen.height;

	    this.baseAspect = this.baseWidth / this.baseHeight;
	    this.targetAspect = targetWidth / targetHeight;

	    float factor = this.targetAspect > this.baseAspect ? targetHeight / this.baseHeight : targetWidth / this.baseWidth;

	    this.m33 = 1 / factor;
	    this.m03 = (targetWidth - this.baseWidth * factor) / 2 * this.m33;
    	this.m13 = (targetHeight - this.baseHeight * factor) / 2 * this.m33;
	}

	void OnGUI(){
		
     	Matrix4x4 gui_matrix = GUI.matrix;

	    gui_matrix.m33 = this.m33;

	    if(this.targetAspect > this.baseAspect) gui_matrix.m03 = this.m03;
	    else gui_matrix.m13 = this.m13;

	    GUI.matrix = gui_matrix;
	
		if(!draw_gui) {
			return;
		}

		if(mySkin != null) {
			GUI.skin = mySkin;
		}

		GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height));
		if(loginFailer != "")
		{
			if(loginFailer == "couldn't connect to host")
			{
				loginFailer = "Currently there are server technical difficulties, thank you for your patience.";
			}
			else if(loginFailer == "400 BAD REQUEST")
			{
			
				loginFailer = "Incorrect username or password";
			}
		}
		
		GUI.Label(
			new Rect(Screen.width/2-225, Screen.height/2-80,450,20),
			loginFailer, guiStyle);
//		scrollPosition = GUILayout.BeginScrollView (
//			scrollPosition,
//			GUILayout.Width(Screen.width), 
//			GUILayout.Height(Screen.height)
//			);

		GUI.Label (
			new Rect(Screen.width/2-100, Screen.height/2-30, 90, 25), 
			"Username"); // top -= width/2 to get to center
		GUI.Label (
			new Rect (Screen.width/2-100, Screen.height/2+30, 90, 25), 
		    "Password");
		GUI.SetNextControlName ("username_val");
		username = GUI.TextField (
			new Rect (Screen.width/2+10, Screen.height/2-24, 120, 20), 
			username);
		GUI.SetNextControlName ("password_val");
		passwordMask = GUI.TextField (
			new Rect (Screen.width/2+10, Screen.height/2+36, 120, 20), 
			passwordMask);

		maskPass ();
		if (GUI.Button (
			new Rect (Screen.width/2-40, Screen.height/2+100, 80, 38), 
		    "Login")) 
		{
			draw_gui = false;
			LoadingController.Instance.show();
			NetworkingController.Instance.Login(username, password, (login_result, login_error) => {
				LoadingController.Instance.hide();
				if(login_result == "success") {
					Application.LoadLevel ("MainMenu"); 
				} else {
					draw_gui = true;
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
	}*/
	
	// Update is called once per frame
	

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
