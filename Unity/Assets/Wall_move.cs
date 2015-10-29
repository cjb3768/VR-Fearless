using UnityEngine;
using System.Collections;

public class Wall_move : MonoBehaviour {
	/**
	float maxMoveSteps = 1400f;
	float minMoveSteps = 0f;
	float stepsMoved = 0f;
	float adjustmentRate = 0.005f;
	*/
	float maxMoveSteps;
	float minMoveSteps;
	float stepsMoved;
	float adjustmentRate;
	Vector3 initpos;
	Claustrophobia_Variables wallVariables;

	// Use this for initialization
	void Start () {
		//set wall variables
		Debug.Log ("Wall position initialized: " + initializeWall ());

		initpos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z); 
		this.transform.position = setStartPosition (initpos);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate () {
		if (Input.GetAxis ("Fire1") < 0.0f) {
			if (stepsMoved < maxMoveSteps){
				stepsMoved ++;
				if (this.tag == "MoveMore") {
					Vector3 pos = initpos;
					pos.x = pos.x + (adjustmentRate * stepsMoved);
					this.transform.position = pos;
				}
				if(this.tag == "MoveLess") {
					Vector3 pos = initpos;
					pos.x = pos.x - (adjustmentRate * stepsMoved);
					this.transform.position = pos;
				}
				Debug.Log ("Current value for stepsMoved: " + stepsMoved + " Current wall x.pos: " + this.transform.position.x + " Initial wall x.pos: " + initpos.x);
				wallVariables.setWallStepsMoved(stepsMoved);
			}

		}
		if (Input.GetAxis ("Fire1") > 0.0f) {
			if(stepsMoved > minMoveSteps){
				stepsMoved --;
				if (this.tag == "MoveMore") {
					Vector3 pos = initpos;
					pos.x = pos.x + (adjustmentRate * stepsMoved);
					this.transform.position = pos;
				}
				if (this.tag == "MoveLess") {
					Vector3 pos = initpos;
					pos.x = pos.x - (adjustmentRate * stepsMoved);
					this.transform.position = pos;
				}
			}
			Debug.Log ("Current value for stepsMoved: " + stepsMoved + " Current wall x.pos: " + this.transform.position.x + " Initial wall x.pos: " + initpos.x);
			wallVariables.setWallStepsMoved(stepsMoved);
		}
	}

	bool initializeWall(){
		try{
			GameObject sv = GameObject.Find ("SimulationVariables");
			wallVariables = sv.GetComponent<Claustrophobia_Variables>();
			maxMoveSteps = wallVariables.getMaxWallMoveSteps ();
			minMoveSteps = wallVariables.getMinWallMoveSteps ();
			adjustmentRate = wallVariables.getWallAdjustmentRate ();
			//if a predefined value not already set calculate steps moved and update simulation variable script
			if (wallVariables.getWallStepsMoved () < 0) {
				stepsMoved = setStepsMoved ();
				wallVariables.setWallStepsMoved (stepsMoved);
				Debug.Log ("User defined wall steps below 0; should be " + stepsMoved + ", currently " + wallVariables.getWallStepsMoved());
			}
			else{
				stepsMoved = wallVariables.getWallStepsMoved();
				Debug.Log ("User defined wall steps greater or equal 0; should be " + stepsMoved + ", currently " + wallVariables.getWallStepsMoved());
			}
			return true;
		}
		catch(UnityException e){
			Debug.LogError(e.Message);
		}
		return false;
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
		return ((maxMoveSteps + minMoveSteps) / 2);
	}
}
