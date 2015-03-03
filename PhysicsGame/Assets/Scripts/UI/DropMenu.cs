using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Image = UnityEngine.UI.Image;
using SimpleJSON;



public class DropMenu : MonoBehaviour {
	private int numLessons = 0;
	private int numCourses = 0;
	private GUIStyle styleBox;
	private float heightTextArea ,heightSpace, heightButton;
	private float widthTextArea, widthButton;
	private string[] lessons;
	//private string[] courses;

	private string[] descriptions;
	private int courseClick = -1;
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

	public struct Course
	{
		public string[] lessons;
		public int[] lessonId;
		public bool[] clicked;
		public string[] descriptions;
		public string course;
		public Vector2[] scrollPositions;

		public Course(string course, int lessonsSize)
			: this()
		{
			this.course = course;
			this.lessons = new string[lessonsSize];
			this.lessonId = new int[lessonsSize];
			this.clicked = new bool[lessonsSize];
			this.descriptions = new string[lessonsSize];
			this.scrollPositions = new Vector2[lessonsSize];
			int i;
			for(i=0; i<lessonsSize; i++)
			{
				this.scrollPositions[i] = Vector2.zero;
			}
		}

	}
	
	public List<Course> courses = new List<Course>();
	public void OnLessonsReturn(string lesson_result, string lesson_error)
	{
		int j, i;

		if(lesson_result != null){
			print (lesson_result);
			JSONNode courses_node = JSON.Parse(lesson_result);
			i = 0;
			j = 0;
			foreach(JSONNode course_node in courses_node["courses"].AsArray)
			{
				courses.Add(new Course(course_node["name"], course_node["lessons"].Count));
				foreach(JSONNode lessons_node in course_node["lessons"].AsArray)
				{	
					courses[j].clicked[i] = false;
					courses[j].lessons[i] = lessons_node["name"];
					courses[j].lessonId[i] = lessons_node["lesson_id"].AsInt;
					courses[j].descriptions[i] = "Here are the descriptions about the game.Here are the descriptions about the game. Here are the descriptions about the game. Here are the descriptions about the game. Here are the descriptions about the game. Here are the descriptions about the game. Here are the descriptions about the game. Here are the descriptions about the game. ";
					i++;
				}
				i = 0;
				j++;
			}
		}
	}

	public void Start () 
	{
		panelRectTransform = GetComponent<RectTransform> ();
		NetworkingController.Instance.GetLessons(OnLessonsReturn);
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
		if (courseClick == -1) {
			for (int i=0; i<courses.Count; i++) {		
				if (GUI.Button (
					new Rect (
					Screen.width / 2 - (2 * (Screen.width) + Screen.width) / 8, 
					yButton,// + barValue + barSize, 
					widthButton, 
					heightButton),
					courses [i].course)) {
					if (courseClick == -1) {
						courseClick = i;
					}
				} else {
					yButton += heightButton + 3;
					if(courseClick > -1){
						break;
					}
				}
				yButton = yButton - heightSpace;
			}
		} else if(courses != null	){
			if(GUI.Button(
				new Rect(
				10, 
				yButton,// + barValue + barSize, 
				70, 
				heightButton),
				"Back"))
			{
				courseClick = -1;
			}
				
			if(courseClick!=-1 && courses.Count != 0){
				for(int i=0; i<courses[courseClick].lessons.Length; i++)
				{		
					if(GUI.Button(
						new Rect(
						Screen.width/2 - (2*(Screen.width) + Screen.width)/8, 
						yButton,// + barValue + barSize, 
						widthButton, 
						heightButton),
						courses[courseClick].lessons[i]))
					{
						if(courses[courseClick].clicked[i])
						{
							courses[courseClick].clicked[i] = false;
						}
						else
						{
							courses[courseClick].clicked[i] = true;
						}
					}
					else
					{
						yButton += heightButton+3;
						if(courses[courseClick].clicked[i] == true)
						{
							GUILayout.BeginArea(
								new Rect(
								Screen.width/2 - (2*(Screen.width) + Screen.width)/8, 
								yButton,
								widthTextArea, 
								heightTextArea-heightSpace)
								);
							
							courses[courseClick].scrollPositions[i] = GUILayout.BeginScrollView(
								courses[courseClick].scrollPositions[i],
								GUILayout.Width(widthTextArea), 
								GUILayout.Height(heightTextArea-heightSpace));
							//					GUILayout.Space (-4);
							//					GUILayout.Box(arrowUp);
							//					GUILayout.Space (-8);
							
							//GUILayout.Label("", GUILayoutOption);
							if(courses[courseClick].descriptions[i] != null && courses[courseClick].descriptions[i] != "" )
							{
								GUILayout.Label(courses[courseClick].descriptions[i]);
								GUILayout.Space (-4);
							}
							if(GUILayout.Button("Start"))
							{
								LessonController controller = ((GameObject)GameObject.Instantiate(m_assignment_controller_prefab)).GetComponent<LessonController>();
								controller.startLesson(courses[courseClick].lessonId[i]);
							}
							GUILayout.EndScrollView();
							GUILayout.EndArea();
							yButton += heightTextArea + heightSpace;
						}
					}
				}
				yButton = yButton - heightSpace;
			}
		}
		GUI.EndScrollView();
	}

//	public void MoveInHierarchy(int delta) {
//		print ("im here");
//		int index = transform.GetSiblingIndex();
//		transform.SetSiblingIndex (index + delta);
//	}
}
