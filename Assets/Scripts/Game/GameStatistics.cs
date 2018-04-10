using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GameStatistics
{
    public string sceneName;
    public int sceneNumber;
    public float timeToCompletion; // Total time to complete task
    public float timeGrabbingObj; // total time grabbing an object
		public float timeToFirstInteraction = 0f;
    public bool correct; // if the solution was correct
    public int totalTouches; // total number of touches to objects
    public int totalGrabs; // total number of times grabbing any object
		public int totalButtonCollisions = 0;
		public int totalButtonTriggers = 0;
    public float pair = -1; // the lightest weight of the pair
    public string weightOrder;

		protected bool firstInteraction = false;

    private string CSVStatPerGrabHeader = "sceneNumber, startTime, endTime, duration, weight, pair, sceneName\n";
    private string CSVStatPerGrab = "";


    public string SerializeJson() {
        string serializedData = "";
        serializedData += "{";

        serializedData += ("\"sceneNumber\":" + sceneNumber);
        serializedData += (",\"timeToCompletion\":" + timeToCompletion);
        serializedData += (",\"timeGrabbingObj\":"+timeGrabbingObj);
				serializedData += (",\"timeToFirstInteraction\":"+timeToFirstInteraction);
        serializedData += (",\"correct\":\"" + correct +"\"");
        serializedData += (",\"totalTouches\":" + totalTouches);
        serializedData += (",\"totalGrabs\":" + totalGrabs);
				serializedData += (",\"totalButtonCollisions\":" + totalButtonCollisions);
				serializedData += (",\"totalButtonTriggers\":" + totalButtonTriggers);
        serializedData += (",\"pair\":\"" + pair + "\"");
        serializedData += (",\"weightOrder\":\"" + weightOrder + "\"");
        serializedData += (",\"sceneName\":\"" + sceneName + "\"");
        serializedData += "}";

        return serializedData;
    }
    public string SerializeCSVHeader()
    {
        string serializedData = "";
        serializedData += "sceneNumber,timeToCompletion,timeGrabbingObj,";
				serializedData += "timeToFirstInteraction,correct,";
				serializedData += "totalTouches,totalGrabs,totalButtonCollisions,";
				serializedData += "totalButtonTriggers,";
        serializedData += "pair,weightOrder,sceneName";
        serializedData += "\n";
        return serializedData;
    }
    public string SerializeCSV()
    {
        string serializedData = "";


        serializedData += sceneNumber + "," + timeToCompletion + "," + timeGrabbingObj + ",";
				serializedData += timeToFirstInteraction + ",";
        serializedData += correct + "," + totalTouches + "," + totalGrabs + ",";
				serializedData += totalButtonCollisions + "," + totalButtonTriggers + ",";
        serializedData += pair + "," + weightOrder + "," + sceneName;

        serializedData += "\n";

        return serializedData;
    }

    public void AddCSVStatPerGrab(int sceneNumber, float startTime, float endTime, float duration, float weight, float pair, string sceneName)
    {
        CSVStatPerGrab += (sceneNumber + "," + startTime + "," + endTime + "," + duration + "," + weight + "," + pair + "," + sceneName + "\n");
    }
    public string GetCSVStatPerGrab()
    {
        return CSVStatPerGrab;
    }
    public string GetCSVStatPerGrabHeader()
    {
        return CSVStatPerGrabHeader;
    }
		public bool IsFirstInteraction() {
			return !firstInteraction;
		}
		public void HandleFirstInteraction(float timestamp) {
			if (IsFirstInteraction()) {
				firstInteraction = true;
				timeToFirstInteraction = timestamp;
			}
		}

}
