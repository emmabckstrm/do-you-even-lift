using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnInfinite : RespawnObject {

	protected float lastSpawn = 0f;

	protected bool continueRespawn = true;

	public int parentNum;
	public float cooldownTime = 6f;
	public float angleX = 0f;
	public float angleY = 0f;
	public float angleZ = 90f;


	// Use this for initialization
	void Start () {
		base.Start();
	}

	// Update is called once per frame
	void Update () {
		if(continueRespawn && Time.time >= (lastSpawn + cooldownTime))
		 {
		     Respawn();
		     lastSpawn = Time.time;
		 }
	}

	protected override void SetParent() {
		parent = this.transform.gameObject;
	}
	protected override void SetRespawnPos() {
		respawnPos = transform;
	}

	public void Respawn() {
		Vector3 localPos = new Vector3(0.3f,0f,0f); // added because of the rotation
		respawnedObject = Instantiate(prefab, respawnPos.position+localPos, Quaternion.Euler(angleX,angleY,angleZ), parent.transform);
	}
	public void StopRespawn() {
		continueRespawn = false;
	}
	public void ContinueRespawn() {
		continueRespawn = true;
	}
}
