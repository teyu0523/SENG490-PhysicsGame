using UnityEngine;
using System.Collections;

public class Resetter : MonoBehaviour {


	void Start () {
	}

	void Update () {
		if(Input.GetKeyDown (KeyCode.R)){
			Reset();
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.rigidbody2D == GameObject.FindWithTag("Bullet").rigidbody2D) {
			Destroy(GameObject.FindWithTag("Bullet"));
		}
	}

	void Reset(){
		Application.LoadLevel (Application.loadedLevel);
	}
}
