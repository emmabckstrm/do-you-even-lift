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
		// shuffles the order
		mass = massList.OrderBy(a => rng.Next()).ToList();
		SpawnBarrels();
	}

	public void SpawnBarrels() {
		int i = 0;
		foreach (Transform child in transform)
		{
			SpawnBarrel(child, i);
			i++;
		}
	}

	protected void SpawnBarrel(Transform t, int i) {
		Respawn(t);
		respawnedObject.GetComponent<Rigidbody>().mass = mass[i];
		respawnedObject.GetComponent<VRTK.InteractableObjectTrackMovement2>().UpdateAngularDrag();
		respawnedObject.GetComponent<VRTK.InteractableObjectTrackMovement2>().UpdateMovementLimitValue();
	}

	public virtual void Respawn(Transform customPos) {
		respawnedObject = Instantiate(prefab, customPos.position, Quaternion.identity, transform.parent.transform);
	}
}
