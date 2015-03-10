using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StatsDisplayPanelController: MonoBehaviour {

	public RectTransform m_panel;
	public GameObject m_text_box_prefab;

	private Dictionary<string, Text> m_text_objects;

	void Awake() {
		m_text_objects = new Dictionary<string, Text>();
	}

	public Text AddTextItem(string name, string value) {
		GameObject spawned = Instantiate(m_text_box_prefab) as GameObject;
		Text spawned_text = spawned.GetComponent<Text>();
		spawned.transform.SetParent(m_panel, false);
		spawned_text.text = value;
		m_text_objects.Add(name, spawned_text);
		return spawned_text;
	}

	public Text this[string key]
	{
		get
		{
			return m_text_objects[key];
		}
	}

	// Use this for initialization
	void Start () {
		AddTextItem("1", "Carrots");
		AddTextItem("2", "Peas");
		AddTextItem("3", "Potatoes");
		AddTextItem("4", "Watermelon!");
	}
}
