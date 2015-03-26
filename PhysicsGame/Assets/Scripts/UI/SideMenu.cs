using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SimpleJSON;

public class SideMenu : MonoBehaviour {
	
	public GameObject questionPanel;
	public GameObject questionPanelList;
	public GameObject questionsPrefab;
	public GameObject submitButton;

	private bool buttonOn = false;
	private bool buttonPress = false;
	private bool showSide = false;
	private Vector2 scrollPosition = Vector2.zero;



	// Use this for initialization
	void Start () {
		
		
	}

	public void init(){
		
	}

	public void parseJSON(JSONNode questions){
		Debug.Log(questions);

		GameObject questionSet;
		Debug.Log(questions["values"].Childs);
		if(questions != null){
			foreach(JSONNode node in questions["values"].Childs){
				questionSet = Instantiate (questionsPrefab) as GameObject;
				questionSetProperty qsp = questionSet.GetComponent <questionSetProperty>();
				qsp.question.text = node["name"];

				if(node["type"] == "float"){
					qsp.answer.contentType = InputField.ContentType.DecimalNumber;
					qsp.answerText.text = node["value"];
				} else if (node["type"] == "string"){
					qsp.answer.contentType = InputField.ContentType.Standard;
					qsp.answerText.text = node["value"];;
				} else {
					Debug.Log(" Add the new type");
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
		button.transform.SetParent(questionPanelList.transform);
		button.transform.localScale = new Vector3(1f, 1f, 1f);
		questionPanel.SetActive(false);
	}

	public void pause(){
		if(buttonOn){
			questionPanel.SetActive(false);
			buttonOn = false;
		} else {
			questionPanel.SetActive(true);	
			buttonOn = true;
		}	
	}

	// Update is called once per frame
	void Update () {
	
	}
}
