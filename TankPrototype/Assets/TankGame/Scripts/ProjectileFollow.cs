using UnityEngine;
using System.Collections;

public class ProjectileFollow : MonoBehaviour {
	
	public Transform tank;
	public Transform farLeft;
	public Transform farRight;
	
	 GameObject targetObject = null;

	void Start(){
		targetObject = tank.gameObject;
	}

	public IEnumerator resetToTank() {
		yield return new WaitForSeconds(1f);
		targetObject = tank.gameObject;
	}


	void Update () {
		Vector3 newPosition = transform.position;
		if (targetObject != null) {
			newPosition.x = targetObject.transform.position.x;
		} 
		else {
			StartCoroutine(resetToTank());
		}
		if (GameObject.FindWithTag ("Bullet") != null) {
			targetObject = GameObject.FindWithTag("Bullet");
		}

		//if (GameObject.FindWithTag("Bullet") == null) {
			//newPosition.x = tank.position.x;
		//}
		//else{
			//newPosition.x = GameObject.FindWithTag("Bullet").transform.position.x;
		//}
		newPosition.x = Mathf.Clamp (newPosition.x, farLeft.position.x, farRight.position.x);
		transform.position = newPosition;
	}
}
