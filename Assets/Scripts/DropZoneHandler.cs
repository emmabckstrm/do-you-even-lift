using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZoneHandler : MonoBehaviour {

    public int numberOfChildren = 0;
    public int numberOfValidDrops = 0;
    public float timeDelay = 2.0f;
    public bool changeSceneAutomatically = false;
    private float timeStart;
    private bool delayingTime;
    private float timeDiff;
    private List<Transform> children;
    //private List<DropZone> dropZones;
    private DropZone[] dropZones;
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
                UpdateAndContinue(CheckCorrectDrops());
            }
        }
	}
    public void UpdateNumberOfChildren()
    {
        numberOfChildren = 0;
        numberOfChildren = transform.childCount;


        dropZones = transform.GetComponentsInChildren<DropZone>();

    }
    // adds a counter to correctly dropped objects
    public void AddValidDrop() {
        numberOfValidDrops++;
        CheckValidDrops();
    }
    public void RemoveValidDrop() {
        numberOfValidDrops--;
        CheckValidDrops();
    }
    // controls the number of dropped objects if it is equal to number of children
    private void CheckValidDrops() {
        Debug.Log("children and valid drops " + numberOfChildren + " " + numberOfValidDrops);
        if (numberOfValidDrops >= numberOfChildren)
        {
            Debug.Log("Valid drops!");
            CheckCorrectDrops();
            delayingTime = true;
            timeStart = Time.time;
        }
        else {
            delayingTime = false;
        }
    }
    // controls if the dropped objects are in weight order
    private bool CheckCorrectDrops()
    {
        float last = -1f;
        int points = 0;
        foreach (DropZone dropZone in dropZones)
        {
            if (dropZone.placedWeight > last)
            {
                // good!
                points++;
                last = dropZone.placedWeight;
            }
        }
        if (points == numberOfChildren)
        {
            Debug.Log("Yes, correct!");
            statManager.localSceneStats.correct = true;
            return true;
        } else
        {
            Debug.Log("Not correct!");
            statManager.localSceneStats.correct = false;
            return false;
        }
    }

    private void UpdateAndContinue(bool correct)
    {
        statManager.localSceneStats.correct = correct;
        numberOfValidDrops = 0;
        if (changeSceneAutomatically)
        {
            sceneManager.NextScene();
        } 
    }

}
