using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SimpleJSON;

public class SideMenu : MonoBehaviour {
	
	public GameObject questionsPrefab;
	public GameObject questionPanel;
	public GameObject submitButton;
	private bool buttonPress = false;
	private bool showSide = false;
	private Vector2 scrollPosition = Vector2.zero;

	// Use this for initialization
	void Start () {
		GameObject questionSet;
		for(int i = 0; i < 10; i ++){
			questionSet = Instantiate (questionsPrefab) as GameObject;
			questionSetProperty qsp = questionSet.GetComponent <questionSetProperty>();
			qsp.question.text = "this is a question";
			qsp.answer.textComponent.text = "answer?";
			questionSet.transform.SetParent(questionPanel.transform);
			questionSet.transform.localScale = new Vector3(1f, 1f, 1f);
		}

		GameObject button = Instantiate (submitButton) as GameObject;
		MenuButtonProperty mbp = button.GetComponent <MenuButtonProperty>();
		mbp.buttonValue.text = "Submit";
		button.transform.SetParent(questionPanel.transform);
		button.transform.localScale = new Vector3(1f, 1f, 1f);
		
	}

	public void init(){
		
	}

	public void pause(JSONNode questions){

	}

	// Update is called once per frame
	void Update () {
	
	}
}
