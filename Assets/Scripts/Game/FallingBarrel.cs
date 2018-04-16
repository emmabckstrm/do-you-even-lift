using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBarrel : RespawnObject {

	protected RespawnPosition originalPos;
	protected VRTK.InteractableObjectCustom interactableObjectScript;
	protected GameObject gameObj;
	private bool triggered = false;

	// Use this for initialization
	void Start () {
		gameControl = GameObject.Find("AppManager").GetComponent<GameControl>();
		currentLevelNum = gameControl.GetCurrentLevel();
		SetParent();
	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider other) {
		if (triggered) return;
		triggered = true;

		gameObj = other.transform.parent.gameObject;
		if (gameObj.tag == "Weight") {
			interactableObjectScript = gameObj.GetComponent<VRTK.InteractableObjectCustom>();
			originalPos = gameObj.GetComponent<RespawnPosition>();

			if (originalPos != null && !interactableObjectScript.IsGrabbed() && !interactableObjectScript.IsTouched()) {

					Respawn(gameObj, originalPos.GetRespawnPosition());
					Destroy(gameObj);
					triggered = false;
			} else {
				triggered = false;
			}
		} else if (gameObj.tag == "Respawn") {
			Destroy(gameObj);
			triggered = false;
		}

	}
	void OnTiggerExit(Collider other) {
		gameObj = other.transform.parent.gameObject;
		if (gameObj.tag == "Weight") {
			if (!triggered) return;
			triggered = false;
		}
	}

	protected void SetNewPrefab(GameObject go) {
		prefab = go;
	}

	public void Respawn() {
		base.Respawn();
	}
}
