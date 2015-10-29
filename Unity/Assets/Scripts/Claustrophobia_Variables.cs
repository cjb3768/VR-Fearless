using UnityEngine;
using System.Collections;

public class Claustrophobia_Variables : MonoBehaviour {
	//variables for wall movement
	//default values
	//public float maxWallMoveSteps = 1400f;
	//public float minWallMoveSteps = 0f;
	//public float wallStepsMoved = -1f;
	//public float wallAdjustmentRate = 0.005f;
	public float maxWallMoveSteps = 700f;
	public float minWallMoveSteps = 0f;
	public float wallStepsMoved = -1f;
	public float wallAdjustmentRate = 0.01f;

	public float maxCeilingMoveSteps = 350f;
	public float minCeilingMoveSteps = -350f;
	public float ceilingStepsMoved = 0f;
	public float ceilingAdjustmentRate = 0.00214f;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public float getMaxWallMoveSteps(){
		return maxWallMoveSteps;
	}

	public float getMinWallMoveSteps(){
		return minWallMoveSteps;
	}

	public float getWallStepsMoved(){
		return wallStepsMoved;
	}

	public void setWallStepsMoved(float newWallSteps){
		wallStepsMoved = newWallSteps;
	}

	public float getWallAdjustmentRate(){
		return wallAdjustmentRate;
	}

	public float getMaxCeilingMoveSteps(){
		return maxCeilingMoveSteps;
	}
	
	public float getMinCeilingMoveSteps(){
		return minCeilingMoveSteps;
	}
	
	public float getCeilingStepsMoved(){
		return ceilingStepsMoved;
	}

	public void setCeilingStepsMoved(float newCeilingSteps){
		ceilingStepsMoved = newCeilingSteps;
	}

	public float getCeilingAdjustmentRate(){
		return ceilingAdjustmentRate;
	}
}
