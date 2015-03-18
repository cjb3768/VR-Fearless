using UnityEngine;
using System.Collections;

public class Footsteps : MonoBehaviour {

	public AudioClip myAudioClip;
	Vector3 currentPosition;
	enum movementState {moving, stopped};
	movementState currentState;

	/**
	 * Play or pause audio as appropriate, based on previous and current movementStates
	 */
	void updateAudio (movementState previousState, movementState currentState) {
		//player was moving
		if (previousState == movementState.moving) {
			//player still moving
			if (currentState == movementState.moving) {
				//do nothing
			}
			//player no longer moving
			else {
				audio.Stop ();
			}
		}
		//player was not moving
		else {
			//player started moving
			if (currentState == movementState.moving){
				audio.Play ();
			}
			//player still not moving
			else {
				//do nothing
			}
		}
	}

	// Use this for initialization
	void Start () {
		currentPosition = transform.position;
		currentState = movementState.stopped;
		audio.clip = myAudioClip;
		audio.volume = 1;

	}
	
	// Update is called once per frame
	void Update () {
		//update currentPosition
		Vector3 previousPosition = currentPosition;
		currentPosition = transform.position;

		//update currentState
		movementState previousState = currentState;
		//if ((currentPosition.x != currentPosition.x) || (currentPosition.z != currentPosition.z)) {
		//if ((previousPosition.x != currentPosition.x)) {
		if (previousPosition != currentPosition) {
			currentState = movementState.moving;
		}
		else {
			currentState = movementState.stopped;
			//audio.Stop ();
		}
	
		//update audio
		updateAudio (previousState, currentState);
	}
}
