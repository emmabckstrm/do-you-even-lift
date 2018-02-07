using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class StatManager : MonoBehaviour {

    public SceneStatistics localSceneStats;
    private SceneManagement sceneManager;

    // Use this for initialization
    void Start () {
        localSceneStats = new SceneStatistics();
        sceneManager = gameObject.GetComponent<SceneManagement>();

    }
	
    // Saves local data to global controller
    public void SaveData(int index)
    {
        localSceneStats.sceneNumber = index;
        GlobalControl.Instance.SaveSceneData(index, localSceneStats);
    }
    public void PrintData()
    {
        Debug.Log(GlobalControl.Instance.SerializeData());
    }
    public void ResetLocalData()
    {
        localSceneStats = new SceneStatistics();
    }

    public void AddCSVStatPerGrab(float startTime, float endTime, float weight, string hand, bool forceRelease)
    {
        float duration = endTime - startTime;
        int sceneNumber = sceneManager.GetSceneNumber();
        string CSVStatPerGrab = sceneNumber + "," + startTime + "," + endTime + "," + duration + "," + weight + "," + hand + "," + forceRelease;
        localSceneStats.AddCSVStatPerGrab(CSVStatPerGrab);
    }


}
