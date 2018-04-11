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
	protected float levelStartTime = 0f;
	protected float levelEndTime;
	protected float levelDuration;
	protected float tempTime;


	//public variables
	public int startLevel = 1;
	public GameStatManager statManager;

	// Use this for initialization
	void Start () {
		physics = GameObject.Find("PlayArea").GetComponent<VRTK.VRTK_BodyPhysics>();
		if (startLevel > 1) {
			StartCoroutine( WaitAndStartAtLevel(2f, startLevel) );
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
		yield return new WaitForSeconds(waitTime);
		DestroyGameObj(name);
	}
	protected IEnumerator WaitAndRemoveBodyCollisions(float waitTime) {
		yield return new WaitForSeconds(waitTime);
		physics.enableBodyCollisions = false;
	}
	protected IEnumerator WaitAndStartAtLevel(float waitTime, int level) {
		yield return new WaitForSeconds(waitTime);
		StartAtLevel(level);
	}

	// starts the game at specified level
	protected void StartAtLevel(int level) {
		currentLevelNum = level;
		LoadGameObject(gamePath + "Level " + (level));
		physics.enableBodyCollisions = true;
		DestroyGameObj("Level " + (1) + "(Clone)");
		for (int i=1; i<level; i++) {
			OpenFloor(i);
			StartCoroutine( WaitAndDestroy(1.0f, ("Environment/Floor " + (i) + "/Floor")) );
			statManager.NewStat();
			//StartCoroutine( WaitAndRemoveBodyCollisions(1.7f) );
		}
	}

	//
	// Public methods
	// ---------------------------------

	public void LoadNextLevel() {
		HandleLevelTime();
		statManager.NewStat();
		LoadGameObject(gamePath + "Level " + (currentLevelNum+1));
		OpenFloor(currentLevelNum);
		StartCoroutine( WaitAndDestroy(1.0f, "Level " + (currentLevelNum) + "(Clone)") );
		StartCoroutine( WaitAndDestroy(1.0f, ("Environment/Floor " + (currentLevelNum) + "/Floor")) );
		physics.enableBodyCollisions = true;
		currentLevelNum++;
		//StartCoroutine( WaitAndRemoveBodyCollisions(1.3f) );
	}
	public int GetCurrentLevel() {
		return currentLevelNum;
	}
	public void ResetLevel() {
		string lvl = ("Level " + currentLevelNum);
		DestroyGameObj(lvl + "(Clone)");
		LoadGameObject(gamePath + lvl);
		statManager.AddReset();
	}


	void OnGUI() {
		if (GUI.Button(new Rect(10, 50, 100, 30), "Save data")) {
			SaveData();
		}
		if (GUI.Button(new Rect(10, 90, 100, 30), "Reset level")) {
			ResetLevel();
		}
	}

	//
	// Methods for statistics -----------------
	public void SaveData() {
		statManager.WriteDataToFile();
		PrintData();
	}

	public void PrintData()
	{
			Debug.Log(statManager.SerializeData());
	}

	public void HandleLevelTime() {
		levelEndTime = Time.time;
		statManager.HandleLevelTime(levelStartTime, levelEndTime);
		levelStartTime = levelEndTime;
	}
	public void IncreaseButtonCollisions() {
		statManager.IncreaseButtonCollisions();
	}
	public void DecreaseButtonCollisions() {
		statManager.DecreaseButtonCollisions();
	}
	public void IncreaseButtonTriggers() {
		statManager.IncreaseButtonTriggers();
	}
	public void DecreaseButtonTriggers() {
		statManager.DecreaseButtonTriggers();
	}
	public void AddTimeGrabbing(float time) {
		statManager.AddTimeGrabbing(time);
	}
	public void AddGrab() {
		statManager.AddGrab();
	}
	public void AddCSVStatPerGrab(float startTime, float endTime, float weight, string hand)
	{
			statManager.HandleFirstInteraction(startTime - levelStartTime);
			statManager.AddCSVStatPerGrab(startTime, endTime, weight, hand);
	}
}
