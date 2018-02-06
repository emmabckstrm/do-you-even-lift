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
    private float[] weightsPair;
    private float[] weightsAll;
    public List<List<float>> weights2;

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
        SetupSceneStats();
        SetupWeightDiscrimination();
    }
    // Sets up an array with scene stats corresponding to number of scenes
    private void SetupSceneStats() {
        totalScenes = SceneManager.sceneCountInBuildSettings + numberOfDiscriminations-1;
        sceneStats = new SceneStatistics[totalScenes];
        for (int i = 0; i < sceneStats.Length; i++) {
            sceneStats[i] = new SceneStatistics();
        }
    }
    // saves scene data
    public void SaveSceneData(int scene, SceneStatistics stats) {
        if (stats != null) {
            if (discriminationsPerformed > 0)
            {
                sceneStats[scene + discriminationsPerformed-1] = stats;
            }
            else
            {
                sceneStats[scene] = stats;
            }
            
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
        int totalWeights = numberOfDiscriminations * 2;
        weightsPair = new float[totalWeights];
        weightsAll = new float[totalWeights];
        float[][] tempList = new float[numberOfDiscriminations][];
        int x = totalWeights - 1;
        int y = 0;
        // sets weights from smallest to largest
        for (int i=0;i< totalWeights; i++) {
            weightsAll[i] = minimumWeight + (weightStep * i);
        }
        // pairing weights lightest with heavinest etc
        for (int i = 0; i < numberOfDiscriminations; i++)
        {
            List<int> randomIndex = RandomListOrder(2);
            float[] tempP = new float[2];
            tempP[randomIndex[0]] = weightsAll[y];
            tempP[randomIndex[1]] = weightsAll[x];
            tempList[i] = tempP;
            x--;
            y++;
        }
        tempList = RandomizeFloatArrayArray(tempList);
        weightsAll = RandomizeFloatArray(weightsAll);
        x = 0;
        for (int i=0; i<numberOfDiscriminations; i++)
        {
            for (int u=0; u<2; u++)
            {
                weightsPair[x] = tempList[i][u];
                x++;
            }
        }

    }
    // Randomizes a list, takes an source list and randomizes the objects
    public float[] RandomizeFloatArray(float[] sourceList)
    {
        float[] result = new float[sourceList.Length];
        List<int> order = RandomListOrder(sourceList.Length);
        for (int i = 0; i < sourceList.Length; i++)
        {
            int x = order[i];
            result[i] = (sourceList[x]);
        }
        return result;
    }
    // Randomizes a list, takes an source list and randomizes the objects
    public float[][] RandomizeFloatArrayArray(float[][] sourceList)
    {
        float[][] result = new float[sourceList.Length][];
        List<int> order = RandomListOrder(sourceList.Length);
        for (int i = 0; i < sourceList.Length; i++)
        {
            int x = order[i];
            result[i] = (sourceList[x]);
        }
        return result;
    }
    // Randomizes a list, takes an source list and randomizes the objects
    public List<float> RandomizeFloatList(List<float> sourceList)
    {
        List<float> result = new List<float>();
        List<int> order = RandomListOrder(sourceList.Count);
        for (int i = 0; i < sourceList.Count; i++)
        {
            int x = order[i];
            result.Add(sourceList[x]);
        }
        return result;
    }
    // Randomizes a list, takes an source list and randomizes the objects
    public List<List<float>> RandomizeFloatListList(List<List<float>> sourceList)
    {
        List<List<float>> result = new List<List<float>>();
        List<int> order = RandomListOrder(sourceList.Count);
        for (int i = 0; i < sourceList.Count; i++)
        {
            int x = order[i];
            result.Add(sourceList[x]);
        }
        return result;
    }
    // Randomizes a list order, returns a list with int indexes
    private List<int> RandomListOrder(int size)
    {

        int x = 0;
        int rand;
        List<int> chosenNums = new List<int>();
        while (x < size)
        {
            rand = RandomNumber(0, size);
            if (!chosenNums.Contains(rand))
            {
                x++;
                chosenNums.Add(rand);
            }
        }
        return chosenNums;
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
    public float[] GetWeightsPair()
    {
        return weightsPair;
    }
    public float[] GetWeightsGroup()
    {
        return weightsAll;
    }
    public GameObject GetWeightPrefab()
    {
        return weightPrefab;
    }
    private int RandomNumber(int min, int max)
    {
        System.Random random = new System.Random();
        return random.Next(min, max);

    }


}
