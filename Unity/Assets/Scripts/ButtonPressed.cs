using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonPressed : MonoBehaviour {

	public Text targetText;
	public string buttonPressed = "A Button";
	//public string pressString = buttonPressed + " Pressed";
	//public string releaseString = buttonPressed + " Released";
	//private GameObject myGuiText;

	// Use this for initialization
	void Start () {
		//myGuiText = GameObject.Find(targetGuiText);
		targetText.text = buttonPressed + " Not Pressed";
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown (buttonPressed)) {
			targetText.text = buttonPressed + " Pressed";
		}
		else if (Input.GetButtonUp (buttonPressed)) {
			targetText.text = buttonPressed + " Released";
		}
	}
}
