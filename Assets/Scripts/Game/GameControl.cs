using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour {


	protected int currentLevelNum = 1;
	protected int maxLevel;
	protected string gamePath = "Game/";
	protected GameObject currentLevelObj;

	// Use this for initialization
	void Start () {

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
		currentLevelObj = (GameObject) Instantiate(Resources.Load("Game/TestLevel1"));
	}
	// Opens the floor
	protected void OpenFloor() {
			Debug.Log("Environment/Floor " + (currentLevelNum-1) + "/Floor");
		Destroy(GameObject.Find("Environment/Floor " + (currentLevelNum-1) + "/Floor"));
	}

	//
	// Public methods
	// ---------------------------------

	public void LoadNextLevel() {
		currentLevelNum++;
		LoadGameObject(gamePath + "TestLevel" + currentLevelNum);
		OpenFloor();
		DestroyGameObj("Level " + (currentLevelNum-1));
	}
}
