using UnityEngine;
using System.Collections;

public class end_sim_button : MonoBehaviour {
	bool nearButton = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (nearButton) {
			if (Input.GetMouseButtonDown (0) || Input.GetButtonDown("A Button") || Input.GetButtonDown("B Button") || Input.GetButtonDown("X Button") || Input.GetButtonDown("Y Button")) {
				Application.LoadLevel ("Meadow");
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Button") {
			nearButton = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.tag.Equals ("Button"))
		{
			nearButton = false;
		}
	}
}
