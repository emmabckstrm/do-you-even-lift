using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class SceneStatistics 
{
    public string sceneName;
    public int sceneNumber;
    public float timeToCompletion; // Total time to complete task
    public float timeGrabbingObj; // total time grabbing an object
    public float timeGrabbingRight; // total time grabbing any object with right controller
    public float timeGrabbingLeft;
    public bool correct; // if the solution was correct
    public int totalTouches; // total number of touches to objects
    public int totalGrabs; // total number of times grabbing any object
    public int totalTouchesRight;
    public int totalTouchesLeft;
    public int totalGrabsRight;
    public int totalGrabsLeft;
    public int totalForceReleases;

    private string CSVStatPerGrabHeader = "sceneName, sceneNumber, startTime, endTime, duration, weight, hand, forceRelease\n";
    private string CSVStatPerGrab = "";

    public string SerializeJson() {
        string serializedData = "";
        serializedData += "{";

        serializedData += ("\"sceneNumber\":" + sceneNumber);
        serializedData += (",\"timeToCompletion\":" + timeToCompletion);
        serializedData += (",\"timeGrabbingObj\":"+timeGrabbingObj);
        serializedData += (",\"timeGrabbingRight\":" + timeGrabbingRight);
        serializedData += (",\"timeGrabbingLeft\":" + timeGrabbingLeft);
        serializedData += (",\"correct\":\"" + correct +"\"");
        serializedData += (",\"totalTouches\":" + totalTouches);
        serializedData += (",\"totalGrabs\":" + totalGrabs);
        serializedData += (",\"totalTouchesRight\":" + totalTouchesRight);
        serializedData += (",\"totalTouchesLeft\":" + totalTouchesLeft);
        serializedData += (",\"totalGrabsRight\":" + totalGrabsRight);
        serializedData += (",\"totalGrabsLeft\":" + totalGrabsLeft);
        serializedData += (",\"totalForceReleases\":" + totalForceReleases);
        serializedData += (",\"sceneName\":\"" + sceneName + "\"");
        serializedData += "}";

        return serializedData;
    }
    public string SerializeCSVHeader()
    {
        string serializedData = "";
        serializedData += "sceneNumber,timeToCompletion,timeGrabbingObj,timeGrabbingRight,timeGrabbingLeft,correct,";
        serializedData += "totalTouches,totalGrabs,totalTouchesRight,";
        serializedData += "totalTouchesLeft,totalGrabsRight,totalGrabsLeft,totalForceReleases";
        serializedData += ",sceneName";
        serializedData += "\n";
        return serializedData;
    }
    public string SerializeCSV()
    {
        string serializedData = "";

        
        serializedData += sceneNumber + "," + timeToCompletion + "," + timeGrabbingObj + "," + timeGrabbingRight + ",";
        serializedData += timeGrabbingLeft + "," + correct + "," + totalTouches + "," + totalGrabs + ",";
        serializedData += totalTouchesRight + "," + totalTouchesLeft + "," + totalGrabsRight + ",";
        serializedData += totalGrabsLeft + "," + totalForceReleases;
        serializedData += "," + sceneName;

        serializedData += "\n";

        return serializedData;
    }

    public void AddCSVStatPerGrab(string data)
    {
        CSVStatPerGrab += data + "\n";
    }
    public string GetCSVStatPerGrab()
    {
        return CSVStatPerGrab;
    }
    public string GetCSVStatPerGrabHeader()
    {
        return CSVStatPerGrabHeader;
    }

}