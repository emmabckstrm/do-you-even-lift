using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour {

    public int currentSceneIndex;
    public int nextSceneIndex;
    private StatManager statManagerScript;

    private void Start()
    {
        statManagerScript = GetComponent<StatManager>();
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        nextSceneIndex = currentSceneIndex;
    }

    void OnGUI()
    {
        // If the button is pressed
        if (GUI.Button(new Rect(10, 10, 100, 30), "Next Scene"))
        {
            
            nextSceneIndex++;
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings) {
                statManagerScript.SaveData(currentSceneIndex);
                SceneManager.LoadScene(nextSceneIndex);
            } else
            {
                return;
            }
        }
    }

    
}
