using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPosition : MonoBehaviour {

	protected Vector3 originalPos;

	// Use this for initialization
	void Start () {
			originalPos = transform.position;
	}

	public Vector3 GetRespawnPosition() {
		return originalPos;
	}
}
