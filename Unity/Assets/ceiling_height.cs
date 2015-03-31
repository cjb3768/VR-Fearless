using UnityEngine;
using System.Collections;

public class ceiling_height : MonoBehaviour {
	float height = 3.5f;
	// Use this for initialization
	void Start () {
		Vector3 initpos = new Vector3(this.transform.position.x, height, this.transform.position.z); 
		this.transform.position = initpos;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetAxis ("Fire1") < 0.0f) {
			if (this.transform.position.y > 2.0f) {
				Vector3 pos = this.transform.position;
				pos.y = pos.y - (0.005f);
				this.transform.position = pos;
			}
		}
		if (Input.GetAxis ("Fire1") > 0.0f) {
			if (this.transform.position.y < 6.0f) {
				Vector3 pos = this.transform.position;
				pos.y = pos.y + (0.005f);
				this.transform.position = pos;
			}
		}
	}
}
