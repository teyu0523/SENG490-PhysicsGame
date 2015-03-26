using UnityEngine;
using System.Collections;

public class BackgroundScroll : MonoBehaviour {

	public float speed = 0.5f;
	
	// Update is called once per frame
	void Update () {

		renderer.material.mainTextureOffset = new Vector2 (Time.time * speed, 0f);
	
	}
}
