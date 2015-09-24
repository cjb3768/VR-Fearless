using UnityEngine;
using System.Collections;

public class EventAudio : MonoBehaviour {

	public AudioClip sourceAudioClip;
	//Vector3 

	// Use this for initialization
	void Start () {
		GetComponent<AudioSource>().volume = 1;
	}
	
	// Update is called once per frame
	void Update () {

	}
}
