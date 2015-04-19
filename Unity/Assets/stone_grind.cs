using UnityEngine;
using System.Collections;

public class stone_grind : MonoBehaviour {

	public AudioSource audioSource;
	public float volume;

	// Use this for initialization
	void Start () {
		audioSource.volume = volume;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Wall") {
			audioSource.Play ();
		}
	}
}
