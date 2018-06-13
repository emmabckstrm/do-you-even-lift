using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using System.Random;
using UnityEngine;

public class BarrelSpawnShuffleMass : MonoBehaviour {

	public GameObject prefab;
	public List<float> massList;

	protected List<float> mass;
	protected GameObject respawnedObject;

	private static System.Random rng = new System.Random();

	// Use this for initialization
	void Start () {
		Debug.Log("hello");
		mass = massList.OrderBy(a => rng.Next()).ToList();
		SpawnBarrels();
	}

	public void SpawnBarrels() {
		int i = 0;
		foreach (Transform child in transform)
		{
			Respawn(child);
			respawnedObject.GetComponent<Rigidbody>().mass = mass[i];
			i++;
		}
	}

	public virtual void Respawn(Transform customPos) {
		respawnedObject = Instantiate(prefab, customPos.position, Quaternion.identity, transform.parent.transform);
	}
}
