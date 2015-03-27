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
	private float elapsed = 0.0f;
	public float zoomSpeed = 2;
	public float targetOrtho;
	public float smoothSpeed = 2.0f;
	public float minOrtho = 1.0f;
	public float maxOrtho = 20.0f;
	private Vector3 velocity = Vector3.zero;

	// Use this for initialization
	void Start () {
		mousePressed = false;
		targetOrtho = Camera.main.orthographicSize;

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
		// Camera zoom in zoom out 
		float scroll = Input.GetAxis ("Mouse ScrollWheel");
		if (scroll != 0.0f) {
			targetOrtho -= scroll * zoomSpeed;
			targetOrtho = Mathf.Clamp (targetOrtho, minOrtho, maxOrtho);
		}

		Camera.main.orthographicSize = Mathf.MoveTowards (Camera.main.orthographicSize, targetOrtho, smoothSpeed * Time.deltaTime);
	    /*if(Input.GetAxis("Mouse ScrollWheel") > 0){
		    this.camera.orthographicSize =  Mathf.Lerp(this.camera.orthographicSize, this.camera.orthographicSize-4, 20.0f);
		} else if(Input.GetAxis("Mouse ScrollWheel") < 0) {
			 this.camera.orthographicSize =  Mathf.Lerp(this.camera.orthographicSize, this.camera.orthographicSize+4, 20.0f);;
		}
		this.camera.orthographicSize = Mathf.MoveTowards (Camera.main.orthographicSize, targetOrtho, smoothSpeed * Time.deltaTime);*/
		
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
