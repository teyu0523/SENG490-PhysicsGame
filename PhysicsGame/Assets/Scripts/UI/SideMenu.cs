using UnityEngine;
using System.Collections;
using SimpleJSON;

public class SideMenu : MonoBehaviour {
	public float moveLeft;
	public GUIStyle guistyleArea;
	//public Texture2D texArea = null;
	public GUIStyle guistyleParameter;
	private bool buttonPress = false;
	private bool showSide = false;
	private Vector2 scrollPosition = Vector2.zero;
	// Use this for initialization
	void Start () {
		moveLeft = -1;
	}
	
	public void pause(JSONNode questions){

	}

	// Update is called once per frame
	void Update () {
		if(moveLeft==-1){
			moveLeft = Screen.width;
		}
		if(buttonPress == true)
		{
			if(moveLeft > Screen.width/2){
				moveLeft -= 10;
			}else{
				moveLeft = Screen.width/2;
			}
		} else {
			if(moveLeft < Screen.width){
				moveLeft += 10;
			}else{
				moveLeft = Screen.width;
				showSide = false;
			}
		}

	}

	void OnGUI() 
	{
		if(showSide)
		{
			//GUILayout.BeginArea(new Rect(moveLeft,0,Screen.width/2,Screen.height),guistyleArea);
			GUILayout.BeginArea(new Rect(moveLeft+20,20,Screen.width/2-20,Screen.height-40),guistyleArea);
			scrollPosition = GUILayout.BeginScrollView(scrollPosition);
			GUILayout.Space(10);
			for(int i=0; i<50; i++)
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label("lalala",guistyleParameter);
				GUILayout.TextField("?");
				GUILayout.EndHorizontal(); 
				GUILayout.Space(10);
			}
			GUILayout.EndScrollView();
			GUILayout.EndArea();
			//GUILayout.EndArea();
		}
	}
}
