using UnityEngine;
using System.Collections;


public class DropMenu : MonoBehaviour {
	public delegate void ClickAction();
	//public static event ClickAction OnClicked;
	int numQues = 2;
	//int yTextArea;
	float heightTextArea;
	float heightButton;
	float widthTextArea;
	float widthButton;
	string[] questions;
	string[] descriptions;
	bool[] clicked;
	float yButton, yTextArea;
	float yButtonDefault;
	float totalHeight; // total height of all UI
	float barValue;	// scroll bar current value
	float barSize;
	
	// Use this for initialization
	void Start () 
	{
		yButtonDefault = 5;
		//yTextAreaDefault = yButton;
		heightTextArea = 500;
		heightButton = 30;
		barValue = 0;
		barSize = 0;
		questions = new string[numQues];
		descriptions = new string[numQues];
		clicked = new bool[numQues];

		questions[0] = "Click";
		questions[1] = "Click2";
//		questions[2] = "Click3";
//		questions[3] = "Click4";
//		questions[4] = "Click5";
		descriptions[0] = "danny is bored";
		descriptions[1] = "mikko spends money";
	}
	
	// Update is called once per frame
	void Update() {

	}


	void OnGUI() 
	{ 
		yButton = yButtonDefault;
		totalHeight = yButton;
		//yTextArea = yTextAreaDefault;
		widthButton = (float)(Screen.width/2 + Screen.width/4);
		widthTextArea = (float)(Screen.width/2 + Screen.width/4);
		for(int i=0; i<numQues; i++)
		{
			if(GUI.Button(new Rect(Screen.width/2 - (2*(Screen.width) + Screen.width)/8, yButton + barValue + barSize, widthButton, heightButton), questions[i]))
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
				if(clicked[i] == true)
				{
					yTextArea = yButton+heightButton;
					GUI.Box(new Rect(Screen.width/2 - (2*(Screen.width) + Screen.width)/8, yTextArea + barValue + barSize, widthTextArea, heightTextArea), descriptions[i]);
					yButton += heightTextArea+heightButton;
					totalHeight += heightTextArea;
					totalHeight += heightButton;
					
				}
				else
				{
					yButton += heightButton;
					totalHeight += heightButton;
				}
			}
		}
		if(Screen.height < totalHeight)
		{
			barSize = 50;			
			barValue = GUI.VerticalScrollbar(
				new Rect(
					(Screen.width/2 + (2*(Screen.width) + Screen.width)/8)+10,
					5,
					10,
					Screen.height - 10),
				barValue,
				(barSize),///100) * (totalHeight-Screen.height),
				0,
				-1*(totalHeight+5+barSize-Screen.height)); //20 for span at bottom
			print (barValue);
		} else {
			barValue = 0;
			barSize = 0;
		}

		print(Screen.height);
		print(totalHeight);
	}
}
