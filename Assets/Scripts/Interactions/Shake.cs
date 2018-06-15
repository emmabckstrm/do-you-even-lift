using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour {

	public enum ShakeDir {
		X,
		XAndZ,
	}
	private Vector3 startPos;
	private Quaternion startRot;
	public bool shake = false;
	protected float shakeIntensity = 0.003f;
	public ShakeDir shakeDirection = ShakeDir.X;


	// Use this for initialization
	void Start () {
		startPos = transform.position;
		startRot = transform.rotation;
	}

	// Update is called once per frame
	void Update () {
		if (shake) {
			transform.position = transform.position + (Random.insideUnitSphere * shakeIntensity);
			/*if (shakeDirection == ShakeDir.XAndZ) {
				transform.rotation = transform.rotation * Quaternion.Euler(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
			} else if (shakeDirection == ShakeDir.X) {
				transform.rotation = transform.rotation * Quaternion.Euler(Random.Range(-1f, 1f), 0f, 0f);
			}*/
		} else {
			//transform.position = startPos;
			//transform.rotation = startRot;
		}
	}

	public void EnableShake() {
		shake = true;
	}
	public void DisableShake() {
		shake = false;
	}
	public bool IsShaking() {
		return shake;
	}
}
