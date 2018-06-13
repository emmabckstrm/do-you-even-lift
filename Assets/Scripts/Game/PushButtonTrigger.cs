using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushButtonTrigger : MonoBehaviour {

	protected bool isPushed = false;
	protected GameObject level;
	public GameObject prefab;
	public ButtonHandler buttonHandlerScript;
	public GameObject button;
	public bool actionOnTrigger = false;
	private bool triggered = false;
	protected Transform buttonPress;
	protected Renderer renderer;
	protected Material mat;
	//public Color emissionColor = new Color(0.3360726f, 0.8161765f, 0.6175128f);
	//protected Color emissionColor = new Color(0.2039f, 0.815686f, 0.058823f);
	protected Color emissionColor = new Color(0.2039f, 0.796078f, 0.611764f);
	public RespawnInfinite respawnScript;
	public enum TriggerActions {
		StopInfiniteRespawn,
	}
	public TriggerActions triggerAction = TriggerActions.StopInfiniteRespawn;

	// Use this for initialization
	void Start () {
		buttonHandlerScript = transform.parent.parent.GetComponent<ButtonHandler>();
	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Button") {
			if (triggered) return;
			triggered = true;
			Debug.Log("pushed");
			buttonHandlerScript.PushButton();
			if (actionOnTrigger) {
				HandleAction(triggerAction);
			}
			mat = button.GetComponent<Renderer>().material;
			mat.SetColor("_EmissionColor", emissionColor);
		}
	}
	void OnTriggerExit(Collider other) {

		if (other.tag == "Button") {
			if (!triggered) return;
			triggered = false;
			Debug.Log("unnnnpushed");
			buttonHandlerScript.UnpushButton();
			if (actionOnTrigger) {
				UnhandleAction(triggerAction);
			}
			mat = button.GetComponent<Renderer>().material;
			mat.SetColor("_EmissionColor", Color.black);
		}
	}

	public void HandleAction(TriggerActions action) {
		if (action == TriggerActions.StopInfiniteRespawn) {
			respawnScript.StopRespawn();
		}
	}
	public void UnhandleAction(TriggerActions action) {
		if (action == TriggerActions.StopInfiniteRespawn) {
			respawnScript.ContinueRespawn();
		}
	}
}
