using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZoneHandler : MonoBehaviour {

    public int numberOfChildren = 0;
    public int numberOfCorrectDrops = 0;
    public float timeDelay = 2.0f;
    public bool changeSceneAutomatically = false;
    private float timeStart;
    private bool delayingTime;
    private float timeDiff;
    private SceneManagement sceneManager;
    private StatManager statManager;

	// Use this for initialization
	void Start () {
        sceneManager = GameObject.Find("AppManager").GetComponent<SceneManagement>();
        statManager = GameObject.Find("AppManager").GetComponent<StatManager>();
        UpdateNumberOfChildren();
    }
	
	// Update is called once per frame
	void Update () {
        // Checks if a current timeDelay is in progress, if so, check the delay has reached the limit
        if (delayingTime) {
            timeDiff = Time.time - timeStart;
            if (timeDiff >= timeDelay) {
                UpdateAndContinue(true);
            }
        }
	}
    public void UpdateNumberOfChildren()
    {
        numberOfChildren = 0;
        foreach (Transform child in transform)
        {
            numberOfChildren++;
        }
    }
    // adds a counter to correctly dropped objects
    public void addCorrectDrop() {
        numberOfCorrectDrops++;
        checkCorrectDrops();
    }
    public void removeCorrectDrop() {
        numberOfCorrectDrops--;
        checkCorrectDrops();
    }
    // controls the number of dropped objects if it is equal to number of children
    private void checkCorrectDrops() {
        if (numberOfCorrectDrops >= numberOfChildren)
            {
                delayingTime = true;
                timeStart = Time.time;
            }
            else {
                delayingTime = false;
            }
    }

    private void UpdateAndContinue(bool correct)
    {
        statManager.localSceneStats.correct = correct;
        if (changeSceneAutomatically)
        {
            sceneManager.NextScene();
        } 
    }

}
