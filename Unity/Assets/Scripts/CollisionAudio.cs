using UnityEngine;
using System.Collections;

public class CollisionAudio : MonoBehaviour {

	public AudioSource audio1;
	public AudioSource audio2;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag.Equals ("Rockslide")) {
			Debug.Log ("Entered Rockslide");
			audio1.Play ();
		}
		else if (other.tag.Equals ("Wall")){
			Debug.Log ("Entered Rockslide");
			audio2.Play ();                           
		}
		else {
			Debug.Log("Entered other collider");
		}
	}
}
