using UnityEngine;
using System.Collections;
using Image = UnityEngine.UI.Image;


public class DropMenu : MonoBehaviour {
	private int numQues = 20;
	private GUIStyle styleBox;
	private float heightTextArea ,heightSpace, heightButton;
	private float widthTextArea, widthButton;
	private string[] questions;
	private string[] descriptions;
	private bool[] clicked;
	private float yButton, yBoxArea, panelHeight, panelTop, panelLeft, panelWidth;
	private float totalHeight; // total height of all UI
	private Vector2 scrollPosition = Vector2.zero;
	private Vector2[] scrollPositions;
	public NetworkingController networkingController;
	private RectTransform panelRectTransform;
	public GUISkin mySkin = null;// = new GUISkin("areaStyle");
	public Texture arrowUp = null;
	// Use this for initialization
	public void Start () 
	{
		panelRectTransform = GetComponent<RectTransform> ();
		networkingController.GetLessons((lesson_result, lesson_error) => {
			if(lesson_result != null) {
				// Success will be returned as data for now, you do not need to know the token.
				// The authentication information will be stored in this controller.
				print(lesson_result);
			}else{
				print (lesson_error);
			}
		});
        
        heightSpace = 3;
		heightButton = 50;
		questions = new string[numQues];
		descriptions = new string[numQues];
		clicked = new bool[numQues];
		scrollPositions = new Vector2[numQues];

		for(int i=0; i<numQues; i++)
		{
			scrollPositions[i] = Vector2.zero;
		}
		questions[0] = "Assignment 1";
		questions[1] = "Assignment";
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
		questions[15] = "Click4444";
		questions[16] = "Click44";
		questions[17] = "Click3";
		questions[18] = "Click4";
		questions[19] = "Click5";
		
		descriptions[0] = "danny is bored";
		descriptions[1] = "mikko spenikko spends o spends money\nmikko spends money\nmikko spends money\n";
		descriptions[2] = "mikko spends jjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjends jjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjends jjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjends jjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjends jjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjends jjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjends jjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjends jjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjends jjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjends jjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjends jjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjends jjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjends jjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjends jjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjends jjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjends jjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjmoney\nmikko spends money\nmikko spends money\nmikko spends money\nmikko spends money\n";
		for(int i=3; i<numQues; i++){
			descriptions[i] = "";
		}
	}
	
	// Update is called once per frame
	public void Update() {

	}

	public void OnGUI() 
	{ 	
		if(mySkin != null);
		GUI.skin = mySkin;
		
		panelTop = panelRectTransform.offsetMax.y*-1;
		panelHeight = panelRectTransform.rect.height; // the rect Top value
		panelLeft = panelRectTransform.offsetMin.x;
		panelWidth = panelRectTransform.rect.width;
		scrollPosition = GUI.BeginScrollView (
			new Rect(panelLeft, panelTop, panelWidth, panelHeight), 
			scrollPosition, 
			new Rect(panelLeft, panelTop, panelWidth-40, yButton)
			);

		heightTextArea = Screen.height/4;
		yButton = panelTop+20; 
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
					GUILayout.BeginArea(
						new Rect(
						Screen.width/2 - (2*(Screen.width) + Screen.width)/8, 
						yButton,
						widthTextArea, 
						heightTextArea-heightSpace)
						);
					
					scrollPositions[i] = GUILayout.BeginScrollView(
						scrollPositions[i],
						GUILayout.Width(widthTextArea), 
						GUILayout.Height(heightTextArea-heightSpace));
//					GUILayout.Space (-4);
//					GUILayout.Box(arrowUp);
//					GUILayout.Space (-8);
					
					//GUILayout.Label("", GUILayoutOption);
					if(descriptions[i] != null && descriptions[i] != "" )
					{
						GUILayout.Label(descriptions[i]);
						GUILayout.Space (-4);
					}
					GUILayout.Button("Start");
					GUILayout.EndScrollView();
					GUILayout.EndArea();
					yButton += heightTextArea + heightSpace;
				}
			}
			yButton = yButton - heightSpace;
		}
		GUI.EndScrollView();
	}

//	public void MoveInHierarchy(int delta) {
//		print ("im here");
//		int index = transform.GetSiblingIndex();
//		transform.SetSiblingIndex (index + delta);
//	}
}
