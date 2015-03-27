using UnityEngine;
using System.Collections;

public class DestoryOnContact : MonoBehaviour {

	public GameObject projectile;
	private SpriteRenderer spriteRenderer;

	private bool targetDead = false;

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

		GameObject controller = GameObject.FindWithTag("GameController");

		if (other.rigidbody2D == GameObject.FindWithTag("Bullet").rigidbody2D) {
			Destroy(other.gameObject);
			Kill ();
			targetDead = true;
			StartCoroutine(Delay());
			controller.GetComponent<GameTankController>().CheckGameStatus();
		}
	}

	IEnumerator Delay(){
		yield return new WaitForSeconds(2f);
	}

	public bool IsTargetDead(){
		return targetDead;
	}

}
