using UnityEngine;
using System.Collections;

public class end_sim_button : MonoBehaviour {
	RaycastHit hit;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Input.GetButton
		Ray myray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(myray,out hit, 5.0f)) {
			Application.LoadLevel (0);
		}
	}

	void OnMouseUp () {

	}
}
