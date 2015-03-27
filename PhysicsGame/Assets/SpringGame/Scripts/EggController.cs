using UnityEngine;
using System;

public class EggController : MonoBehaviour {

	public GameObject explosionPrefab;

	public AudioSource source;
	public float compressPct = 0.0f;
	public float springConstant = 10.0f;
	public float springLength = 15.0f;
	public float mass = 1.0f;
	public Boolean springFlag = false;
	public Boolean launched = false;

	private Vector3 m_initial_position = Vector3.zero;

	void Awake(){
		source = GetComponent<AudioSource>();
		m_initial_position = transform.position;
	}

	void Update(){
		if(Input.GetAxisRaw("Vertical") > 0){
			compressPct += Time.deltaTime*0.25f; //compress by 25% per second
			if(compressPct > 1.0)
			{
				compressPct = 1.0f;
			}
		} else if(Input.GetAxisRaw("Vertical") < 0){
			compressPct -= Time.deltaTime*0.25f; //compress by 25% per second
			if(compressPct < 0.0)
			{
				compressPct = 0.0f;
			}
		} else if(Input.GetAxisRaw("Jump") > 0 && !launched){
			Input.ResetInputAxes();
			Launch(compressPct);
			compressPct = 0.0f;
		}
	}

	//Launch the egg
	void Launch (float compression){
		if(compression > 0.01){
			source.Play ();
		}
		float distance = compression*springLength;
		float energy = springConstant * distance * distance;
		float velocity = (float)Math.Sqrt(energy / mass);
		rigidbody2D.velocity = new Vector2(0.0f, velocity);
		launched = true;
	}

	public void resetEgg() {
		gameObject.SetActive(true);
		transform.position = m_initial_position;
		rigidbody2D.velocity = Vector2.zero;
		launched = false;
	}

	public void breakEgg() {
		GameObject explosion = Instantiate(explosionPrefab) as GameObject;
		explosion.transform.position = transform.position;
		gameObject.SetActive(false);
	}
}
