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
        if (SceneManager.GetActiveScene().name.Contains("discrimination") && SceneManager.GetActiveScene().name.Contains("pair"))
        {
            GlobalControl.Instance.PerformDiscrimination();
        }
        statManagerScript.SaveData(SceneManager.GetActiveScene().buildIndex);
        
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
            statManagerScript.ResetLocalData();
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            statManagerScript.PrintData();
            return;
        }
    }
    //dynamically loads weights for scenes with 'discrimination' in name
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
            Vector3 localPos = new Vector3(0, 0, i*0.45f);
            Transform parent = GameObject.Find("Weight placement").transform;
            weight = Instantiate(prefab, parent.position, Quaternion.identity, parent);
            weight.transform.localPosition = localPos;
            weight.transform.rotation = Quaternion.Euler(0, 90f, 0);
            weight.GetComponent<Rigidbody>().mass = weights[j];
            weight.GetComponent<VRTK.InteractableObjectTrackMovement>().UpdateMovementLimitValue();
            j++;
        }
    }
    // creates obejcts for weight discrimination in group
    private void CreateObjectsDiscriminationGroup()
    {
        GameObject weight;
        float[] weights = GlobalControl.Instance.GetWeights();
        //int num = GlobalControl.Instance.GetDiscriminations() * 2;
        for (int i=0; i<weights.Length; i++)
        {
            Vector3 localPos = new Vector3(0, 0, i * 0.3f);
            Transform parent = GameObject.Find("Weight placement").transform;
            weight = Instantiate(prefab, parent.position, Quaternion.identity, parent);
            weight.GetComponent<Rigidbody>().mass = weights[i];
            weight.transform.localPosition = localPos;
            weight.transform.rotation = Quaternion.Euler(0, 90f, 0);
            weight.GetComponent<VRTK.InteractableObjectTrackMovement>().UpdateMovementLimitValue();
        }
    }

    
}
