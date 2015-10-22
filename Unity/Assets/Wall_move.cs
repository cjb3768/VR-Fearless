using UnityEngine;
using System.Collections;

public class Wall_move : MonoBehaviour {

	float maxMoveSteps = 1400f;
	float minMoveSteps = 0f;
	float stepsMoved = 0f;
	float adjustmentRate = 0.005f;
	Vector3 initpos;

	// Use this for initialization
	void Start () {
		initpos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z); 
		stepsMoved = setStepsMoved ();
		this.transform.position = setStartPosition (initpos);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate () {
		/**if (Input.GetAxis ("Fire1") < 0.0f) {
			stepsMoved ++;
			if (this.tag == "MoveMore") {
				if (stepsMoved < 14){
				//if (this.transform.position.x < (initpos.x + 7)) {
					//Vector3 pos = this.transform.position;
					//pos.x += 0.05f;
					//this.transform.position = pos;
					Vector3 pos = initpos;
					pos.x = pos.x + (0.05f * stepsMoved);
					this.transform.position = pos;
				}
			}
			if(this.tag == "MoveLess") {
				if (stepsMoved < 14){
				//if(this.transform.position.x >= (initpos.x - 7)) {
					//Vector3 pos = this.transform.position;
					//pos.x -= 0.05f;
					//this.transform.position = pos;
					Vector3 pos = initpos;
					pos.x = pos.x - (0.05f * stepsMoved);
					this.transform.position = pos;
				}
			}
		}
		if (Input.GetAxis ("Fire1") > 0.0f) {
			stepsMoved --;
			if (this.tag == "MoveMore") {
				if (stepsMoved > 0){
				//if (this.transform.position.x > initpos.x) {
					//Vector3 pos = this.transform.position;
					//pos.x -= 0.05f;
					//this.transform.position = pos;
					Vector3 pos = initpos;
					pos.x = pos.x - (0.05f * stepsMoved);
					this.transform.position = pos;
				}
			}
			if(this.tag == "MoveLess") {
				if (stepsMoved > 0){
				//if(this.transform.position.x < initpos.x) {
					//Vector3 pos = this.transform.position;
					//pos.x += 0.05f;
					//this.transform.position = pos;
					Vector3 pos = initpos;
					pos.x = pos.x + (0.05f * stepsMoved);
					this.transform.position = pos;
				}
			}
		}*/

		if (Input.GetAxis ("Fire1") < 0.0f) {
			if (stepsMoved < maxMoveSteps){
				stepsMoved ++;
				if (this.tag == "MoveMore") {
					//if (stepsMoved < 14){
						//if (this.transform.position.x < (initpos.x + 7)) {
						//Vector3 pos = this.transform.position;
						//pos.x += 0.05f;
						//this.transform.position = pos;
					Vector3 pos = initpos;
						//pos.x = pos.x + (0.05f * stepsMoved);
					pos.x = pos.x + (adjustmentRate * stepsMoved);
					this.transform.position = pos;
					//}
				}
				if(this.tag == "MoveLess") {
					//if (stepsMoved < 14){
						//if(this.transform.position.x >= (initpos.x - 7)) {
						//Vector3 pos = this.transform.position;
						//pos.x -= 0.05f;
						//this.transform.position = pos;
					Vector3 pos = initpos;
						//pos.x = pos.x + (0.05f * stepsMoved);
					pos.x = pos.x - (adjustmentRate * stepsMoved);
					this.transform.position = pos;
					//}
				}
				Debug.Log ("Current value for stepsMoved: " + stepsMoved + " Current wall x.pos: " + this.transform.position.x + " Initial wall x.pos: " + initpos.x);
			}

		}
		if (Input.GetAxis ("Fire1") > 0.0f) {
			if(stepsMoved > minMoveSteps){
				stepsMoved --;
				if (this.tag == "MoveMore") {
					//if (stepsMoved > 0) {
						//if (this.transform.position.x > initpos.x) {
						//Vector3 pos = this.transform.position;
						//pos.x -= 0.05f;
						//this.transform.position = pos;
					Vector3 pos = initpos;
						//pos.x = pos.x - (0.05f * stepsMoved);
						//pos.x = pos.x + (0.05f * stepsMoved);
					pos.x = pos.x + (adjustmentRate * stepsMoved);
					this.transform.position = pos;
					//}
				}
				if (this.tag == "MoveLess") {
					//if (stepsMoved > 0) {
						//if(this.transform.position.x < initpos.x) {
						//Vector3 pos = this.transform.position;
						//pos.x += 0.05f;
						//this.transform.position = pos;
					Vector3 pos = initpos;
						//pos.x = pos.x + (0.05f * stepsMoved);
						//pos.x = pos.x - (0.05f * stepsMoved);
					pos.x = pos.x - (adjustmentRate * stepsMoved);
					this.transform.position = pos;
					//}
				}
			}
			Debug.Log ("Current value for stepsMoved: " + stepsMoved + " Current wall x.pos: " + this.transform.position.x + " Initial wall x.pos: " + initpos.x);
		}
	}

	Vector3 setStartPosition(Vector3 position){
		Vector3 startPosition = position;
		if (this.tag == "MoveMore") {
			startPosition.x = startPosition.x + (adjustmentRate * stepsMoved);
		}
		if (this.tag == "MoveLess") {
			startPosition.x = startPosition.x - (adjustmentRate * stepsMoved);
		}
		return startPosition;
	}

	float setStepsMoved(){
		return (maxMoveSteps + minMoveSteps) / 2;
	}
}
