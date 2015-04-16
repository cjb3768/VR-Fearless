using UnityEngine;
using System.Collections;

public class Wall_move : MonoBehaviour {
	float motion;

	// Use this for initialization
	void Start () {
		Vector3 initpos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z); 
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate () {
		if (Input.GetAxis ("Fire1") < 0.0f) {
			if (this.tag == "MoveMore") {
				if (this.transform.position.x <= 150.0f) {
					Vector3 pos = this.transform.position;
					pos.x += 0.05f;
					this.transform.position = pos;
				}
			}
			if(this.tag == "MoveLess") {
				if(this.transform.position.x >= 163.0f) {
					Vector3 pos = this.transform.position;
					pos.x -= 0.05f;
					this.transform.position = pos;
				}
			}
		}
		if (Input.GetAxis ("Fire1") > 0.0f) {
			if (this.tag == "MoveMore") {
				if (this.transform.position.x >= 157.0f) {
					Vector3 pos = this.transform.position;
					pos.x -= 0.05f;
					this.transform.position = pos;
				}
			}
			if(this.tag == "MoveLess") {
				if(this.transform.position.x <= 170.0f) {
					Vector3 pos = this.transform.position;
					pos.x += 0.05f;
					this.transform.position = pos;
				}
			}
		}
	}
}
