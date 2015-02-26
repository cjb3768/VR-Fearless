using UnityEngine;
using System.Collections;

public class ButtonPressed : MonoBehaviour {

	public GUIText targetGuiText;
	public string buttonPressed = "A Button";
	public string pressString = "Button Pressed";
	public string releaseString = "Button Released";
	//private GameObject myGuiText;

	// Use this for initialization
	void Start () {
		//myGuiText = GameObject.Find(targetGuiText);
		targetGuiText.text = releaseString;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (buttonPressed)) {
			targetGuiText.text = pressString;
		}
	}
}
