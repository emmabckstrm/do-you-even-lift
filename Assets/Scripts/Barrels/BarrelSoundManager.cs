using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelSoundManager : MonoBehaviour {

	private AudioSource source;
	private float volLow = 0.7f;
	private float volHigh = 1f;

	public AudioClip clip;

	// Use this for initialization
	void Awake () {
		clip = Resources.Load("Sounds/thud") as AudioClip;
		source = GetComponent<AudioSource>();
	}

	void OnCollisionEnter(Collision collision) {
		if (source != null && clip != null) {
			float collVol = collision.relativeVelocity.magnitude;
			//Debug.Log(collVol);
			float vol = Random.Range(volLow, volHigh);
			source.PlayOneShot(clip, collVol*0.8f);
		}
	}
}
