using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZoneHandler : MonoBehaviour {

    public int numberOfChildren = 0;
    public int numberOfCorrectDrops = 0;
    public float timeDelay = 2.0f;
    private float timeStart;
    private bool delayingTime;
    private float timeDiff;
    private SceneManagement sceneManager;
    private StatManager statManager;

	// Use this for initialization
	void Start () {
        sceneManager = GameObject.Find("AppManager").GetComponent<SceneManagement>();
        statManager = GameObject.Find("AppManager").GetComponent<StatManager>();
        foreach (Transform child in transform)
        {
            numberOfChildren++;
        }
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
        Debug.Log("Checking correct drops");
        if (numberOfCorrectDrops == numberOfChildren)
            {
                Debug.Log("Yes, we're maxed out!");
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
        sceneManager.NextScene();
    }

}
