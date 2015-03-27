using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour {
	public SpriteRenderer spriteRenderer;
	public AudioSource source1;
	public AudioSource source2;

	void Start(){
		spriteRenderer = GetComponent<SpriteRenderer> ();

		AudioSource[] sources = GetComponents<AudioSource>();
		source1 = sources[0];
		//source2 = sources[1];
	}

	void OnCollisionEnter2D(Collision2D col){
		if(col.relativeVelocity.magnitude > 5){
			source1.Play();
			col.gameObject.GetComponent<EggController>().breakEgg();
			(GameController.Instance as SpringGameController).OnFailure();
		} else {
			//source2.Play();
			col.gameObject.SetActive(false);
			(GameController.Instance as SpringGameController).OnSuccess();
		}
	}
}
