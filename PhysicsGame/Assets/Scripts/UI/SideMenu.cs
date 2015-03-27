using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SimpleJSON;

public class SideMenu : MonoBehaviour {
	
	public GameObject questionPanel;
	public GameObject questionPanelList;
	public GameObject questionsPrefab;
	public GameObject submitButton;
	public Button backButton;
	public GameController gameController;

	public JSONNode answers;
	private bool buttonOn = false;
	private bool buttonPress = false;
	private bool showSide = false;
	private Vector2 scrollPosition = Vector2.zero;

	public void Start()
	{
		backButton.onClick.AddListener(gameController.exitLesson);
	}

	public void parseJSON(JSONNode questions, JSONNode answers){
		if(questionPanel == null){
			Debug.LogWarning("Missing: questionPanel");

		}
		if(questionPanelList == null){
			Debug.LogWarning("Missing: questionPanelList");

		}
		if(questionsPrefab == null){
			Debug.LogWarning("Missing: questionsPrefab");

		}
		if (submitButton == null){
			Debug.LogWarning("Missing: submitButton");

		}
		if (gameController == null){
			Debug.LogWarning("Missing: gameController");
		}
		this.answers = answers;
		GameObject questionSet;
		if(questions != null){
			foreach(JSONNode node in questions["values"].Childs){
				questionSet = Instantiate (questionsPrefab) as GameObject;
				questionSetProperty qsp = questionSet.GetComponent <questionSetProperty>();
				if(qsp == null){
					Debug.LogWarning("qsp null.");
				}
				qsp.question.text = node["name"];
				qsp.type = node["type"].Value;
				if(node["type"].Value == "float"){
					qsp.answer.contentType = InputField.ContentType.DecimalNumber;
				} else if (node["type"].Value == "string"){
					qsp.answer.contentType = InputField.ContentType.Standard;
				} else {
					Debug.LogWarning(" Add the new type");
				}
				qsp.answer.text  = node["value"].Value;

				if (node["editable"].Value.Equals("false")) {
					Debug.Log(node["editable"]);
					qsp.answer.interactable = false;
				}
				qsp.answer.onValueChange.AddListener((string value) => submitString(qsp.question.text, value, qsp.type));
				//qsp.answer.onEndEdit.AddListener((string value) => submitString(qsp.question.text, value, qsp.type));
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
		Debug.Log("im here");
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
		Debug.Log(answers);
		gameController.OnSubmit(answers);
	}

	public void submitString(string name, string arg, string type){
		answers["values"][name]["value"] = arg;
		gameController.OnMenuChanged(answers);
		gameController.SetProperty(name, arg);
		/*if(type == "float"){
			gameController.SetProperty(name, float.parse(arg));
		} else if (type == "string") {
			gameController.SetProperty(name, arg);
		} else {
			Debug.Log("Please add new data type.");
		}*/
	}

	public void setString(string name, string arg){
		answers["values"][name]["value"] = arg;
		
	}

	// Update is called once per frame
	void Update () {
	
	}
}
