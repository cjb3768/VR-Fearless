using UnityEngine;
using System.Collections;

public class Rockslide : MonoBehaviour {

	public AudioClip myAudioClip;
	bool hasHappened;

	// Use this for initialization
	void Start () {
		audio.clip = myAudioClip;
		hasHappened = false;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Rockslide") {
			if(hasHappened == false) {
				hasHappened = true;
				audio.Play();
			}
		}
	}
}
