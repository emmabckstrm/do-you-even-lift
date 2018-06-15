using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBarrel : RespawnObject {
	/*
	Respawns barrels that fall outside of the tower
	*/

	protected RespawnPosition originalPos;
	protected VRTK.InteractableObjectCustom interactableObjectScript;
	protected GameObject gameObj;
	private bool triggered = false;
	private bool respawned = false;

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
		if (other.isTrigger && other.tag == "Weight") {
			interactableObjectScript = other.GetComponent<VRTK.InteractableObjectCustom>();
			originalPos = other.GetComponent<RespawnPosition>();

			if (originalPos != null && !interactableObjectScript.IsGrabbed() && !interactableObjectScript.IsTouched()) {
					if (respawned) return;
					respawned = true;
					Respawn(other.gameObject, originalPos.GetRespawnPosition(), other.gameObject.transform.parent.transform);
					Destroy(other.gameObject);

					triggered = false;
					respawned = false;
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

	public void RespawnBarrel() {

	}
}
