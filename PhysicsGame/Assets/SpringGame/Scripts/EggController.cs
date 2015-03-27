using UnityEngine;
using System;

public class EggController : MonoBehaviour {

	public AudioSource source;
	public float compressPct = 0.0f;
	public float springConstant = 10.0f;
	public float springLength = 15.0f;
	public float mass = 1.0f;
	public Boolean springFlag = false;

	Rigidbody2D eggRigidbody;

	void Awake(){
		eggRigidbody = GetComponent<Rigidbody2D> ();
		source = GetComponent<AudioSource>();
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
		} else if(Input.GetAxisRaw("Jump") > 0){
			Input.ResetInputAxes();
			Launch(compressPct);
			compressPct = 0.0f;
			springFlag = true;
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
		eggRigidbody.velocity = new Vector2(0.0f, velocity);
	}
}
