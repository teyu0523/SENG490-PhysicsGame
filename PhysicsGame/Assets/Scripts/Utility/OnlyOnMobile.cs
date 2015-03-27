using UnityEngine;
using System.Collections;

public class OnlyOnMobile : MonoBehaviour {

	// Use this for initialization
	void Awake () {
#if !(UNITY_IOS || UNITY_ANDROID || UNITY_BLACKBERRY || UNITY_EDITOR)
		gameObject.SetActive(false);
#endif
	}
}
