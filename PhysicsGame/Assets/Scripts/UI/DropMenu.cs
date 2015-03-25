
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using SimpleJSON;
using Image = UnityEngine.UI.Image;

public class DropMenu : MonoBehaviour {
	//private int numLessons = 0;
	//private int numCourses = 0;
	private GUIStyle styleBox;
	private float heightTextArea ,heightSpace, heightButton;
	private float widthTextArea, widthButton;
	private string[] lessons;
	public GameObject displayPanel;
	public GameObject displayPanelLessons;
	public GameObject buttonPrefab;
	public GameObject lessonPrefab;
	public GameObject scrollView;
	public GameObject scrollViewLessons;
	public Button backButton;
	private GameObject[] descriptionObjects;
	private float native_width = 600;
	private float native_height = 800;
  	private float scale_width;
 	private float scale_height;

	private string[] descriptions;
	private int courseClick = -1;
	private float yButton, yBoxArea, panelHeight, panelTop, panelLeft, panelWidth;
	private float totalHeight; // total height of all UI
	private Vector2 scrollPosition = Vector2.zero;
	private Vector2[] scrollPositions;
	private RectTransform panelRectTransform;
	public int[] lessonId;
	public GameObject m_assignment_controller_prefab = null;
	public string lessonsResult;
	public string val;
	public int courseIndex;
	//private string lesson_result = null;
	// Use this for initialization

	private bool draw_gui = true;

	public struct Course
	{
		public string[] lessons;
		public int[] lessonId;
		public bool[] clicked;
		public string[] descriptions;
		public int id;
		public string course;
		public Vector2[] scrollPositions;

		public Course(string course, int id, int lessonsSize)
			: this()
		{
			this.course = course;
			this.id = id;
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
		draw_gui = true;
		LoadingController.Instance.hide();
		int j, i;
		Debug.Log(lesson_result);
		if(lesson_result != null){
			//print (lesson_result);
			JSONNode courses_node = JSON.Parse(lesson_result);
			i = 0;
			j = 0;
			foreach(JSONNode course_node in courses_node["courses"].AsArray)
			{
				courses.Add(new Course(course_node["name"], course_node["course_id"].AsInt, course_node["lessons"].Count));
				foreach(JSONNode lessons_node in course_node["lessons"].AsArray)
				{	
					courses[j].clicked[i] = false;
					courses[j].lessons[i] = lessons_node["name"];
					courses[j].lessonId[i] = lessons_node["lesson_id"].AsInt;
					courses[j].descriptions[i] = lessons_node["description"];
					i++;
				}
				i = 0;
				j++;
			}
		} else {
			Debug.Log("Lesson result is null, printing lesson_error: " + lesson_error);
		}
		// create gameobject for all courses
		for (i=0; i<courses.Count; i++) {	
			GameObject newButton = Instantiate (buttonPrefab) as GameObject;
			MenuButtonProperty buttonProperty = newButton.GetComponent <MenuButtonProperty>();
			buttonProperty.buttonValue.text = courses[i].course;
			buttonProperty.index = i;
			buttonProperty.mainButton.onClick.AddListener(() => clickedButtonCourses(buttonProperty.index)); 
			newButton.transform.SetParent(displayPanel.transform);
			newButton.transform.localScale = new Vector3(1f, 1f, 1f);

		}
	}

	public void Awake()
	{
		scale_width = (Screen.width) / native_width;
     	scale_height = (Screen.height) / native_height;   
	}

	public void Start () 
	{
		scrollViewLessons.SetActive(false);
		panelRectTransform = GetComponent<RectTransform> ();
		LoadingController.Instance.show();
		draw_gui = false;
		NetworkingController.Instance.GetLessons(OnLessonsReturn);
        heightSpace = 3;
		heightButton = 50;
	}

	public void clickedButtonCourses(int index){
		
		backButton.interactable = true;
		if (index >= 0) {
			var children = new List<GameObject>();
			foreach (Transform child in displayPanelLessons.transform) children.Add(child.gameObject);
			children.ForEach(child => Destroy(child));
			courseIndex = index;
			scrollViewLessons.SetActive(true);
			scrollView.SetActive(false);
			descriptionObjects = new GameObject[courses[index].lessons.Length];
			for(int i=0; i<courses[index].lessons.Length; i++)
			{		

				GameObject newButton = Instantiate (buttonPrefab) as GameObject;
				MenuButtonProperty buttonProperty = newButton.GetComponent <MenuButtonProperty>();
				buttonProperty.buttonValue.text = courses[index].lessons[i];
				buttonProperty.index = i;
				buttonProperty.mainButton.onClick.AddListener(() => clickedButtonDescription(buttonProperty.index)); 
				newButton.transform.SetParent(displayPanelLessons.transform);
				newButton.transform.localScale = new Vector3(1f, 1f, 1f);



				/*newButton = Instantiate (buttonPrefab ) as GameObject;
				buttonProperty = newButton.GetComponent <MenuButtonProperty>();
				buttonProperty.buttonValue.text = "Start";
				buttonProperty.index = i;
				buttonProperty.mainButton.onClick.AddListener(() => clickedButtonLessons(buttonProperty.index));
				newButton.transform.SetParent(displayPanelLessons.transform);
				newButton.SetActive(false);
				descriptionObjects[i] = newButton;*/

				newButton = Instantiate (lessonPrefab) as GameObject;
				buttonProperty = newButton.GetComponent <MenuButtonProperty>();
				if ( courses[index].descriptions[i] != null){
					buttonProperty.descriptions.text = courses[index].descriptions[i];
				}
				buttonProperty.index = i;
				buttonProperty.mainButton.onClick.AddListener(() => clickedButtonLessons(buttonProperty.index));
				newButton.transform.SetParent(displayPanelLessons.transform);
				newButton.transform.localScale = new Vector3(1f, 1f, 1f);
				newButton.SetActive(false);
				descriptionObjects[i] = newButton;
			}

		}

	}

	public void setPreviousPage(){
		scrollViewLessons.SetActive(false);
		scrollView.SetActive(true);
		backButton.interactable = false;
	}

	public void clickedButtonDescription(int index){
		if(descriptionObjects[index].activeSelf){
			descriptionObjects[index].SetActive(false);
		}else{
			descriptionObjects[index].SetActive(true);
		}
		
	}

	public void clickedButtonLessons(int index){
		LessonController controller = ((GameObject)GameObject.Instantiate(m_assignment_controller_prefab)).GetComponent<LessonController>();
		controller.startLesson(courses[courseIndex].id, courses[courseIndex].lessonId[index]);
		draw_gui = false;
	}

	public void clickedButtonLogout() {
		NetworkingController.Instance.Logout(null);
	}

	// Update is called once per frame
	void Update() {
		
	}

	/*public void OnGUI() 
	{ 	
		GUI.matrix = Matrix4x4.TRS (new Vector3 (0, 0, 0), Quaternion.identity, new Vector3 (scale_width, scale_height, 1));
		if(!draw_gui) {
			return;
		}

		if(mySkin != null) {
			GUI.skin = mySkin;
		}
		
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
						yButton += heightButton;
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
								controller.startLesson(courses[courseClick].id, courses[courseClick].lessonId[i]);
								draw_gui = false;
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
	}*/

}
