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
				Debug.Log(loginFailer);
			}
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

}
