using UnityEngine;
using System.Collections;

public class stone_grind : MonoBehaviour {

	public AudioClip myAudioClip;

	// Use this for initialization
	void Start () {
		audio.clip = myAudioClip;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Wall") {
			audio.Play ();
		}
	}
}
