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
	private float yButton, yBoxArea, yButtonDefault, panelTop;
	private float totalHeight; // total height of all UI
	private float barValue;	// scroll bar current value
	private Vector2 scrollPosition = Vector2.zero;
	private Vector2[] scrollPositions;
	public NetworkingController networkingController;
	private RectTransform panelRectTransform;

	// Use this for initialization
	public void Start () 
	{
		panelRectTransform = GetComponent<RectTransform> ();
		panelTop = Screen.height - panelRectTransform.rect.height; // the rect Top value
		networkingController.GetLessons((lesson_result, lesson_error) => {
			if(lesson_result != null) {
				// Success will be returned as data for now, you do not need to know the token.
				// The authentication information will be stored in this controller.
				print(lesson_result);
			}else{
				print (lesson_error);
			}
		});


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
			new Rect(0, panelTop+5, Screen.width, Screen.height-panelTop-10), 
			scrollPosition, 
			new Rect(00, panelTop+5, Screen.width-20, yButton-panelTop-10)
			);

		heightTextArea = Screen.height/5;
		yButton = panelTop+5; 
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
					scrollPositions[i] = GUILayout.BeginScrollView(
						scrollPositions[i],
						GUILayout.Width(widthTextArea), 
						GUILayout.Height(heightTextArea));
					GUILayout.Label(
						descriptions[i]);
					GUILayout.Button("Start");
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
		print ("im here");
		int index = transform.GetSiblingIndex();
		transform.SetSiblingIndex (index + delta);
	}
}
