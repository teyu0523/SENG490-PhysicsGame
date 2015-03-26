using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingController : MonoBehaviour {

	// Singleton creation, allowing access from anywheres!
	private static LoadingController m_instance = null;

	public GameObject m_panel;
	public GameObject m_icon;

	private int m_show_count;

	private float m_time = 0.0f;
	
	public static LoadingController Instance {
		get{return m_instance;}
	}

	public void Awake() {
		// If this is the first loading controller it becomes the singleton
		// The first loading controller will not be destroyed on a scene change.
		if(m_instance == null) {
			m_instance = this;
			DontDestroyOnLoad(gameObject);
		} else {
			// If this is not the first instance. Destroy it.
			Object.Destroy(gameObject);
		}
		m_panel.SetActive(false);
		m_show_count = 0;
	}

	public void Update() {
		m_time += Time.deltaTime;
		if(m_time > 0.6f) {
			m_time -= 0.6f;
		}
		if(m_time <= 0.3f) {
			m_icon.transform.localScale = new Vector3(0.5f + m_time*2, 0.5f + m_time*2, 1.0f);
		} else {
			m_icon.transform.localScale = new Vector3(0.5f + ((0.6f-m_time)*2), 0.5f + ((0.6f-m_time)*2), 1.0f);
		}
	}

	public void show() {
		if(++m_show_count > 0) {
			m_panel.SetActive(true);
		}
	}

	public void hide() {
		if(--m_show_count <= 0) {
			m_panel.SetActive(false);
			m_show_count = 0;
		}
	}
}
