using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour {
	/*
	Placed on parent object of buttons.
	Keeps track of how many buttons that have been activated and compares to its number of children.
	*/

	protected GameControl gameControl;
	protected int numberOfChildren = 0;
	protected int pushedButtons = 0;
	private float timeStart;
	private bool delayingTime;
	private float timeDiff;
	private float timeDelay = 1.5f;
	protected bool levelWon = false;

	// Use this for initialization
	void Start () {
		gameControl = GameObject.Find("AppManager").GetComponent<GameControl>();
		UpdateNumberOfChildren();
	}

	// Update is called once per frame
	void Update () {
		// Checks if a current timeDelay is in progress, if so, check the delay has reached the limit
		if (delayingTime) {
				timeDiff = Time.time - timeStart;
				if (timeDiff >= timeDelay && levelWon) {
						//Debug.Log("We have a winner!");
						levelWon = false;
						//if (respawnScript != null) {
						//	respawnScript.StopRespawn();
						//}
						gameControl.LoadNextLevel();

				}
		}
	}
	// Counts how many children the handler has
	public void UpdateNumberOfChildren()
	{
			numberOfChildren = 0;
			numberOfChildren = transform.childCount;
	}
	// Adds a number to total pushed buttons count
	public void PushButton() {
		pushedButtons++;
		gameControl.IncreaseButtonTriggers();
		CheckPushedButtons();
	}
	// Removes a number to total pushed buttons count
	public void UnpushButton() {
		pushedButtons--;
		CheckPushedButtons();
	}
	public void CheckPushedButtons() {
		if (pushedButtons >= numberOfChildren) {
			delayingTime = true;
			levelWon = true;
			timeStart = Time.time;
		}
		else {
			delayingTime = false;
		}
	}

}
