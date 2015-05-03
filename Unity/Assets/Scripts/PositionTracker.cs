﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
//using System.Drawing.Imaging;

public class PositionTracker : MonoBehaviour {

	public struct playerPosition{
		public float xPos, yPos, zPos;
		public float wRot, xRot, yRot, zRot;
		public string screenshotName;
	};
	
	public GameObject player;
	private bool inModule = false;
	private int numFramesSinceScreenCap = 0;
	private int framesBetweenScreenCaps;
	private string currentScreenShotName = "";
	public DateTime currentTime;

	public RuntimePlatform currentPlatform;

	public List<playerPosition> positions;

	// Use this for initialization
	void Start () {
		currentPlatform = Application.platform;
		positions = new List<playerPosition> ();
		DontDestroyOnLoad (transform.gameObject);
		//InvokeRepeating ("captureScreenShot", 0, 1);
		InvokeRepeating ("getPosition", 0, 1);
	}
	
	// Update is called once per frame
	void Update () {

		/*
		addPositionToList(Application.loadedLevel);
		int i = positions.Count;

		if (Application.loadedLevel == 1 || Application.loadedLevel == 3 || Application.loadedLevel == 4) {
			Debug.Log ("In a module");
		} 
		else {
			Debug.Log ("In a menu");
		}

		Debug.Log ("Player position: (" + positions[i].xPos + ", " + positions[i].yPos + ", " + positions[i].zPos + ")\t" 
		           + "Player rotation: (" + positions[i].wRot + ", " + positions[i].xRot + ", " + positions[i].yRot + ", " + positions[i].zRot + ")\n");
		*/
	}

	void getPosition(){
		addPositionToList(Application.loadedLevel);
		int i = positions.Count;
		
		if (Application.loadedLevel == 1 || Application.loadedLevel == 3 || Application.loadedLevel == 4) {
			Debug.Log ("In a module");
		} 
		else {
			Debug.Log ("In a menu");
		}
		
		Debug.Log ("Player position: (" + positions[i].xPos + ", " + positions[i].yPos + ", " + positions[i].zPos + ")\t" 
		           + "Player rotation: (" + positions[i].wRot + ", " + positions[i].xRot + ", " + positions[i].yRot + ", " + positions[i].zRot + ")\n");
	}

	void addPositionToList(int sceneIndex){
		playerPosition p = new playerPosition ();
		if (sceneIndex == 1 || sceneIndex == 3 || sceneIndex == 4) {
			//in a phobia module
			if (inModule == false){
				player = GameObject.Find ("OVRPlayerController");
				inModule = true;
			}

			p.xPos = player.transform.position.x;
			p.yPos = player.transform.position.y;
			p.zPos = player.transform.position.z;
			p.wRot = player.transform.rotation.w;
			p.xRot = player.transform.rotation.x;
			p.yRot = player.transform.rotation.y;
			p.zRot = player.transform.rotation.z;

			positions.Add (p);
		}
		else{
			//in a menu
			inModule = false;

			p.xPos = 0;
			p.yPos = 0;
			p.zPos = 0;
			p.wRot = 0;
			p.xRot = 0;
			p.yRot = 0;
			p.zRot = 0;

			positions.Add (p);
		}

	}
	void OnApplicationQuit() {
		//Debug.Log ("Num positions recorded = " + positions.Count);
		//for (int i = 0; i < positions.Count; i++) {
		//	Debug.Log ("Player position: (" + positions[i].xPos + ", " + positions[i].yPos + ", " + positions[i].zPos + ")\t" 
		//	           + "Player rotation: (" + positions[i].wRot + ", " + positions[i].xRot + ", " + positions[i].yRot + ", " + positions[i].zRot + ")\n");
		//}
	}

	//Use Unity's inbuilt screenshot taking application
	//Note that at time of writing the function causes at best a "jerk" every time it is called, and unplayable lag at the worst.
	//It depends on the machine specs, it seems.
	void unityScreenShot() {
		string fileName = "";

		currentTime = DateTime.Now;
		currentScreenShotName = currentTime.Month + "_" + currentTime.Day + "_"
			+ currentTime.Year + "_" + currentTime.Hour + "_" 
				+ currentTime.Minute + "_" + currentTime.Second;

		if (currentPlatform == RuntimePlatform.WindowsEditor || currentPlatform == RuntimePlatform.WindowsPlayer) {
			//Debug.Log ("Running on Windows");
			fileName = "Logs/Screenshots/";
			fileName += currentScreenShotName + ".jpg";
			
			Application.CaptureScreenshot(fileName);
		} else if (currentPlatform == RuntimePlatform.OSXEditor || currentPlatform == RuntimePlatform.OSXPlayer) {
			//Debug.Log ("Running on OSX");
			//fileName = "Logs/Screenshots/";

			//It seems CaptureScreenshot and mac doesn't play nicely together if you specify anything more than a filename

			//Application.CaptureScreenshot(currentScreenShotName+".png");
			Application.CaptureScreenshot("test.png");
		}


	}

	//Note that this method must be attached to a texture to work. Will not work in current implementation.
	void captureScreenShot(){
		Texture2D tex = new Texture2D (Screen.width, Screen.height, TextureFormat.RGB24,false);
		tex.ReadPixels (new Rect (0, 0, Screen.width, Screen.height), 0, 0);
		tex.Apply ();
		 
		currentTime = DateTime.Now;
		currentScreenShotName = currentTime.Month + "_" + currentTime.Day + "_"
			+ currentTime.Year + "_" + currentTime.Hour + "_" 
			+ currentTime.Minute + "_" + currentTime.Second + ".jpg";

		byte[] pic = tex.EncodeToJPG();
		Destroy (tex);
		File.WriteAllBytes (Application.dataPath+"/ ../"+currentScreenShotName,pic);
	}
}