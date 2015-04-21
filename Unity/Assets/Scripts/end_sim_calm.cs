using UnityEngine;
using System.Collections;

public class end_sim_calm : MonoBehaviour {
	bool nearButton = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//if (nearButton) {
			if (Input.GetKeyDown (KeyCode.K)) {
				Application.LoadLevel ("Second_Menu");
			}
		//}
	}

	//void OnTriggerEnter(Collider other) {
	//	nearButton = true;
	//}
}
