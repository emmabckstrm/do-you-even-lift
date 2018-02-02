using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class StatManager : MonoBehaviour {

    public SceneStatistics localSceneStats;

	// Use this for initialization
	void Start () {
        localSceneStats = new SceneStatistics();
	}
	
    // Saves local data to global controller
    public void SaveData(int index)
    {
        GlobalControl.Instance.SaveSceneData(index, localSceneStats);
    }
    public void PrintData()
    {
        Debug.Log(GlobalControl.Instance.SerializeData());
    }

    
}
