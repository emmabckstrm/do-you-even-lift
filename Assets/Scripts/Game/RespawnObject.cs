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
	void Start () {
		gameControl = GameObject.Find("AppManager").GetComponent<GameControl>();
		currentLevelNum = gameControl.GetCurrentLevel();
		parent = GameObject.Find("Level " + currentLevelNum);
		respawnPos = GameObject.Find("Level " + currentLevelNum + "/RespawnPosition").transform;
		//respawnPos = parent.Find("RespawnPosition").transform;
	}

	public void Respawn() {
		Debug.Log("Respawn an object!");
		respawnedObject = Instantiate(prefab, respawnPos.position, Quaternion.identity, parent.transform);
	}
}
