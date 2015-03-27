using UnityEngine;
using System.Collections;

public class SpringScaling : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		GameObject theEgg = GameObject.Find("Egg");
		EggController egg = theEgg.GetComponent<EggController>();

		if(egg.springFlag)
		{
			egg.springFlag = false;
			transform.localScale = new Vector3 (1.2f, 1.5f, 1.0f);
			transform.localPosition = new Vector3(0.04f, -9.23f, -1.0f);
		} else {	
			transform.localScale = new Vector3 (1.2f, 1.5f-egg.compressPct, 1.0f);
			transform.localPosition = new Vector3(0.04f, -9.23f + egg.compressPct, 1.0f);
		}
	}
}
