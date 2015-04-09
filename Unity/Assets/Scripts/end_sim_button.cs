using UnityEngine;
using System.Collections;

public class end_sim_button : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

	}

	void OnTriggerEnter(Collider other) {
		Application.LoadLevel (0);
	}
}
