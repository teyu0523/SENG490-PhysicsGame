using UnityEngine;
using System.Collections;

public class DestoryOnContact : MonoBehaviour {

	public GameObject projectile;
	private SpriteRenderer spriteRenderer;

	void Start (){
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	void Kill(){
		spriteRenderer.enabled = false;
		collider2D.enabled = false;
		rigidbody2D.isKinematic = true;
		particleSystem.Play ();
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.rigidbody2D == GameObject.FindWithTag("Bullet").rigidbody2D) {
			Destroy(other.gameObject);
			Kill ();
			StartCoroutine(Delay());
			//Destroy (gameObject);
		}
	}

	IEnumerator Delay(){
		yield return new WaitForSeconds(2f);
	}

}
