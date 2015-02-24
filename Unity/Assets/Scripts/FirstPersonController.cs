using UnityEngine;
using System.Collections;

public class FirstPersonController : MonoBehaviour {

	public float movementSpeed = 5.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//Rotation
		float rotLeftRight = Input.GetAxis ("Mouse X");
		float rotUpDown = Input.GetAxis ("Mouse Y") * -1;
		transform.Rotate (0, rotLeftRight, 0);
		Camera.main.transform.Rotate (rotUpDown, 0, 0);

		//Movement
		float forwardSpeed = Input.GetAxis ("Vertical") * movementSpeed;
		float sideSpeed = Input.GetAxis ("Horizontal") * movementSpeed;
		Vector3 speed = new Vector3 (sideSpeed, 0, forwardSpeed);
		speed = transform.rotation * speed;
		/*if (Input.GetAxis ("Jump")) {
			speed.y = 5.0f;
		}

		speed.y -= 9.8f * Time.deltaTime;*/
		AudioSource stepSound = GetComponent<AudioSource> ();


		CharacterController cc = GetComponent<CharacterController> ();
		cc.SimpleMove (speed);
	
	}
}
