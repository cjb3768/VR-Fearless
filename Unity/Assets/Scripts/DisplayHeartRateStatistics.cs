using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisplayHeartRateStatistics : MonoBehaviour {
	
	//Base script for showing heart rate info; need to connect biometrics to this
	
	public Text heartRateText;
	public Text heartRateDeviationText;
	public string heartRateInfo = "No Information Yet";
	public string heartRateDeviationInfo = "+ 0 bpm";
	public int updateDelay = 10;
	public float baselineHeartRate = 90f;
	private int updateCounter = 0;
	
	string updateHeartRateInfo (float currentHeartRate){
		//will ultimately show accurate heart rate information; for now, just shows whatever double is passed in
		return "Heart Rate: " + currentHeartRate + " bpm";
	}
	
	string updateHeartRateDeviationInfo (float currentHeartRate, float baselineHeartRate){
		float deviation = baselineHeartRate - currentHeartRate;
		string deviationInfo = "Delta Heart Rate: ";
		
		if (deviation < 0) {
			deviationInfo += "-" + deviation + " bpm";
		} 
		else if (deviation > 0) {
			deviationInfo += "+" + deviation + " bpm";
		} 
		else {
			deviationInfo += deviation + " bpm";
		}

		return deviationInfo;
	}
	
	// Use this for initialization
	void Start () {
		heartRateText.text = heartRateInfo;
		heartRateDeviationText.text = heartRateDeviationInfo;
	}
	
	// Update is called once per frame
	void Update () {
		//here is where we will call other scripts to read in real heart rate information; for now, we are just going to do random numbers between 80 and 100 on a delay
		updateCounter ++;
		if (updateCounter == updateDelay) {
			float currentHeartRate = Random.Range (80, 100);
			heartRateInfo = updateHeartRateInfo (currentHeartRate);
			heartRateText.text = heartRateInfo;
			heartRateDeviationInfo = updateHeartRateDeviationInfo(currentHeartRate, baselineHeartRate);
			heartRateDeviationText.text = heartRateDeviationInfo;
			updateCounter = 0;
		}
	}
}