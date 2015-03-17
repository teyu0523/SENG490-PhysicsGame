using UnityEngine;
using System.Collections;

public class CamController : MonoBehaviour {

	private GameObject cam;
	private float speed;

	// Use this for initialization
	void Start () {
		cam = GameObject.FindWithTag("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {
		speed = Input.GetAxisRaw("Horizontal");// * Time.deltaTime;
		Vector3 pos = cam.transform.position;
		cam.transform.position = Vector3.Lerp(pos,new Vector3(pos.x+speed, pos.y, pos.z),0.1f);
	}
}
