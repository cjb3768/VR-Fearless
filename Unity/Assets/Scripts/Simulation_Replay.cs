using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Simulation_Replay : MonoBehaviour {

	public struct playerOrientation{
		public float xPos, yPos, zPos;
		public float wRot, xRot, yRot, zRot;
		public int hour, min, sec, msec;
		public int sceneIndex;
	};

	public int currentSceneIndex = 0, previousSceneIndex = -1, currentOrientation = 0;
	public float xPos=0, yPos=0, zPos=0;
	public float wRot=1, xRot=0, yRot=0, zRot=0;
	public bool replayFlag = false;
	public bool logFileRead = false;
	public GameObject player;
	public List <playerOrientation> orientationList = new List<playerOrientation>();
	public String filePath;

	//public int activeSceneIndex;


	// Use this for initialization
	void Start () {

		DontDestroyOnLoad (transform.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		//handle switching in and out of replay mode
		if (Input.GetKeyDown ("o")) {
			replayFlag = true;
			getOrientationsFromFile (filePath, ref orientationList);
			loadOrientation(orientationList[currentOrientation]);
		} 
		else if (Input.GetKeyDown ("p")) {
			//currently not supported; will need to do some finagling in positiontracker to handle this.
		} 
		else if (Input.GetKeyDown ("l")) {
			if (replayFlag){
				if(currentOrientation < orientationList.Count - 1){
					//progress to next orientation
					currentOrientation++;
					loadOrientation(orientationList[currentOrientation]);
				}
			}
		} 
		else if (Input.GetKeyDown ("j")) {
			if (replayFlag){
				if(currentOrientation > 0){
					//reload previous orientation
					currentOrientation--;
					loadOrientation(orientationList[currentOrientation]);
				}
			}
		}



		//if (replayFlag) {
		//	loadOrientation (orientationList[9]);
		//}
		/**
			if (currentSceneIndex != previousSceneIndex){
				Application.LoadLevel (currentSceneIndex);
				//right now, I'm not regularly updating the currentSceneIndex; for now, we will set previousSceneIndex equal to our currentSceneIndex
				previousSceneIndex = currentSceneIndex;
			}
			//make sure a scene is not loading
			if (!(Application.isLoadingLevel)){
				if (Application.loadedLevel == 0 || Application.loadedLevel == 2){
					//do nothing for now
				}
				else{
					//find player and put them in this position
					//if (currentSceneIndex != previousSceneIndex){
						player = GameObject.Find ("OVRPlayerController");
					//}
					player.transform.position = new Vector3(xPos, yPos, zPos);
					player.transform.rotation = new Quaternion(xRot, yRot, zRot, wRot);
				}
			}
		}
		*/
	}


	//functions to add
	//file readin
	//calling a specific scene

	public void loadOrientation(playerOrientation orientation){
		if (orientation.sceneIndex != previousSceneIndex){
			Application.LoadLevel (orientation.sceneIndex);
			//right now, I'm not regularly updating the currentSceneIndex; for now, we will set previousSceneIndex equal to our currentSceneIndex
			previousSceneIndex = orientation.sceneIndex;
		}
		//make sure a scene is not loading
		if (!(Application.isLoadingLevel)){
			if (Application.loadedLevel == 0 || Application.loadedLevel == 2){
				//do nothing for now
			}
			else{
				//find player and put them in this position
				//if (currentSceneIndex != previousSceneIndex){
				player = GameObject.Find ("OVRPlayerController");
				//}
				player.transform.position = new Vector3(orientation.xPos, orientation.yPos, orientation.zPos);
				player.transform.rotation = new Quaternion(orientation.xRot, orientation.yRot, orientation.zRot, orientation.wRot);
				//player.transform.position = new Vector3(xPos, yPos, zPos);
				//player.transform.rotation = new Quaternion(xRot, yRot, zRot, wRot);
			}
		}
	}

	public void parseOrientationLine(string orientationLog, ref playerOrientation orientation){
		//split orientationLog by commas
		string[] itemsFromLog = orientationLog.Split(',');
		//get time data from log
		orientation.hour = Int32.Parse(itemsFromLog [0]);
		orientation.min = Int32.Parse(itemsFromLog [1]);
		orientation.sec = Int32.Parse(itemsFromLog [2]);
		orientation.msec = Int32.Parse(itemsFromLog [3]);
		//get sceneIndex from log
		orientation.sceneIndex = Int32.Parse(itemsFromLog [4]);
		//get position data from log
		orientation.xPos = float.Parse(itemsFromLog [5]);
		orientation.yPos = float.Parse(itemsFromLog [6]);
		orientation.zPos = float.Parse(itemsFromLog [7]);
		//get rotation data from log
		orientation.wRot = float.Parse(itemsFromLog [8]);
		orientation.xRot = float.Parse(itemsFromLog [9]);
		orientation.yRot = float.Parse(itemsFromLog [10]);
		orientation.zRot = float.Parse(itemsFromLog [11]);
	}

	public void getOrientationsFromFile(String filePath, ref List<playerOrientation> orientations){
		//note: function tacitly assumes we are using a file that has only positionTracker data. Will need to update later for different kinds of logs.
		System.IO.StreamReader reader = new System.IO.StreamReader(filePath);
		//get the first line of the file.
		string line = reader.ReadLine();
		Debug.Log (line);
		Debug.Log ("current number of orientations = " + orientations.Count);
		//int i = orientations.Count;

		while ((line = reader.ReadLine ()) != null) {
			//if not a blank line
			if (line != ""){
				//remove all parens and whitespace from line, convert colons to commas
				line = line.Replace ("(", "");
				line = line.Replace (")", "");
				line = line.Replace (" ", "");
				line = line.Replace (":", ",");

				//create new playerOrientation filled with data from line
				playerOrientation newOrient = new playerOrientation();
				parseOrientationLine (line, ref newOrient);
				orientations.Add (newOrient);
				//i++;

			}
		}
		Debug.Log ("final number of orientations = " + orientations.Count);
	}
}
