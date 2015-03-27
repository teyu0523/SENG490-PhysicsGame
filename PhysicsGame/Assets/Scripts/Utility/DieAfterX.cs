using UnityEngine;
using System.Collections;

public class DieAfterX : MonoBehaviour {
	public float m_delay;

	void Start () {
		StartCoroutine(dieDelayed(m_delay));
	}

	public IEnumerator dieDelayed(float delay) {
		yield return new WaitForSeconds(delay);
		Destroy(gameObject);
	}
}
