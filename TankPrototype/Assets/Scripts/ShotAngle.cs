using UnityEngine;
using System.Collections;

public class ShotAngle : MonoBehaviour {

	Transform barrel;
	Transform shotSpawn;


	void Awake(){
		barrel = GetComponentInParent<Transform> ();
		shotSpawn = GetComponent<Transform> ();
	}


	void Update () {
		shotSpawn.rotation = barrel.rotation;

		
	}

}
