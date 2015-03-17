using UnityEngine;
using System.Collections;

public class CamController : MonoBehaviour {

	private GameObject cam;
	private float speed;

	// Use this for initialization
	void Start () {
		cam = GameObject.FindWithTag("MainCamera");
		
		//this.camera
	}
	
	// Update is called once per frame
	void Update () {
		speed = Input.GetAxisRaw("Horizontal");// * Time.deltaTime;
		Vector3 pos = cam.transform.position;
		cam.transform.position = Vector3.Lerp(pos,new Vector3(pos.x+speed, pos.y, pos.z),0.1f);

		//if (Input.GetKeyDown(KeyCode.LeftArrow)){
		//	Vector3 pos = cam.transform.position;
		//	cam.transform.position = Vector3.Lerp(pos,new Vector3(pos.x-1, pos.y, pos.z),0.1f);
		//}
		//if (Input.GetKeyDown(KeyCode.RightArrow)){
		//	cam.transform.Translate(1, 0, 0);
		//}
	}
}
