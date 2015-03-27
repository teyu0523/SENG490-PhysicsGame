using UnityEngine;
using System.Collections;

public class ExplosionScript : MonoBehaviour {

	public float m_delay;
	public AudioSource source;
	
	void Start () {
		transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
		StartCoroutine(dieDelayed(m_delay));

		source = GetComponent<AudioSource>();
		source.Play();
	}
	
	public IEnumerator dieDelayed(float delay) {
		yield return new WaitForSeconds(delay);
		Destroy(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		float scale = Time.deltaTime;
		transform.localScale += new Vector3 (scale, scale, scale);
	}
}
