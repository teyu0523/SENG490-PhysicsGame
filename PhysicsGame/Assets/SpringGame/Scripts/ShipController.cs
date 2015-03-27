using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour {
	public AudioSource source;

	void Start(){
		source = GetComponent<AudioSource>();
	}

	void OnCollisionEnter2D(Collision2D col){
		if(col.relativeVelocity.magnitude > 5){
			col.gameObject.GetComponent<EggController>().breakEgg();
			(GameController.Instance as SpringGameController).OnFailure();
		} else {
			source.Play();
			col.gameObject.SetActive(false);
			(GameController.Instance as SpringGameController).OnSuccess();
		}
	}
}
