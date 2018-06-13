using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour {


	protected int currentLevelNum = 0;
	protected int maxLevel;
	protected string gamePath = "Game/";
	protected GameObject currentLevelObj;
	protected string currentFloorName;
	protected GameObject currentFloor;
	protected int floors;
	protected Transform[] floorObjects;
	protected VRTK.VRTK_BodyPhysics physics;
	protected VRTK.PositionRewind positionRewind;
	protected float waitingTime = 1.0f;
	protected float levelStartTime = 0f;
	protected float levelEndTime;
	protected float levelDuration;
	protected float tempTime;

	protected GlobalControl globalControl;


	//public variables
	public int startLevel = 0;
	public GameStatManager statManager;

	// Use this for initialization
	void Start () {
		physics = GameObject.Find("PlayArea").GetComponent<VRTK.VRTK_BodyPhysics>();
		positionRewind = GameObject.Find("PlayArea").GetComponent<VRTK.PositionRewind>();
		globalControl = GameObject.Find("AppManager").GetComponent<GlobalControl>();
		currentLevelNum = startLevel;
		if (startLevel > 0) {
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
			currentFloorName = ("Environment/Floor " + (floorNum) + "/Floor(Clone)");
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
		DestroyGameObj("Level " + (0) + "(Clone)");
		LoadGameObject(gamePath + "Level " + (level));
		physics.enableBodyCollisions = true;
		for (int i=0; i<level; i++) {
			OpenFloor(i);
			StartCoroutine( WaitAndDestroy(1.0f, ("Environment/Floor " + (i) + "/Floor(Clone)")) );
			statManager.NewStat();
			//StartCoroutine( WaitAndRemoveBodyCollisions(1.7f) );
		}
	}
	protected void HandleWin() {
		Debug.Log("You won!");
		SaveData();
	}

	// Handles a chunk of different status
	protected void HandleLevelStats(bool correct = false) {
		statManager.HandleLevelIndex();
		HandleLevelTime();
		if (correct) {
			statManager.SetCorrect();
		}
		statManager.SetUsingLiftLimitation(globalControl.useLiftLimitation);
	}

	//
	// Public methods
	// ---------------------------------

	public void LoadNextLevel(bool correct = true) {
		string lvl = gamePath + "Level " + (currentLevelNum+1);
		if (Resources.Load(lvl) != null) {
			HandleLevelStats(correct);
			statManager.NewStat();
			LoadGameObject(lvl);
		} else {
			// game is won
			HandleWin();
		}
		DestroyGameObj("Level " + (currentLevelNum) + "(Clone)");
		OpenFloor(currentLevelNum);
		//StartCoroutine( WaitAndDestroy(1.0f, "Level " + (currentLevelNum) + "(Clone)") );
		StartCoroutine( WaitAndDestroy(1.0f, ("Environment/Floor " + (currentLevelNum) + "/Floor(Clone)")) );
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
	// Resets the game. Creates new stats and destroys old levels.
	// Reloads floors
	// Moves player back to original position
	public void ResetGame(int startAtThisLvl = 0) {
		string lvl = ("Level " + currentLevelNum);
		DestroyGameObj(lvl + "(Clone)");
		for (int i=0; i<currentLevelNum; i++) {
			Transform floorParent = GameObject.Find("Environment/Floor " + i).transform;
			GameObject newFloor = (GameObject) Instantiate(Resources.Load(gamePath + "Floor"), floorParent);
		}
		statManager.ResetStats();
		statManager.NewStat();
		positionRewind.RewindPositionToOriginal();
		StartAtLevel(startAtThisLvl);
		currentLevelNum = startAtThisLvl;
	}


	void OnGUI() {
		if (GUI.Button(new Rect(10, 50, 100, 30), "Save data")) {
			SaveData();
		}
		if (GUI.Button(new Rect(10, 90, 100, 30), "Reset level")) {
			ResetLevel();
		}
		if (GUI.Button(new Rect(10, 130, 100, 30), "Next level")) {
			LoadNextLevel(false);
		}
		if (GUI.Button(new Rect(10, 170, 100, 30), "New game")) {
			ResetGame(0);
		}
	}

	//
	// Methods for statistics -----------------
	public void SaveData() {
		HandleLevelStats();
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
