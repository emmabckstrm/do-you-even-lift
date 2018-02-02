using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalControl : MonoBehaviour
{
    public static GlobalControl Instance;
    public SceneStatistics[] sceneStats;
    private int totalScenes;
    public enum MovementLimitationTypes
    {
        VelocityAnyDirection,
        VelocityVertical,
        AccelerationAnyDirection,
        AccelerationVertical,
    }
    [Tooltip("Determines in what direction the movement limit is")]
    public MovementLimitationTypes movementLimitType = MovementLimitationTypes.VelocityAnyDirection;
    public int numberOfDiscriminations = 3;
    public float minimumWeight = 1.0f;
    public float weightStep = 1.0f;
    public GameObject weightPrefab;
    private int discriminationsPerformed = 0;
    private float[] weights;

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
        Debug.Log("Setting up stuff!");
        SetupSceneStats();
        SetupWeightDiscrimination();
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
        if (stats != null) {
            sceneStats[scene] = stats;
        }
        
    }
    // Serialize data to json format
    public string SerializeData()
    {
        string serializedData = "[";


        for (int i=0;i< sceneStats.Length; i++)
        {
            SceneStatistics sceneStat = sceneStats[i];
            serializedData += sceneStat.Serialize();
            if (i < sceneStats.Length-1)
            {
                serializedData += ",";
            }            
            // TODO: Fix so that it dynamically loops through scenneStat properties
            /*
            PropertyInfo[] properties = typeof(SceneStatistics).GetProperties();
            Debug.Log("hello " + properties + " length " + properties.Length);
            foreach (PropertyInfo property in properties)
            {
                Debug.Log("yoyoyo00");
                Debug.Log("Name: " + property.Name + " Value: " + property.GetValue(sceneStat, null));
            }*/
        }
        serializedData += "]";
        return serializedData;
    }

    // Sets up an array of weights as numbers for weight discrimination scenes
    private void SetupWeightDiscrimination()
    {
        weights = new float[numberOfDiscriminations * 2];
        float[] tempWeights = new float[weights.Length];
        int j = weights.Length-1;
        int k = 0;
        // sets weights from smallest to largest
        for (int i=0;i< weights.Length; i++) {
            tempWeights[i] = minimumWeight + (weightStep * i);
        }
        // pairing weights lightest with heavinest etc
        for (int i =0;i<weights.Length; i=i+2)
        {
            weights[i] = tempWeights[k];
            weights[i + 1] = tempWeights[j];
            j--;
            k++;
        }
        

    }
    public bool CheckDiscriminationsPerformed()
    {
        if (discriminationsPerformed < numberOfDiscriminations) {
            return true;
        } else
        {
            return false;
        }
    }
    public void PerformDiscrimination()
    {
        discriminationsPerformed++;
    }
    public int GetDiscriminations()
    {
        return numberOfDiscriminations;
    }
    public int GetDiscriminationsPerformed()
    {
        return discriminationsPerformed;
    }
    public float[] GetWeights()
    {
        return weights;
    }
    public GameObject GetWeightPrefab()
    {
        return weightPrefab;
    }


}
