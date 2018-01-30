using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class SceneStatistics
{
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
}