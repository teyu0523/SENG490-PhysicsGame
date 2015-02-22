using UnityEngine;
using System.Collections;

/// <summary>
/// Scene must contain a camera tagged as MainCamera!
/// </summary>
public class RemoveOutsideOfScreen : MonoBehaviour {	
	// Update is called once per frame
	void Update () {
		if(Camera.main != null)
		{
			Vector2 pos = Camera.main.WorldToViewportPoint(transform.position);
			if(pos.x + renderer.bounds.extents.x < 0f)
				GameObject.Destroy(gameObject);
			if(pos.x - renderer.bounds.extents.x > 1f)
				GameObject.Destroy(gameObject);
			if(pos.y + renderer.bounds.extents.y < 0f)
				GameObject.Destroy(gameObject);
			if(pos.y - renderer.bounds.extents.y > 1f)
				GameObject.Destroy(gameObject);
		}
		else
		{
			throw new System.Exception("Scene does not contain a camera tagged MainCamera!");
		}
	}
}
