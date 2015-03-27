using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour {

	public Sprite explosionSprite;
	public SpriteRenderer spriteRenderer;
	public AudioSource source1;
	public AudioSource source2;

	void Start(){
		explosionSprite = Resources.Load<Sprite> ("explosion");
		spriteRenderer = GetComponent<SpriteRenderer> ();

		AudioSource[] sources = GetComponents<AudioSource>();
		source1 = sources[0];
		source2 = sources[1];
	}

	void OnCollisionEnter2D(Collision2D col){
		Destroy (col.gameObject);
		if(col.relativeVelocity.magnitude > 5){
			spriteRenderer.sprite = explosionSprite;
			source1.Play();
		} else {
			source2.Play();
		}
	}
}
