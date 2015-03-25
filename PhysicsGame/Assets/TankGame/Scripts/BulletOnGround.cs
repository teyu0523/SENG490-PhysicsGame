using UnityEngine;
using System.Collections;

public class BulletOnGround : MonoBehaviour {

	public GameObject projectile;

	void OnTriggerEnter2D(Collider2D other){
		if (other.rigidbody2D == GameObject.FindWithTag("Bullet").rigidbody2D) {
			Destroy(other.gameObject);
		}
	}
}
