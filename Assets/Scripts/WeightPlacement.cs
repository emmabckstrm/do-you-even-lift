using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightPlacement : MonoBehaviour {

	protected int numberOfCollisions;
	protected GameControl gameControl;
	protected GameObject gameObj;

	private bool triggered = false;

	// Use this for initialization
	void Start () {
		gameControl = GameObject.Find("AppManager").GetComponent<GameControl>();
	}


		void OnTriggerEnter(Collider other) {
			gameObj = other.transform.parent.gameObject;
			if (gameObj.tag == "Weight") {
				if (triggered) return;
				triggered = true;
				gameControl.IncreaseButtonCollisions();
			}
		}

		void OnTriggerExit(Collider other) {
			gameObj = other.transform.parent.gameObject;
			if (gameObj.tag == "Weight") {
				if (!triggered) return;
				triggered = false;
			}
		}
}
