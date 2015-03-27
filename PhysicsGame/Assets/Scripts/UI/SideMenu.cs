using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using SimpleJSON;
using System;

public class SideMenu : MonoBehaviour {
	
	public GameObject questionPanel;
	public GameObject questionPanelList;
	public GameObject questionsPrefab;
	public GameObject submitButton;
	public Button backButton;
	public GameController gameController;
	public Text numTries;
	public JSONNode answers;

	private List<InputField> list = new List<InputField>();
	private bool _pause = false;
	private int _tries = 0;
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
			_tries = int.Parse(questions["max_tries"].Value);
			numTries.text = questions["max_tries"].Value;
			foreach(JSONNode node in questions["values"].Childs){
				questionSet = Instantiate (questionsPrefab) as GameObject;
				questionSetProperty qsp = questionSet.GetComponent <questionSetProperty>();
				if(qsp == null){
					Debug.LogWarning("qsp null.");
				}
				qsp.question.text = node["name"].Value;
				qsp.type = node["type"].Value;
				if(qsp.type == "float" || qsp.type == "integer"){
					qsp.answer.contentType = InputField.ContentType.DecimalNumber;
					qsp.maxBound = float.Parse(node["max_value"]);
					qsp.minBound = float.Parse(node["min_value"]);
				} else if (qsp.type == "string" || qsp.type == "paragraph"){
					qsp.answer.contentType = InputField.ContentType.Standard;
					qsp.maxLength = int.Parse(node["max_length"]);
					qsp.answer.characterLimit = qsp.maxLength;
				} else {
					Debug.LogWarning(" Add the new type");
				}
				qsp.answer.text = node["value"].Value;
				if (node["editable"].Value.Equals("false")) {
					Debug.Log(node["editable"]);
					qsp.answer.interactable = false;
				}
				qsp.answer.name = node["name"].Value;
				list.Add(qsp.answer);
				//qsp.answer.onValueChange.AddListener((string value) => submitString(qsp.question.text, value, qsp.type));
				if(qsp.type == "float" || qsp.type == "integer"){
					qsp.answer.onValueChange.AddListener((string value) => submitString(qsp.question.text, value, qsp.maxBound, qsp.minBound, qsp.type));
				} else if (qsp.type == "string" || qsp.type == "paragraph"){
					qsp.answer.onEndEdit.AddListener((string value) => submitString(qsp.question.text, value, qsp.maxLength, qsp.type));
				}
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
	public bool Pause
    {
		get
		{
		    return this._pause;
		}
    }

	public int Tries
    {
		get
		{
		    return this._tries;
		}
		set
		{
		    this._tries = value;
		}
    }

	public void pause(){
		if(_pause){
			questionPanel.SetActive(false);
			_pause = false;
			Time.timeScale = 1.0f;     

		} else {
			questionPanel.SetActive(true);	
			_pause = true;
			Time.timeScale = 0.0f;     
		}	
	}

	public void submit(){
		Debug.Log(answers);
		pause();
		gameController.OnSubmit(answers);
	}

	public void submitString(string name, string arg, int maxLength, string type){
		answers["value"][name]["value"] = arg;
		gameController.OnMenuChanged(answers);
		gameController.SetProperty(name, arg);
	}

	public void submitString(string name, string arg, float max, float min, string type){
		if(arg.Equals("-")){
			return;
		}
		try{
			float val = float.Parse(arg);
			if(val < min){
				arg = min.ToString();
				setString(name, arg);
			} else if (val > max){
				arg = max.ToString();
				setString(name, arg);
			}
			// this freeze the program but will fix 0s infront of numbers
			/*if(!arg.Equals(val.ToString())){
				arg = val.ToString();
				setString(name, arg);
			}*/
			
			answers["values"][name]["value"] = arg;
			gameController.OnMenuChanged(answers);
			gameController.SetProperty(name, arg);
			
		} catch (FormatException){
			return;
			answers["value"][name]["value"] = "0.0";
			gameController.OnMenuChanged(answers);
			gameController.SetProperty(name, "0.0");
			setString(name, "0.0");
		} catch (OverflowException) {

		}
	}

	public void setString(string name, string arg){
		answers["values"][name]["value"] = arg;
		foreach(InputField inputF in list){
			if(inputF.name.Equals (name)){
				inputF.text = arg;
				break;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		numTries.text = _tries.ToString();
	}
}
