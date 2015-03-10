using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StatsDisplayPanelController: MonoBehaviour {

	public RectTransform m_panel;
	public CanvasGroup m_fade_group;
	public GameObject m_text_box_prefab;

	private Dictionary<string, Text> m_text_objects = new Dictionary<string, Text>();

	private GameObject m_parent = null;
	private Vector2 m_offset = Vector2.zero;

	public float m_fade_time = 0.4f;
	private float m_time = 0;
	private float m_start_alpha = 0.0f;
	private float m_target_alpha = 1.0f;

	// -------- Dictionary Like Interface -------- //

	/// <summary>
	/// Adds a line of editable text to the panel.
	/// </summary>
	/// <returns>The Text GUI object that was created.</returns>
	/// <param name="name">A dictionary name for the new text object.</param>
	/// <param name="value">A default value for the new text object.</param>
	public Text AddTextItem(string name, string value) {
		GameObject spawned = Instantiate(m_text_box_prefab) as GameObject;
		Text spawned_text = spawned.GetComponent<Text>();
		spawned.transform.SetParent(m_panel.transform, false);
		spawned_text.text = value;
		m_text_objects.Add(name, spawned_text);
		return spawned_text;
	}

	/// <summary>
	/// Attaches the panel to a parent object. (Doesn't use transforms!!!)
	/// </summary>
	/// <param name="parent">The object that the panel should track.</param>
	public void Attach(GameObject parent) {
		Attach(parent, Vector2.zero);
	}

	/// <summary>
	/// Attaches the panel to a parent object. (Doesn't use transforms!!!)
	/// </summary>
	/// <param name="parent">The object that the panel should track.</param>
	/// <param name="offset">An x/y offset from the center of the parent object.</param>
	public void Attach(GameObject parent, Vector2 offset) {
		m_parent = parent;
		m_offset = offset;
	}

	/// <summary>
	/// Gets the child text object with the specified key.
	/// </summary>
	/// <param name="key">The key provided in AddTextItem().</param>
	public Text this[string key]
	{
		get
		{
			return m_text_objects[key];
		}
	}

	// -------- Actual Operations -------- //

	void Start () {
		m_fade_group.alpha = m_start_alpha;
		GameController.Instance.addStatsDisplay(this);
		gameObject.SetActive(false);
	}

	protected IEnumerator DelayedDisable(float delay) {
		yield return new WaitForSeconds(delay);
		gameObject.SetActive(false);
	}

	/// <summary>
	/// Called to cause the panel to show or hide.
	/// </summary>
	/// <param name="display">True if the panel should be visible.</param>
	public void display(bool display)
	{
		if(display && !gameObject.activeSelf) {
			gameObject.SetActive(display);
			m_time = 0;
			m_target_alpha = 1.0f;
			m_start_alpha = 0.0f;
		}
		else if(gameObject.activeSelf) {
			m_time = 0;
			m_target_alpha = 0.0f;
			m_start_alpha = 1.0f;
			StartCoroutine(DelayedDisable(m_fade_time));
		}
	}

	void Update() {
		if(m_time <= m_fade_time) {
			m_time += Time.deltaTime;
			m_fade_group.alpha = Mathf.Lerp(m_start_alpha, m_target_alpha, m_time/m_fade_time);
		} else {
			m_fade_group.alpha = m_target_alpha;
		}
	}

	void LateUpdate() {
		if(m_parent != null) {
			transform.position = new Vector3(m_parent.transform.position.x + m_offset.x, m_parent.transform.position.y + m_offset.y, transform.position.z);
		}
	}
}
