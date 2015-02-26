using UnityEngine;
using System.Collections;
using SimpleJSON;

public class TestGameLauncher : MonoBehaviour {

	public GameObject m_assignment_controller_prefab = null;

	// Use this for initialization
	void Start () {
		NetworkingController.Instance.Login("geoff", "pass", OnLogin);
	}

	public void OnLogin(string success, string error)
	{
		if(success != null)
		{
			Debug.Log(success);
			NetworkingController.Instance.GetLessons(OnLessonsReturn);
		}
	}

	public void OnLessonsReturn(string success, string error)
	{
		if(success != null)
		{
			Debug.Log(success);

			JSONNode courses_node = JSON.Parse(success);
			foreach(JSONNode course_node in courses_node["courses"].AsArray)
			{
				if(course_node["name"].Value.ToLower() == "intro to inuco")
				{
					foreach(JSONNode lesson_node in course_node["lessons"].AsArray)
					{
						if(lesson_node["name"].Value.ToLower() == "entering numbers")
						{
							Debug.Log("Found lesson: " + lesson_node["lesson_id"]);
							LessonController controller = ((GameObject)GameObject.Instantiate(m_assignment_controller_prefab)).GetComponent<LessonController>();
							controller.startLesson(lesson_node["lesson_id"].AsInt);
							return;
						}
					}
				}
			}
		}
	}
}
