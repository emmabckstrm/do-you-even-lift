using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnObject : MonoBehaviour {
	/*
	Different methods to instantiate objects
	*/

	protected GameControl gameControl;
	protected GameObject parent;
	protected Transform respawnPos;
	protected Transform respawnPosOriginal;
	protected int currentLevelNum = 0;
	protected GameObject respawnedObject;

	public GameObject prefab;

	// Use this for initialization
	public void Start () {
		gameControl = GameObject.Find("AppManager").GetComponent<GameControl>();
		currentLevelNum = gameControl.GetCurrentLevel();
		SetParent();
		SetRespawnPos();
		//respawnPos = parent.Find("RespawnPosition").transform;
	}
	protected virtual void SetParent() {
		parent = GameObject.Find("Level " + currentLevelNum + "(Clone)");
	}
	protected virtual void SetRespawnPos() {
		respawnPos = GameObject.Find("Level " + currentLevelNum + "(Clone)/RespawnPosition").transform;
		respawnPosOriginal = respawnPos;
	}

	public virtual void Respawn() {
		respawnedObject = Instantiate(prefab, respawnPos.position, Quaternion.identity, parent.transform);
	}
	public virtual void Respawn(GameObject gameObj) {
		respawnedObject = Instantiate(gameObj, respawnPos.position, Quaternion.identity, parent.transform);

	}
	public virtual void Respawn(GameObject gameObj, Transform customPos) {
		respawnedObject = Instantiate(gameObj, customPos.position, Quaternion.identity, parent.transform);
	}
	public virtual void Respawn(GameObject gameObj, Vector3 customPos) {
		respawnedObject = Instantiate(gameObj, customPos, Quaternion.identity, parent.transform);
	}
	public virtual void Respawn(GameObject gameObj, Vector3 customPos, Transform customParent) {
		respawnedObject = Instantiate(gameObj, customPos, Quaternion.identity, customParent);

	}

}
