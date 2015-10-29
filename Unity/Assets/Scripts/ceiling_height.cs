using UnityEngine;
using System.Collections;

public class ceiling_height : MonoBehaviour {
	float ceilingStartHeight = 3.5f;

	float minMoveSteps;
	float maxMoveSteps;
	float stepsMoved = 0;
	float ceilingAdjustmentRate;
	Vector3 initpos;
	Claustrophobia_Variables ceilingVariables;

	// Use this for initialization
	void Start () {
		initializeCeiling();
		initpos = new Vector3(this.transform.position.x, ceilingStartHeight, this.transform.position.z); 
		this.transform.position = initpos;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetAxis ("Fire1") < 0.0f) {
			//lower ceiling

			//if (this.transform.position.y > 2.0f) {
			if (stepsMoved < maxMoveSteps){
				stepsMoved ++;
				//Vector3 pos = this.transform.position;
				//pos.y = pos.y - (0.005f);
				//this.transform.position = pos;
				Vector3 pos = initpos;
				pos.y = pos.y - (ceilingAdjustmentRate * stepsMoved);
				this.transform.position = pos;
			}
		}
		if (Input.GetAxis ("Fire1") > 0.0f) {
			//raise ceiling

			//if (this.transform.position.y < 6.0f) {
			if (stepsMoved > minMoveSteps){
				stepsMoved --;
				//Vector3 pos = this.transform.position;
				//pos.y = pos.y + (0.005f);
				//this.transform.position = pos;
				Vector3 pos = initpos;
				pos.y = pos.y - (ceilingAdjustmentRate * stepsMoved);
				this.transform.position = pos;
			}
		}
		ceilingVariables.setCeilingStepsMoved(stepsMoved);
	}

	bool initializeCeiling(){
		try{
			GameObject sv = GameObject.Find ("SimulationVariables");
			ceilingVariables = sv.GetComponent<Claustrophobia_Variables>();
			maxMoveSteps = ceilingVariables.getMaxCeilingMoveSteps ();
			minMoveSteps = ceilingVariables.getMinCeilingMoveSteps ();
			ceilingAdjustmentRate = ceilingVariables.getCeilingAdjustmentRate ();
			//if a predefined value not already set calculate steps moved and update simulation variable script
			//if (ceilingVariables.getWallStepsMoved () < 0) {
			//	stepsMoved = setStepsMoved ();
			//	ceilingVariables.setWallStepsMoved (stepsMoved);
			//	Debug.Log ("User defined ceiling steps below 0; should be " + stepsMoved + ", currently " + ceilingVariables.getWallStepsMoved());
			//}
			//else{
			//	stepsMoved = ceilingVariables.getWallStepsMoved();
			//	Debug.Log ("User defined ceiling steps greater or equal 0; should be " + stepsMoved + ", currently " + ceilingVariables.getWallStepsMoved());
			//}
			return true;
		}
		catch(UnityException e){
			Debug.LogError(e.Message);
		}
		return false;
	}
}
