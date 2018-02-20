using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour {

    public int currentSceneIndex;
    public int nextSceneIndex;
    private bool reloadScene = false;
    private float timeSceneStart;
    private StatManager statManagerScript;
    private int sceneNumber;

    private SceneSettings sceneSettings;
    public GameObject prefab;
    private CreateObjects createObjectsScript;

    private void Start()
    {
        statManagerScript = GetComponent<StatManager>();
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        nextSceneIndex = currentSceneIndex;
        prefab = GlobalControl.Instance.GetWeightPrefab();
    }
    // called first
    void OnEnable()
    {
        createObjectsScript = GameObject.Find("SceneSettings").transform.GetComponent<CreateObjects>();
        prefab = GlobalControl.Instance.GetWeightPrefab();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // unsubscribe
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        sceneSettings = GameObject.Find("SceneSettings").transform.GetComponent<SceneSettings>();
        createObjectsScript = GameObject.Find("SceneSettings").transform.GetComponent<CreateObjects>();
        
        createObjectsScript.Create();
        timeSceneStart = Time.time;
        sceneNumber++;
    }

    void OnGUI()
    {
        // If the NextScene button is pressed
        if (GUI.Button(new Rect(10, 10, 100, 30), "Next Scene"))
        {
            NextScene();
        }
        
            /*
            if (GUI.Button(new Rect(120, 40, 100, 30), "Save all data"))
            {
                statManagerScript.SaveData(currentSceneIndex);
                statManagerScript.PrintData();
            }*/   
    }

    public void NextScene()
    {
        StopTime();
        if (sceneSettings.performDiscriminations)
        {
            GlobalControl.Instance.PerformDiscrimination();
        }
        statManagerScript.SaveData(SceneManager.GetActiveScene().buildIndex);
        
        // only adds nextScene index if the scene is not reloadable
        if (!CheckIfReloadableScene())
        {
            nextSceneIndex++;
            if (sceneSettings.reloadableScene)
            {
                GlobalControl.Instance.ResetDiscriminations();
            }
        }
        
        
        LoadNextScene();
    }
    // check if the current scene should be reloaded
    private bool CheckIfReloadableScene()
    {
        if (sceneSettings.reloadableScene)
        {
            return (GlobalControl.Instance.CheckDiscriminationsPerformed());
        }
        else {
            return false;
        }
    }
    // loads the next scene
    private void LoadNextScene()
    {
        // loads next scene
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            statManagerScript.ResetLocalData();
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            statManagerScript.PrintData();
            return;
        }
    }
    private void StopTime()
    {
        float duration = Time.time - timeSceneStart;
        statManagerScript.localSceneStats.timeToCompletion = duration;
    }
  

    public int GetSceneNumber()
    {
        return sceneNumber;
    }

    
}
