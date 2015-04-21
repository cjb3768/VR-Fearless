using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {
	bool nearButton = false;
	bool onBridge = false;

	private CharacterController Controller = null;

	public AudioClip gravelFootsteps;
	public AudioClip bridgeFootsteps;
	public AudioClip windNoise;

	public AudioSource footsteps;
	public AudioSource wind;
	public AudioSource creak;

	public ParticleEmitter littleDust;
	//public ParticleEmitter muchDust;


	//Vector3 currentPosition;
	enum movementState {moving, stopped};
	movementState currentState;

	public float minScheduleWait = 5f;
	public float maxScheduleWait = 30f;
	private int currentClip = -1;
	private double waitTimeUntilEvent;
	
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
				footsteps.Stop ();
			}
		}
		//player was not moving
		else {
			//player started moving
			if (currentState == movementState.moving){
				footsteps.Play ();
			}
			//player still not moving
			else {
				//do nothing
			}
		}
	}


	// Use this for initialization
	void Start () {
		//currentPosition = transform.position;
		currentState = movementState.stopped;
		footsteps.clip = gravelFootsteps;
		wind.clip = windNoise;

		Controller = gameObject.GetComponent<CharacterController> ();

	}
	
	// Update is called once per frame
	void Update () {
		if (nearButton) {
			if (Input.GetMouseButtonDown (0) || Input.GetButtonDown("A Button") || Input.GetButtonDown("B Button") || Input.GetButtonDown("X Button") || Input.GetButtonDown ("Y Button")) {
				Application.LoadLevel ("Meadow");
			}
		}

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

		//update footstep audio
		updateAudio (previousState, currentState);


		double currentTime = AudioSettings.dspTime;
		if (currentTime + 1.0f > waitTimeUntilEvent) {
			if(onBridge && ((int)currentTime) % 2 == 0)
				creak.Play();	
			else
			{
				wind.PlayScheduled (waitTimeUntilEvent);
				//muchDust.emit = true;
			}
			//determine next track
			//currentClip = nextClip (currentClip);
			//determine time until next clip plays
			waitTimeUntilEvent += GetWaitTime ();
		}


	}

	float GetWaitTime () {
		float f = Random.Range (minScheduleWait, maxScheduleWait);
		return f;
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.tag.Equals ("Bridge")) {
			Debug.Log ("On bridge");
			onBridge = true;
			footsteps.clip = bridgeFootsteps;
			footsteps.Stop ();
			//littleDust.emit = false;
			if(currentState == movementState.moving)
				footsteps.Play ();
		} else {
			//Debug.Log ("Near button");
			nearButton = true;
		}
	}

	void OnTriggerExit(Collider other){
		if (other.tag.Equals ("Bridge")) {
			Debug.Log ("Off bridge");
			onBridge = false;
			footsteps.clip = gravelFootsteps;
			footsteps.Stop ();
			//littleDust.emit = true;
			if(currentState == movementState.moving)
				footsteps.Play ();
		} else {
			//Debug.Log ("Away button");
			nearButton = false;
		}
	}

}
