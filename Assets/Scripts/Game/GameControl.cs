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

	//public variables
	public int startLevel = 1;

	// Use this for initialization
	void Start () {
		physics = GameObject.Find("PlayArea").GetComponent<VRTK.VRTK_BodyPhysics>();
		if (startLevel > 1) {
			StartAtLevel(startLevel);
		}
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
	// Loads a prefab from path
	protected void LoadGameObject(string name) {
		currentLevelObj = (GameObject) Instantiate(Resources.Load(name));
	}
	// Opens the floor
	protected void OpenFloor(int floorNum) {
			currentFloorName = ("Environment/Floor " + (floorNum) + "/Floor");
			currentFloor = GameObject.Find(currentFloorName);
			floors = currentFloor.transform.childCount;
			//Debug.Log("flloooors " + floors);
			floorObjects = new Transform[floors];
			for (int i=0;i<floors; i++) {
				floorObjects[i] = currentFloor.transform.GetChild(i);
				floorObjects[i].GetComponent<Rigidbody>().isKinematic = false;
			}
			//Destroy(currentFloor);
	}
	protected IEnumerator WaitAndDestroy(float waitTime, string name) {
		Debug.Log("wait and destroy " + name);
		yield return new WaitForSeconds(waitTime);
		DestroyGameObj(name);
	}
	protected IEnumerator WaitAndRemoveBodyCollisions(float waitTime) {
		yield return new WaitForSeconds(waitTime);
		physics.enableBodyCollisions = false;
	}

	// starts the game at specified level
	protected void StartAtLevel(int level) {
		LoadGameObject(gamePath + "Level " + (level));
		for (int i=1; i<level; i++) {
			OpenFloor(i);
			DestroyGameObj("Level " + (currentLevelNum));
			StartCoroutine( WaitAndDestroy(1.0f, ("Environment/Floor " + (i) + "/Floor")) );
			physics.enableBodyCollisions = true;
			StartCoroutine( WaitAndRemoveBodyCollisions(1.7f) );
		}
	}

	//
	// Public methods
	// ---------------------------------

	public void LoadNextLevel() {
		Debug.Log("currentlevel num " + (currentLevelNum));
		LoadGameObject(gamePath + "Level " + (currentLevelNum+1));
		OpenFloor(currentLevelNum);
		StartCoroutine( WaitAndDestroy(1.0f, "Level " + (currentLevelNum) + "(Clone)") );
		StartCoroutine( WaitAndDestroy(1.0f, ("Environment/Floor " + (currentLevelNum) + "/Floor")) );
		physics.enableBodyCollisions = true;
		currentLevelNum++;
		StartCoroutine( WaitAndRemoveBodyCollisions(1.3f) );
	}
	public int GetCurrentLevel() {
		return currentLevelNum;
	}
	public void ResetLevel() {
		string lvl = ("Level " + currentLevelNum);
		DestroyGameObj(lvl + "(Clone)");
		LoadGameObject(gamePath + lvl);
	}
	public void SaveData() {}

	void OnGUI() {
		if (GUI.Button(new Rect(10, 50, 100, 30), "Save data")) {
			SaveData();
		}
		if (GUI.Button(new Rect(10, 90, 100, 30), "Reset level")) {
			ResetLevel();
		}
	}
}
