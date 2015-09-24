using UnityEngine;
using System.Collections;

public class LightIntensity : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Light>().intensity = 0.2f;
	}
	
	// Update is called once per frame
	void Update () {
		float intensDir = Input.GetAxis ("Fire1");
		if(intensDir != 0) {
			GetComponent<Light>().intensity += intensDir * 0.1f;
		}
	}
}
