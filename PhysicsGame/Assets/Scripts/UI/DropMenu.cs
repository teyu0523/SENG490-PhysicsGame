using UnityEngine;
using System.Collections;
using Image = UnityEngine.UI.Image;
using SimpleJSON;


public class DropMenu : MonoBehaviour {
	private int numQues = 0;
	private GUIStyle styleBox;
	private float heightTextArea ,heightSpace, heightButton;
	private float widthTextArea, widthButton;
	private string[] words;
	private ArrayList lessons = new ArrayList();
	private string[] descriptions;
	private bool[] clicked;
	private float yButton, yBoxArea, panelHeight, panelTop, panelLeft, panelWidth;
	private float totalHeight; // total height of all UI
	private Vector2 scrollPosition = Vector2.zero;
	private Vector2[] scrollPositions;
	private RectTransform panelRectTransform;
	public int[] lessonId;
	public GUISkin mySkin = null;// = new GUISkin("areaStyle");
	public Texture arrowUp = null;
	public GameObject m_assignment_controller_prefab = null;
	public string lessonsResult;
	public string val;
	//private string lesson_result = null;
	// Use this for initialization
	
	public void OnLessonsReturn(string lesson_result, string lesson_error)
	{
		if(lesson_result != null){
			JSONNode courses_node = JSON.Parse(lesson_result);
			foreach(JSONNode course_node in courses_node["courses"].AsArray)
			{
				numQues += course_node["lessons"].Count;				
			}
			words = new string[numQues];
			descriptions = new string[numQues];
			clicked = new bool[numQues];
			scrollPositions = new Vector2[numQues];
			lessonId = new int[numQues];
			int i;
			for(i=0; i<numQues; i++){
				descriptions[i] = "Here are the descriptions about the game .... for question number " + i;
			}
			for(i=0; i<numQues; i++)
			{
				scrollPositions[i] = Vector2.zero;
			}
			i = 0;
			foreach(JSONNode course_node in courses_node["courses"].AsArray)
			{
				//if(course_node["name"].Value.ToLower() == "intro to inuco")
				foreach(JSONNode lessons_node in course_node["lessons"].AsArray)
				{
					words[i] = lessons_node["name"];
					lessonId[i] = lessons_node["lesson_id"].AsInt;
					i++;
				}
			}
		}
	}

	public void Start () 
	{
		panelRectTransform = GetComponent<RectTransform> ();
		
		NetworkingController.Instance.GetLessons(OnLessonsReturn);
		/*NetworkingController.Instance.GetLessons((lesson_result, lesson_error) => {
		
			if(lesson_result != null) {
				OnLessonsReturn(lesson_result, lesson_error);
				// Success will be returned as data for now, you do not need to know the token.
				// The authentication information will be stored in this controller.
				print(lesson_result);
				
//						foreach(JSONNode lesson_node in course_node["lessons"].AsArray)
//						{
//							if(lesson_node["name"].Value.ToLower() == "entering numbers")
//							{
//								//when click start
//								Debug.Log("Found lesson: " + lesson_node["lesson_id"]);
//								LessonController controller = ((GameObject)GameObject.Instantiate(m_assignment_controller_prefab)).GetComponent<LessonController>();
//								controller.startLesson(lesson_node["lesson_id"].AsInt);
//							}
//						}
				
			}else{
				print (lesson_error);
			}
		});*/
				
        heightSpace = 3;
		heightButton = 50;

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
				words[i]))
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
					if(GUILayout.Button("Start"))
					{
						LessonController controller = ((GameObject)GameObject.Instantiate(m_assignment_controller_prefab)).GetComponent<LessonController>();
						controller.startLesson(lessonId[i]);
					}
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
