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
		if(col.gameObject.GetComponent<EggController>().launched){
			col.gameObject.GetComponent<EggController>().breakEgg();
			(GameController.Instance as SpringGameController).OnFailure();
		}
	}
}
