using UnityEngine;
using System.Collections;

public class Rockslide : MonoBehaviour {

	//public AudioClip myAudioClip;
	public AudioSource myAudioSource;
	bool audioHasPlayed;

	// Use this for initialization
	void Start () {
		//myAudioSource.clip = myAudioClip;
		audioHasPlayed = false;
		myAudioSource.volume = 1;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider other) {
		if (other.tag.Equals ("Rockslide")) {
			if (audioHasPlayed == false) {
				Debug.Log ("Rockslide Collider entered, sound should trigger");
				audioHasPlayed = true;
				myAudioSource.Play ();
			}
		}
	}
}
