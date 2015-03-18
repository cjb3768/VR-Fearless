using UnityEngine;
using System.Collections;

public class AmbientAudio : MonoBehaviour {

	// Allows for call to ambient audio, on customizable randomized timer

	public AudioClip sourceAudioClip;
	public int minDelay = 1;
	public int maxDelay = 10;
	public int delayScalar = 44100;
	public float audioDelay = 10.0f;
	Random rand;

	//calculate delay 

	// Use this for initialization
	void Start () {
		audio.volume = 1;
	}
	
	// Update is called once per frame
	void Update () {
		//audioDelay = (float) minDelay;
		audio.clip = sourceAudioClip;
		audio.PlayDelayed (audioDelay);
	}
}
