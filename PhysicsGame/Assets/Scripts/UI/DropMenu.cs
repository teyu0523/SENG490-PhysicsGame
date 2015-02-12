using UnityEngine;
using System.Collections;


public class DropMenu : MonoBehaviour {
	public int numQues = 20;
	public GUIStyle styleBox;
	public float heightTextArea ,heightSpace, heightButton;
	public float widthTextArea, widthButton;
	public string[] questions;
	public string[] descriptions;
	public bool[] clicked;
	public float yButton, yBoxArea, yButtonDefault;
	public float totalHeight; // total height of all UI
	public float barValue;	// scroll bar current value
	public Vector2 scrollPosition = Vector2.zero;
	public Vector2[] scrollPositions;

	// Use this for initialization
	public void Start () 
	{
		heightSpace = 10;
		yButtonDefault = 5;
		heightButton = 30;
		barValue = 0;
		questions = new string[numQues];
		descriptions = new string[numQues];
		clicked = new bool[numQues];
		scrollPositions = new Vector2[numQues];
		for(int i=0; i<numQues; i++)
		{
			scrollPositions[i] = Vector2.zero;
		}

		questions[0] = "Click";
		questions[1] = "Click2";
		questions[2] = "Click2";
		questions[3] = "Click2";
		questions[4] = "Click2";
		questions[5] = "Click2";
		questions[6] = "Click2";
		questions[7] = "Click2";
		questions[8] = "Click2";
		questions[9] = "Click2";
		questions[10] = "Click";
		questions[11] = "Click2";
		questions[12] = "Click2";
		questions[13] = "Click2";
		questions[14] = "Click2";
		questions[15] = "Click2";
		questions[16] = "Click2";
		questions[17] = "Click2";
		questions[18] = "Click2";
		questions[19] = "Click5";
		
		descriptions[0] = "danny is bored";
		descriptions[1] = "mikko spenikko spends o spends money\nmikko spends money\nmikko spends money\n";
		descriptions[2] = "mikko spends money\nmikko spends money\nmikko spends money\nmikko spends money\nmikko spends money\n";

	}
	
	// Update is called once per frame
	public void Update() {

	}

	public void OnGUI() 
	{ 
		
		scrollPosition = GUI.BeginScrollView (
			new Rect(0, 0, Screen.width, Screen.height), 
			scrollPosition, 
			new Rect(0, 0, Screen.width-20, yButton)
			);

		heightTextArea = Screen.height/5;
		yButton = 0;
		//yTextArea = yTextAreaDefault;
		widthButton = (float)(Screen.width/2 + Screen.width/4);
		widthTextArea = (float)(Screen.width/2 + Screen.width/4);

		for(int i=0; i<numQues; i++)
		{
			if(GUI.Button(
				new Rect(
					Screen.width/2 - (2*(Screen.width) + Screen.width)/8, 
					yButton,// + barValue + barSize, 
					widthButton, 
					heightButton),
				questions[i]))
			{
				if(clicked[i])
				{
					clicked[i] = false;
				}
				else
				{
					clicked[i] = true;
				}
			}
			else
			{
				yButton += heightButton+3;
				if(clicked[i] == true)
				{
					GUIStyle style = new GUIStyle("areaStyle");
					GUILayout.BeginArea(
						new Rect(
						Screen.width/2 - (2*(Screen.width) + Screen.width)/8, 
						yButton,
						widthTextArea, 
						heightTextArea),
						style
						);

//					GUI.BeginGroup(new Rect(
//						Screen.width/2 - (2*(Screen.width) + Screen.width)/8, 
//						yButton,
//						widthTextArea, 
//						heightTextArea));

					scrollPositions[i] = GUILayout.BeginScrollView(
						scrollPositions[i],
						GUILayout.Width(widthTextArea), 
						GUILayout.Height(heightTextArea));
					GUILayout.Label(
						descriptions[i]);
					GUILayout.EndScrollView();
					GUILayout.EndArea();
					yButton += heightTextArea;
				}
			}
			yButton += heightSpace;
		}
	
		GUI.EndScrollView();
	}

	public void MoveInHierarchy(int delta) {
		int index = transform.GetSiblingIndex();
		transform.SetSiblingIndex (index + delta);
	}
}
