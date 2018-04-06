using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnInfinite : RespawnObject {

	protected float lastSpawn = 0f;
	protected float cooldownTime = 3f;

	public int parentNum;

	// Use this for initialization
	void Start () {
		base.Start();
	}

	// Update is called once per frame
	void Update () {
		if(Time.time >= lastSpawn + cooldownTime)
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
		respawnedObject = Instantiate(prefab, respawnPos.position+localPos, Quaternion.Euler(0f,0f,90f), parent.transform);
	}
}
