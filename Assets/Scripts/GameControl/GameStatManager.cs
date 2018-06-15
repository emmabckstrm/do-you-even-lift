using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class GameStatManager : MonoBehaviour {

	protected List<GameStatistics> stats;
	protected GameControl gameControl;
	protected string path = "DataLogs/";

	// Use this for initialization
	void Start () {
		gameControl = GetComponent<GameControl>();
		//currentLevelStats = new GameStatistics();
		stats = new List<GameStatistics>();
		stats.Add(new GameStatistics());
	}
	public void ResetStats() {
		stats = new List<GameStatistics>();
	}
	public void NewStat() {
		stats.Add(new GameStatistics());
	}
	public void HandleLevelIndex() {
		int lvlNum = gameControl.GetCurrentLevel();
		stats[lvlNum].sceneNumber = lvlNum;
		stats[lvlNum].sceneName = "Level " + lvlNum;
	}
	public void HandleLevelTime(float start, float end) {
		float duration = end - start;
		stats[gameControl.GetCurrentLevel()].timeToCompletion = duration;
	}
	public void IncreaseButtonCollisions() {
		stats[gameControl.GetCurrentLevel()].totalButtonCollisions += 1;
	}
	public void DecreaseButtonCollisions() {
		stats[gameControl.GetCurrentLevel()].totalButtonCollisions -= 1;
	}
	public void IncreaseButtonTriggers() {
		stats[gameControl.GetCurrentLevel()].totalButtonTriggers += 1;
	}
	public void DecreaseButtonTriggers() {
		stats[gameControl.GetCurrentLevel()].totalButtonTriggers -= 1;
	}
	public void AddTimeGrabbing(float time) {
		stats[gameControl.GetCurrentLevel()].timeGrabbingObj += time;
	}
	public void AddGrab() {
		stats[gameControl.GetCurrentLevel()].totalGrabs += 1;
	}
	public void AddReset() {
		stats[gameControl.GetCurrentLevel()].numberOfResets += 1;
	}
	public void SetCorrect() {
		stats[gameControl.GetCurrentLevel()].correct = true;
	}
	public void SetUsingLiftLimitation(bool t) {
		stats[gameControl.GetCurrentLevel()].usingLiftLimitation = t;
	}
	public void HandleFirstInteraction(float startTime) {
		stats[gameControl.GetCurrentLevel()].HandleFirstInteraction(startTime);
	}
	public void AddCSVStatPerGrab(float startTime, float endTime, float weight, string hand)
	{
			float duration = endTime - startTime;
			string sceneName = "Level " + gameControl.GetCurrentLevel();
			int sceneNumber = gameControl.GetCurrentLevel();
			stats[gameControl.GetCurrentLevel()].AddCSVStatPerGrab(sceneNumber, startTime, endTime, duration, weight, -1, sceneName);
	}

	// Serialize data to json format
	public string SerializeDataJson()
	{
			string serializedData = "[";

			for (int i = 0; i < stats.Count; i++)
			{
					GameStatistics stat = stats[i];
					serializedData += stat.SerializeJson();
					if (i < stats.Count - 1)
					{
							serializedData += ",";
					}
					// TODO: Fix so that it dynamically loops through scenneStat properties
					/*
					PropertyInfo[] properties = typeof(statistics).GetProperties();
					Debug.Log("hello " + properties + " length " + properties.Count);
					foreach (PropertyInfo property in properties)
					{
							Debug.Log("Name: " + property.Name + " Value: " + property.GetValue(stat, null));
					}*/
			}
			serializedData += "]";
			return serializedData;
	}
	public string SerializeDataCSV()
	{
			string serializedData = stats[0].SerializeCSVHeader();
			for (int i = 0; i < stats.Count; i++)
			{
					GameStatistics stat = stats[i];
					serializedData += stat.SerializeCSV();
			}
			return serializedData;
	}
	public string SerializeDataPerGrabCSV()
	{
			string serializedData = "";
			serializedData += stats[0].GetCSVStatPerGrabHeader();
			for (int i = 0; i < stats.Count; i++)
			{
					GameStatistics stat = stats[i];
					serializedData += stat.GetCSVStatPerGrab();
			}
			return serializedData;
	}

	public string SerializeData()
	{
			string serializedData = SerializeDataJson();
			serializedData += "\n\n\n";
			serializedData += SerializeDataCSV();
			serializedData += "\n\n\n";
			serializedData += SerializeDataPerGrabCSV();

			return serializedData;
	}
	// writes all collected data to files
	public void WriteDataToFile()
	{
			// create folder with this name DateTime.Now.ToString("yyyy-MM-dd HH:mm")
			string currentPath = path + System.DateTime.Now.ToString("yyyy-MM-dd HHmm");
			currentPath += "/";

			Directory.CreateDirectory(Path.GetDirectoryName(currentPath));
			// writes to json
			WriteStringToFile(currentPath + "log.json", SerializeDataJson());
			// write to one csv
			WriteStringToFile(currentPath + "log.csv", SerializeDataCSV());
			// write to another csv
			WriteStringToFile(currentPath + "logPerGrab.csv", SerializeDataPerGrabCSV());
	}

	// Writes to file
	public void WriteStringToFile(string p, string str)
	{

			using (FileStream fs = new FileStream(p, FileMode.OpenOrCreate, FileAccess.ReadWrite))
			{
					StreamWriter sw = new StreamWriter(fs);
					sw.Write(str);
					sw.Flush();
			}
	}
}
