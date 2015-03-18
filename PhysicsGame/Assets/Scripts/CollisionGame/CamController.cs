using UnityEngine;
using System.Collections;

public class CamController : MonoBehaviour {

	//private GameObject cam;
	private float speed;
	private Vector3 startVec;
	private bool mousePressed;
	public float smoothTime = 0.001f;
	public float MinXPosRestriction;
	public float MaxXPosRestriction;
	public float MinYPosRestriction;
	public float MaxYPosRestriction;
	
	private Vector3 velocity = Vector3.zero;

	// Use this for initialization
	void Start () {
		mousePressed = false;
	}
	
	// Update is called once per frame
	void Update () {


		if(Input.GetMouseButtonDown(0)){
			mousePressed = true;
		}else if(Input.GetMouseButtonUp(0)){
			mousePressed = false;
		}
		if(mousePressed){
			transform.position = Vector3.SmoothDamp(
				transform.position,  
				transform.position - new Vector3(
					Input.GetAxis("Mouse X")*10,
					0, 
					0), 
				ref velocity, 
				smoothTime);
		}
		// Need to fix the camera border
		/*if(Input.GetAxis("Mouse ScrollWheel") > 0){
		    this.camera.fieldOfView -= 4;
		} else if(Input.GetAxis("Mouse ScrollWheel") < 0) {
			this.camera.fieldOfView += 4;
		}*/
		speed = Input.GetAxisRaw("Horizontal");
		transform.position = Vector3.SmoothDamp(
				transform.position,  
				new Vector3(
					transform.position.x+(speed*10), 
					transform.position.y, 
					transform.position.z
				), 
			ref velocity, 
			smoothTime);

		transform.position = new Vector3 (
			Mathf.Clamp(
				transform.position.x, 
				MinXPosRestriction, 
				MaxXPosRestriction
				), 
			transform.position.y, 
			transform.position.z
			);
	}
}
