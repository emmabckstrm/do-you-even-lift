using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushButton : MonoBehaviour {

	protected int numberOfCollisions;
	protected GameControl gameControl;

	private bool triggered = false;

	// Use this for initialization
	void Start () {
		gameControl = GameObject.Find("AppManager").GetComponent<GameControl>();
	}

	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter(Collision other) {
		if (other.gameObject.tag == "Weight") {
			if (triggered) return;
			triggered = true;
			gameControl.IncreaseButtonCollisions();
		}
	}

	void OnCollisionExit(Collision other) {
		if (other.gameObject.tag == "Weight") {
			if (!triggered) return;
			triggered = false;
		}
	}

}
