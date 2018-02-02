using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour {

    public int currentSceneIndex;
    public int nextSceneIndex;
    private bool reloadScene = false;
    private StatManager statManagerScript;

    public GameObject prefab;
    public float gridX = 5f;
    public float gridY = 5f;
    public float spacing = 2f;

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
        Debug.Log("OnEnable called");
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
        Debug.Log("OnSceneLoaded: " + scene.name);
        CreateObjects();
        //Debug.Log(mode);
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
        GlobalControl.Instance.PerformDiscrimination();
        // only adds nextScene index if the scene is not reloadable
        if (!CheckIfReloadableScene())
        {
            nextSceneIndex++;
        }
        LoadNextScene();
    }
    // check if the current scene should be reloaded
    private bool CheckIfReloadableScene()
    {
        if (SceneManager.GetActiveScene().name.Contains("discrimination") && SceneManager.GetActiveScene().name.Contains("pair"))
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
            statManagerScript.SaveData(currentSceneIndex);
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            statManagerScript.SaveData(currentSceneIndex);
            statManagerScript.PrintData();
            return;
        }
    }
    //dynamically loads weights
    private void CreateObjects()
    {
        if (SceneManager.GetActiveScene().name.Contains("discrimination"))
        {
            if (SceneManager.GetActiveScene().name.Contains("pair"))
            {
                // if weight discrimination pair
                CreateObjectsDiscriminationPair();
            } else
            {
                // if weight discrimination group
                CreateObjectsDiscriminationGroup();
            }
        }
         
    }
    // creates objects for weight discrimination in pars
    private void CreateObjectsDiscriminationPair()
    {
        GameObject weight;
        float[] weights = GlobalControl.Instance.GetWeights();
        int j = GlobalControl.Instance.GetDiscriminationsPerformed()*2;

        for (int i = 0; i < 2; i++)
        {
            Vector3 pos = new Vector3(-3.9f, 1.4f, 0.5f + i * 0.45f);
            weight = Instantiate(prefab, pos, Quaternion.identity);
            //weight.transform.localScale = weight.transform.localScale * 0.7f;
            weight.transform.rotation = Quaternion.Euler(0, 90f, 0);
            weight.GetComponent<Rigidbody>().mass = weights[j];
            j++;
        }
    }
    // creates obejcts for weight discrimination in group
    private void CreateObjectsDiscriminationGroup()
    {
        Debug.Log("yoyo");
        GameObject weight;
        float[] weights = GlobalControl.Instance.GetWeights();
        //int num = GlobalControl.Instance.GetDiscriminations() * 2;
        for (int i=0; i<weights.Length; i++)
        {
            Vector3 pos = new Vector3(-3.9f, 1.4f, 0.5f + i * 0.45f);
            weight = Instantiate(prefab, pos, Quaternion.identity);
            //weight.transform.localScale = weight.transform.localScale * 0.7f;
            weight.transform.rotation = Quaternion.Euler(0, 90f, 0);
            weight.GetComponent<Rigidbody>().mass = weights[i];
        }
    }

    
}
