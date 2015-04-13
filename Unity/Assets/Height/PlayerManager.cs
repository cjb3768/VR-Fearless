using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {
	bool nearButton = false;
	bool onBridge = false;

	private CharacterController Controller = null;

	public AudioClip gravelFootsteps;
	public AudioClip bridgeFootsteps;
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
		audio.clip = gravelFootsteps;
		audio.volume = 1;

		Controller = gameObject.GetComponent<CharacterController> ();

	}
	
	// Update is called once per frame
	void Update () {
		if (nearButton) {
			if (Input.GetMouseButtonDown (0)) {
				Application.LoadLevel (0);
			}
		}


		//update currentPosition
		/*Vector3 previousPosition = currentPosition;
		currentPosition = transform.position;
		
		//update currentState
		movementState previousState = currentState;
		//if ((currentPosition.x != currentPosition.x) || (currentPosition.z != currentPosition.z)) {
		//if ((previousPosition.x != currentPosition.x)) {
		if (previousPosition != currentPosition) {
			currentState = movementState.moving;
			Debug.Log ("Now moving");
		}
		else {
			currentState = movementState.stopped;
			Debug.Log ("Now stopped");
			//audio.Stop ();
		}*/
		/*
		movementState previousState = currentState;

		double vel = Controller.velocity.magnitude;

		double threshold = .2;

		if (vel > threshold && previousState == movementState.stopped) {
			currentState = movementState.moving;
			//Debug.Log("Moving");
		} else if(vel <= threshold && previousState == movementState.moving){
			currentState = movementState.stopped;
			//Debug.Log ("Stopped");
		}

		//update audio
		updateAudio (previousState, currentState);
	*/
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.tag.Equals ("Bridge")) {
			//Debug.Log ("On bridge");
			onBridge = true;
			audio.clip = bridgeFootsteps;
			audio.Stop ();
			if(currentState == movementState.moving)
				audio.Play ();
		} else {
			//Debug.Log ("Near button");
			nearButton = true;
		}
	}

	void OnTriggerExit(Collider other){
		if (other.tag.Equals ("Bridge")) {
			//Debug.Log ("Off bridge");
			onBridge = false;
			audio.clip = gravelFootsteps;
			audio.Stop ();
			if(currentState == movementState.moving)
				audio.Play ();
		} else {
			//Debug.Log ("Away button");
			nearButton = false;
		}
	}

}
