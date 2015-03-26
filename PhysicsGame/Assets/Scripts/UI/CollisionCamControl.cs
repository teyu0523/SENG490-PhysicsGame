using UnityEngine;
using System.Collections;

public class CollisionCamControl : MonoBehaviour {
	public float moveLeft;
	private bool buttonPress = false;
	// Use this for initialization
	void Start () {
		moveLeft = -1;
	}
	
	// Update is called once per frame
	void Update () {
	
		if(moveLeft==-1){
			moveLeft = Screen.width;
		}
		if(buttonPress == true)
		{
			if(moveLeft > Screen.width/2){
				moveLeft -= 10;
			}else{
				moveLeft = Screen.width/2;
			}
		} else {
			if(moveLeft < Screen.width){
				moveLeft += 10;
			}else{
				moveLeft = Screen.width;
			}
		}
	}
}
