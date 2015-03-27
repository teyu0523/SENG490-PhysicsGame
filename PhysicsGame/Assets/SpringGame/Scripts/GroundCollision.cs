using UnityEngine;
using System.Collections;

public class GroundCollision : MonoBehaviour {
	
	void Start(){

	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown (KeyCode.R)){
			Application.LoadLevel (Application.loadedLevel);
		}
	}

	void OnCollisionEnter2D(Collision2D col){
		if(col.relativeVelocity.magnitude > 5){
			Destroy (col.gameObject);
		}
	}
}
