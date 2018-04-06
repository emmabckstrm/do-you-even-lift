using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBarrel : RespawnObject {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider other) {

		if (other.transform.parent.gameObject.tag == "Weight") {
			//if (triggered) return;
			//triggered = true;
			Debug.Log("HEY");
			Respawn();
		}
		Destroy(other.transform.parent.gameObject);
	}

	public void Respawn() {
		Debug.Log("hello");
		base.Respawn();
	}
}
