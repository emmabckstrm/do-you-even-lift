using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalControl : MonoBehaviour
{
    public static GlobalControl Instance;
    public SceneStatistics[] sceneStats;
    private int totalScenes;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        //If instance already exists and it's not this:
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        //Sets this to not be destroyed when reloading scene0
        DontDestroyOnLoad(gameObject);
    }
    // Sets up an array with scene stats corresponding to number of scenes
    private void SetupSceneStats() {
        totalScenes = SceneManager.sceneCountInBuildSettings;
        sceneStats = new SceneStatistics[totalScenes];
        for (int i = 0; i < totalScenes; i++) {
            sceneStats[i] = new SceneStatistics();
        }
    }
    // saves scene data
    public void SaveSceneData(int scene, SceneStatistics stats=null) {
        sceneStats[scene] = stats;
    }
}
