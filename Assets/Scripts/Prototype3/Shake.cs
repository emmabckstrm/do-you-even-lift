using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour {

	private Vector3 startPos;
	public bool shake = false;
	public float shakeDistance = 0.03f;

	// Use this for initialization
	void Start () {
		startPos = transform.position;
	}

	// Update is called once per frame
	void Update () {
		if (shake) {
			transform.position = startPos + (Random.insideUnitSphere * shakeDistance);
		} else {
			transform.position = startPos;
		}
	}
}
