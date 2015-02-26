using UnityEngine;
using System.Collections;

public class ProjectileFollow : MonoBehaviour {
	
	public Transform tank;
	public Transform farLeft;
	public Transform farRight;


	void Update () {
		Vector3 newPosition = transform.position;
		if (GameObject.Find("Asteroid") == null) {
			newPosition.x = tank.position.x;
		}
		else{
			newPosition.x = GameObject.Find("Asteroid").transform.position.x;
		}
		newPosition.x = Mathf.Clamp (newPosition.x, farLeft.position.x, farRight.position.x);
		transform.position = newPosition;
	}
}
