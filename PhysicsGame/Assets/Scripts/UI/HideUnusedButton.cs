using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HideUnusedButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Button button = gameObject.GetComponent<Button>();
		if(button != null) {
			if(button.onClick.GetPersistentEventCount() == 0) {
				gameObject.SetActive(false);
			}
		}
	}
}
