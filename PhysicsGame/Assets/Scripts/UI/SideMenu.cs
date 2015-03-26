using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SimpleJSON;

public class SideMenu : MonoBehaviour {
	
	public GameObject questionPanel;
	public GameObject questionPanelList;
	public GameObject questionsPrefab;
	public GameObject submitButton;
	public JSONNode answers;
	private bool buttonOn = false;
	private bool buttonPress = false;
	private bool showSide = false;
	private Vector2 scrollPosition = Vector2.zero;

	public void parseJSON(JSONNode questions, JSONNode answers){
		Debug.Log(questions);
		Debug.Log(answers);
		this.answers = answers;
		GameObject questionSet;
		Debug.Log(questions["values"].Childs);
		if(questions != null){
			foreach(JSONNode node in questions["values"].Childs){
				questionSet = Instantiate (questionsPrefab) as GameObject;
				questionSetProperty qsp = questionSet.GetComponent <questionSetProperty>();
				qsp.question.text = node["name"];

				if(node["type"] == "float"){
					Debug.Log("im here22222222222222");
					qsp.answer.contentType = InputField.ContentType.DecimalNumber;
				} else if (node["type"] == "string"){
					qsp.answer.contentType = InputField.ContentType.Standard;
				} else {
					Debug.Log(" Add the new type");
				}
				qsp.answer.onEndEdit.AddListener((string value) => submitString(node["name"], value));
				qsp.answerText.text = node["value"];
				questionSet.transform.SetParent(questionPanelList.transform);
				questionSet.transform.localScale = new Vector3(1f, 1f, 1f);
			}
		} else {
			Debug.LogWarning("JSON not parsed");
		}
		GameObject button = Instantiate (submitButton) as GameObject;
		MenuButtonProperty mbp = button.GetComponent <MenuButtonProperty>();
		mbp.buttonValue.text = "Submit";
		mbp.mainButton.onClick.AddListener(submit);
		button.transform.SetParent(questionPanelList.transform);
		button.transform.localScale = new Vector3(1f, 1f, 1f);
		questionPanel.SetActive(false);
	}


	public void pause(){
		if(buttonOn){
			questionPanel.SetActive(false);
			buttonOn = false;
			Time.timeScale = 1.0f;     

		} else {
			questionPanel.SetActive(true);	
			buttonOn = true;
			Time.timeScale = 0.0f;     
		}	
	}

	public void submit(){
		Debug.Log("im here");
		/*var children = new List<GameObject>();
		foreach (Transform child in questionPanelList.transform) children.Add(child.gameObject);
		*/
	}

	public void submitString(string name, string arg){
		Debug.Log(name + " " + arg);
		answers["value"]["name"]["value"] = arg;
	}
	// Update is called once per frame
	void Update () {
	
	}
}
