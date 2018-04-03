using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushButtonTrigger : MonoBehaviour {

	protected bool isPushed = false;
	protected GameObject level;
	public GameObject prefab;
	public ButtonHandler buttonHandlerScript;
	public GameObject button;
	private bool triggered = false;
	protected Transform buttonPress;
	protected Renderer renderer;
	protected Material mat;
	public Color emissionColor = new Color(0.3360726f, 0.8161765f, 0.6175128f);

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
			mat = button.GetComponent<Renderer>().material;
			mat.SetColor("_EmissionColor", emissionColor);
		}
	}
	void OnTriggerExit(Collider other) {
		if (!triggered) return;
		triggered = false;
		if (other.tag == "Button") {
			buttonHandlerScript.UnpushButton();
			mat = button.GetComponent<Renderer>().material;
			mat.SetColor("_EmissionColor", Color.black);
		}
	}
}
