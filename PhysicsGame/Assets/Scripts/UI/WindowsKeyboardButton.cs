using UnityEngine;
using System.Collections;

public class WindowsKeyboardButton : MonoBehaviour {

	bool m_showing_keyboard = false;
	
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
	void Awake() {
		GameObject.DontDestroyOnLoad(gameObject);
	}
#else
	void Awake () {
		Object.Destroy(gameObject);
	}
#endif

	public void OnClick() {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
		if(m_showing_keyboard) {
			VirtualKeyboard.ShowTouchKeyboard();
		} else {
			VirtualKeyboard.HideTouchKeyboard();
		}
#endif
	}
}
