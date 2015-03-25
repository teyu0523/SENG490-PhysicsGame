
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using SimpleJSON;
using Image = UnityEngine.UI.Image;

public class DropMenu : MonoBehaviour {
	public GameObject displayPanel;
	public GameObject displayPanelLessons;
	public GameObject buttonPrefab;
	public GameObject lessonPrefab;
	public GameObject scrollView;
	public GameObject scrollViewLessons;
	public Button backButton;
	public GameObject m_assignment_controller_prefab = null;

	private string lessonsResult;
	private int courseIndex;
	private GameObject[] descriptionObjects;
	private string[] lessons;
	private int[] lessonId;
	private string[] descriptions;

	/* struct of each course */
	public struct Course
	{
		public string[] lessons;
		public int[] lessonId;
		public bool[] clicked;
		public string[] descriptions;
		public int id;
		public string course;

		public Course(string course, int id, int lessonsSize)
			: this()
		{
			this.course = course;
			this.id = id;
			this.lessons = new string[lessonsSize];
			this.lessonId = new int[lessonsSize];
			this.clicked = new bool[lessonsSize];
			this.descriptions = new string[lessonsSize];
		}

	}

	/* display menu */
	public void setPreviousPage(){
		scrollViewLessons.SetActive(false);
		scrollView.SetActive(true);
		backButton.interactable = false;
	}

	/* display the additional description and start button in lesson menu*/
	public void clickedButtonDescription(int index){
		if(descriptionObjects[index].activeSelf){
			descriptionObjects[index].SetActive(false);
		}else{
			descriptionObjects[index].SetActive(true);
		}
		
	}

	/* redirect to game */
	public void clickedButtonLessons(int index){
		if(m_assignment_controller_prefab!=null){
			LessonController controller = ((GameObject)GameObject.Instantiate(m_assignment_controller_prefab)).GetComponent<LessonController>();
			controller.startLesson(courses[courseIndex].id, courses[courseIndex].lessonId[index]);
		} else {
			Debug.Log("Shouldn't happen!");
		}
	}

	public void logout(){
		NetworkingController.Instance.Logout(null);
	}

	public List<Course> courses = new List<Course>();
	public void OnLessonsReturn(string lesson_result, string lesson_error)
	{
		LoadingController.Instance.hide();
		int j, i;
		Debug.Log(lesson_result);
		/* parse json */
		if(lesson_result != null){
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


	public void Start () 
	{
		scrollViewLessons.SetActive(false);
		LoadingController.Instance.show();
		NetworkingController.Instance.GetLessons(OnLessonsReturn);
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

			/* generate list of lessons */
			for(int i=0; i<courses[index].lessons.Length; i++)
			{		
				/* generate the buttons with title of lesson */
				GameObject newButton = Instantiate (buttonPrefab) as GameObject;
				MenuButtonProperty buttonProperty = newButton.GetComponent <MenuButtonProperty>();
				buttonProperty.buttonValue.text = courses[index].lessons[i];
				buttonProperty.index = i;
				buttonProperty.mainButton.onClick.AddListener(() => clickedButtonDescription(buttonProperty.index)); 
				newButton.transform.SetParent(displayPanelLessons.transform);
				newButton.transform.localScale = new Vector3(1f, 1f, 1f);

				/* generate the descriptions and start button while by default hiding them */
				newButton = Instantiate (lessonPrefab) as GameObject;
				buttonProperty = newButton.GetComponent <MenuButtonProperty>();
				if ( courses[index].descriptions[i] != null){
					buttonProperty.descriptions.text = courses[index].descriptions[i];
				} else {
					buttonProperty.descriptions.text = "";
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



	// Update is called once per frame
	void Update() {
		
	}

	

}
