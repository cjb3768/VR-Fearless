using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisplayTime : MonoBehaviour {

	public Text currentTimeDisplay;
	public string currentTimeString = "00:00:00";

	string formatCurrentTime (float currentTime){
		int integerTime = (int) currentTime;
		int hrs = integerTime / 3600;
		int mins = integerTime / 60;
		int secs = integerTime % 60;
		string formattedTime = string.Format ("Current time: {0:00}:{1:00}:{2:00}", hrs, mins, secs);
		return formattedTime;
	}

	string updateCurrentTime (){
		float currentTime = Time.realtimeSinceStartup;
		return formatCurrentTime(currentTime);
	}

	// Use this for initialization
	void Start () {
		currentTimeDisplay.text = currentTimeString;
	}
	
	// Update is called once per frame
	void Update () {
		currentTimeString = updateCurrentTime ();
		currentTimeDisplay.text = currentTimeString;
	}
}
