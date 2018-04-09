using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingBarrel : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
	// Destroys a gameobject of a certain name
	protected void DestroyGameObj(string name) {
		GameObject obj = GameObject.Find(name);
		if (obj != null) {
			Destroy(obj);
		}
	}

	protected IEnumerator WaitAndDestroy(float waitTime, string name) {
		yield return new WaitForSeconds(waitTime);
		DestroyGameObj(name);
	}
}
