using UnityEngine;
using System.Collections;


public class AmbientAudio : MonoBehaviour {

	// Allows for call to ambient audio, on customizable randomized timer\

	public AudioClip[] sourceAudioClips = new AudioClip[5];

	public float minScheduleWait = 5f;
	public float maxScheduleWait = 30f;
	private int currentClip = -1;
	private double waitTimeUntilEvent;


	/**
	 * Calculate and return index of next clip to play
	 * Guaranteed to not be the same clip twice in a row
	 */
	int nextClip (int previousClip){
		int i = previousClip;
		Debug.Log ("previousClip = " + i + "\n");
		while (i == previousClip) {
			i = Random.Range (0, sourceAudioClips.Length);
		}
		Debug.Log ("i = " + i + "\n");
		return i;
	}

	/**
	 * Calculate and return wait time until next track plays 
	 */
	float waitTime () {
		float f = Random.Range (minScheduleWait, maxScheduleWait);
		return f;
	}

	// Use this for initialization
	void Start () {
		currentClip = nextClip(sourceAudioClips.Length);
		waitTimeUntilEvent = AudioSettings.dspTime + 5.0f;
		GetComponent<AudioSource>().volume = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
		double currentTime = AudioSettings.dspTime;
		if (currentTime + 1.0f > waitTimeUntilEvent) {
			GetComponent<AudioSource>().clip = sourceAudioClips [currentClip];
			GetComponent<AudioSource>().PlayScheduled (waitTimeUntilEvent);
			//determine next track
			currentClip = nextClip (currentClip);
			//determine time until next clip plays
			waitTimeUntilEvent += waitTime ();
		}

	}
}
