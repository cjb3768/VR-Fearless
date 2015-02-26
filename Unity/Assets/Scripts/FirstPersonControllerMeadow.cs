using UnityEngine;
using System.Collections;

public class FirstPersonControllerMeadow : MonoBehaviour {
	
	public float movementSpeed = 5.0f;
	
	// Use this for initialization
	void Start () {
		
	}

	void OnGui () {
		var testTextArea = new Rect (0, 0, Screen.width, Screen.height);
		GUI.Label (testTextArea, "This is test text. Gold Team Rules.");
	}

	// Update is called once per frame
	void Update () {
		//Rotation
		float rotLeftRight = Input.GetAxis ("Right Analog X");
		float rotUpDown = Input.GetAxis ("Right Analog Y") * -1;
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
		
		
		CharacterController cc = GetComponent<CharacterController> ();
		cc.SimpleMove (speed);
		
	}
}
