﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class LessonReviewController : MonoBehaviour {

	public GameObject m_answer_prefab;
	public GameObject m_answer_list_container;
	public Text m_title_label;
	public Text m_mark_label;

	public void populateResults(JSONNode lesson_results)
	{
		m_title_label.text = lesson_results["name"].Value;
		m_mark_label.text = lesson_results["mark_percent"].Value;
		foreach (JSONNode node in lesson_results["answers"].AsArray) {
			GameObject new_answer = Instantiate (m_answer_prefab) as GameObject;
			AnswerDisplayContainer container = new_answer.GetComponent<AnswerDisplayContainer>();
			container.m_name_label.text = node["name"];
			container.m_type_label.text = node["type"];
			container.m_mark_label.text = node["mark"];
			new_answer.transform.SetParent(m_answer_list_container.transform);
		}
	}

	public void OnContinuePressed()
	{
		Application.LoadLevel("MainMenu");
	}
}
