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
			SetNewPrefab(other.transform.parent.gameObject);
			Respawn();
			Destroy(other.transform.parent.gameObject);
		} else if (other.transform.parent.gameObject.tag == "Respawn") {
			Destroy(other.transform.parent.gameObject);
		}

	}

	protected void SetNewPrefab(GameObject gameObj) {
		prefab = gameObj;
	}

	public void Respawn() {
		Debug.Log("hello");
		base.Respawn();
	}
}
