using UnityEngine;
using System.Collections;

public class Resetter : MonoBehaviour {

	public GameObject projectile;

//	void Update () {
//		if(Input.GetKeyDown (KeyCode.R)){
//			Reset();
//		}
//	}

	void OnTriggerExit2D(Collider2D other){
		if (other.rigidbody2D == GameObject.FindWithTag("Bullet").rigidbody2D) {
			Destroy(other.gameObject);
		}
	}

//	void Reset(){
//		Application.LoadLevel (Application.loadedLevel);
//	}
}
