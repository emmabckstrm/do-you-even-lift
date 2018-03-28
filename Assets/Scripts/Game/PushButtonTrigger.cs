using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushButtonTrigger : MonoBehaviour {

	protected bool isPushed = false;
	protected GameObject level;
	public GameObject prefab;
	public ButtonHandler buttonHandlerScript;
	private bool triggered = false;

	// Use this for initialization
	void Start () {
		//buttonHandlerScript = transform.parent.GetComponent<ButtonHandler>();
	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider other) {
		if (triggered) return;
		triggered = true;

		if (other.tag == "Button") {
			buttonHandlerScript.PushButton();
		}
	}
	void OnTriggerExit(Collider other) {
		if (!triggered) return;
		triggered = false;
		if (other.tag == "Button") {
			buttonHandlerScript.UnpushButton();
		}
	}
}
