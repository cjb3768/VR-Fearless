using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Simulation_Replay : MonoBehaviour {

	public struct playerOrientation{
		public float xPos, yPos, zPos;
		public float wRot, xRot, yRot, zRot;
		public int hour, min, sec, msec;
		public int sceneIndex;
	};

	public int currentSceneIndex = 0, previousSceneIndex = -1;
	public float xPos=0, yPos=0, zPos=0;
	public float wRot=1, xRot=0, yRot=0, zRot=0;
	public bool replayFlag = false;
	public bool logFileRead = false;
	public GameObject player;
	public List <playerOrientation> orientationList;

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
		} 
		else if (Input.GetKeyDown ("p")) {
			//currently not supported; will need to do some finagling in positiontracker to handle this.
		}

		if (replayFlag) {
			if (currentSceneIndex != previousSceneIndex){
				Application.LoadLevel (currentSceneIndex);
				//right now, I'm not regularly updating the currentSceneIndex; for now, we will set previousSceneIndex equal to our currentSceneIndex
				previousSceneIndex = currentSceneIndex;
			}
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
	}
	//functions to add
	//file readin
	//calling a specific scene
}
