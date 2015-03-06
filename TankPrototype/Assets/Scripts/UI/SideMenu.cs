using UnityEngine;
using System.Collections;

public class SideMenu : MonoBehaviour {

	public GUIStyle styleArea;
	public Texture2D texArea = null;
	private bool buttonPress = false;

	// Use this for initialization
	void Start () {
		if (texArea != null){
			styleArea.normal.background = texArea;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.P))
		{
			if(buttonPress){
				Time.timeScale = 1;
				buttonPress = false;
			} else {
				Time.timeScale = 0;
				buttonPress = true;
			}
		}
	}

	void OnGUI() 
	{
		if(buttonPress)
		{
			//GUILayout.BeginArea(new Rect(Screen.width/2,0,Screen.width/2,Screen.height),styleArea);
			GUILayout.BeginArea(new Rect(Screen.width/2,20,Screen.width/2,Screen.height-20),styleArea);
			GUILayout.BeginHorizontal();
			GUILayout.Label("lalala",styleArea);
			GUILayout.Label("lalala2",styleArea);
			GUILayout.EndHorizontal();    
			GUILayout.EndArea();
			//	GUILayout.EndArea();
		}
	}
}
