using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour {


	protected int currentLevelNum = 1;
	protected int maxLevel;
	protected string gamePath = "Game/";
	protected GameObject currentLevelObj;
	protected string currentFloorName;
	protected GameObject currentFloor;
	protected int floors;
	protected Transform[] floorObjects;
	protected VRTK.VRTK_BodyPhysics physics;
	protected float waitingTime = 1.0f;
	protected float startTime;
	protected float tempTime;

	// Use this for initialization
	void Start () {
		physics = GameObject.Find("PlayArea").GetComponent<VRTK.VRTK_BodyPhysics>();
	}

	// Update is called once per frame
	void Update () {

	}
	protected void GetCurrentLevel() {

	}
	// Destroys a gameobject of a certain name
	protected void DestroyGameObj(string name) {
		//level2 = GameObject.Find(name);
		Destroy(GameObject.Find(name));
	}
	// Loads a prefab from path
	protected void LoadGameObject(string name) {
		currentLevelObj = (GameObject) Instantiate(Resources.Load(name));
	}
	// Opens the floor
	protected void OpenFloor() {
			currentFloorName = ("Environment/Floor " + (currentLevelNum-1) + "/Floor");
			currentFloor = GameObject.Find(currentFloorName);
			floors = currentFloor.transform.childCount;
			Debug.Log("flloooors " + floors);
			floorObjects = new Transform[floors];
			for (int i=0;i<floors; i++) {
				floorObjects[i] = currentFloor.transform.GetChild(i);
				floorObjects[i].GetComponent<Rigidbody>().isKinematic = false;
			}
			//Destroy(currentFloor);
	}
	protected IEnumerator WaitAndDestroy(float waitTime, string name) {
		yield return new WaitForSeconds(waitTime);
		DestroyGameObj(name);
	}

	//
	// Public methods
	// ---------------------------------

	public void LoadNextLevel() {
		currentLevelNum++;
		Debug.Log("currentlevel num " + currentLevelNum);
		LoadGameObject(gamePath + "Level " + currentLevelNum);
		OpenFloor();
		StartCoroutine( WaitAndDestroy(1.0f, "Level " + (currentLevelNum-1)) );
		StartCoroutine( WaitAndDestroy(1.0f, ("Environment/Floor " + (currentLevelNum-1) + "/Floor")) );
		physics.enableBodyCollisions = true;
	}
}
