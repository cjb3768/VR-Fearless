﻿using UnityEngine;
using System.Collections;

public class end_sim_button : MonoBehaviour {
	bool nearButton = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (nearButton) {
			if (Input.GetMouseButtonDown (0)) {
				Application.LoadLevel (0);
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		nearButton = true;
	}
}