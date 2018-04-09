using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnObject : MonoBehaviour {

	protected GameControl gameControl;
	protected GameObject parent;
	protected Transform respawnPos;
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
		parent = GameObject.Find("Level " + currentLevelNum);
	}
	protected virtual void SetRespawnPos() {
		respawnPos = GameObject.Find("Level " + currentLevelNum + "/RespawnPosition").transform;
	}

	public virtual void Respawn() {
		Debug.Log("Respawn an object!");
		respawnedObject = Instantiate(prefab, respawnPos.position, Quaternion.identity, parent.transform);
	}
}
