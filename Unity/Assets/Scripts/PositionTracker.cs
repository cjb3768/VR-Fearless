using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
//using System.Drawing.Imaging;

public class PositionTracker : MonoBehaviour {

	public struct playerPosition{
		public float xPos, yPos, zPos;
		public float wRot, xRot, yRot, zRot;
		public int hour, minute, second, millisecond;
		public int currentScene;
		public string screenshotName;
	};
	
	public GameObject player;
	private bool inModule = false;
	private int numFramesSinceScreenCap = 0;
	private int framesBetweenScreenCaps;
	public int lastScene = -1, currentScene = 0;
	private string currentScreenShotName = "";
	private string filePath = "";

	public System.IO.StreamWriter outputFile;

	public DateTime currentTime;

	public RuntimePlatform currentPlatform;

	public List<playerPosition> positions;

	// Use this for initialization
	void Start () {
		currentPlatform = Application.platform;

		if (currentPlatform == RuntimePlatform.WindowsEditor || currentPlatform == RuntimePlatform.WindowsPlayer) {
			Debug.Log ("Running on Windows");
			filePath = "Logs\\";
		} else if (currentPlatform == RuntimePlatform.OSXEditor || currentPlatform == RuntimePlatform.OSXPlayer) {
			Debug.Log ("Running on OSX");
			filePath = "Logs/";
		}

		positions = new List<playerPosition> ();
		Debug.Log ("Beginning to record position");
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
		lastScene = currentScene;
		currentScene = Application.loadedLevel;
		addPositionToList(lastScene,currentScene);
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

	void addPositionToList(int prevScene, int sceneIndex){
		playerPosition p = new playerPosition ();
		p.currentScene = sceneIndex;

		if (prevScene != sceneIndex) {
			Debug.Log ("Scene changed");
			inModule = false;
		}

		if (sceneIndex == 1 || sceneIndex == 3 || sceneIndex == 4) {
			//in a phobia module
			if (inModule == false){
				player = GameObject.Find ("OVRPlayerController");
				inModule = true;
			}

			//setPositionTime (p);

			currentTime = DateTime.Now;
			p.hour = currentTime.Hour;
			p.minute = currentTime.Minute;
			p.second = currentTime.Second;
			p.millisecond = currentTime.Millisecond;

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

			//setPositionTime (p);

			currentTime = DateTime.Now;
			p.hour = currentTime.Hour;
			p.minute = currentTime.Minute;
			p.second = currentTime.Second;
			p.millisecond = currentTime.Millisecond;

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

	void setPositionTime(playerPosition p){
		currentTime = DateTime.Now;
		p.hour = currentTime.Hour;
		p.minute = currentTime.Minute;
		p.second = currentTime.Second;
		p.millisecond = currentTime.Millisecond;
	}

	void OnApplicationQuit() {
		//Debug.Log ("Num positions recorded = " + positions.Count);
		//for (int i = 0; i < positions.Count; i++) {
		//	Debug.Log ("Player position: (" + positions[i].xPos + ", " + positions[i].yPos + ", " + positions[i].zPos + ")\t" 
		//	           + "Player rotation: (" + positions[i].wRot + ", " + positions[i].xRot + ", " + positions[i].yRot + ", " + positions[i].zRot + ")\n");
		//}
		writeLogToFile ();
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

	public void writeLogToFile(){
		Debug.Log ("Writing log to file");
		long hrs;
		long min;
		long sec;
		string lineToPrint = "";
		
		DateTime rightNow = DateTime.Now;
		filePath += "log-" + rightNow.Month + "-" + rightNow.Day + "-"
			+ rightNow.Year + "-" + rightNow.Hour + "-" 
				+ rightNow.Minute + "-" + rightNow.Second + ".txt";
		
		outputFile = new System.IO.StreamWriter(@filePath);
		
		using (outputFile){
			outputFile.WriteLine ("#Timestamp-Hour:Minute:Second:Millisecond, Current Scene Index, (xPos, yPos, zPos), (wRot, xRot, yRot, zRot)");
			for (int i=0; i < positions.Count; i++){

				PositionTracker.playerPosition currPos=positions[i];

				//timestamp
				lineToPrint = "" + currPos.hour + ":" + currPos.minute + ":" + currPos.second + ":" + currPos.millisecond;

				//scene index
				lineToPrint += ", " + currPos.currentScene;

				//position and rotation data
				lineToPrint += ", (" + currPos.xPos + ", " + currPos.yPos + ", " + currPos.zPos + "), ("
					+ currPos.wRot + ", " + currPos.xRot + ", " + currPos.yRot + ", " + currPos.zRot + ")";

				lineToPrint += "\n";
				
				outputFile.WriteLine(lineToPrint);
			}
			/**
			if(packets.Count > 0){
				outputFile.WriteLine("#Timestamp, Heart Rate, Heart Rate Variability, Heart Rate Confidence, Current Scene, (xPos, yPos, zPos), (wRot, xRot, yRot, zRot)");
				for(int i=0;i<packets.Count;i++){
					SummaryPacket sp = packets[i];
					
					
					hrs = (sp.getTimestampMilliseconds () / 3600000);
					min = ((sp.getTimestampMilliseconds () % 3600000) / 60000);
					sec = (((sp.getTimestampMilliseconds () % 3600000) % 60000) / 1000);
					string lineToPrint = sp.getTimestampMonth() + "/" 
						+ sp.getTimestampDay() + "/"
							+ sp.getTimestampYear() + ","
							+ hrs + ":" + min + ":" + sec + ","
							+ sp.getHeartRate() + ","
							+ sp.getHeartRateVariability() + ","
							+ sp.getHeartRateConfidence() + ","
							+ sp.getCurrentScene();
					
					try{
						PositionTracker.playerPosition currPos=pp[i];
						lineToPrint+=", ("+currPos.xPos+", "+currPos.yPos+", "+currPos.zPos+"), ("
							+currPos.wRot+", "+currPos.xRot+", "+currPos.yRot+", "+currPos.zRot+")";
					}
					catch{
					}
					
					
					lineToPrint += "\n";
					
					outputFile.WriteLine(lineToPrint);
				}
			}
			*/
		}
	}
}
