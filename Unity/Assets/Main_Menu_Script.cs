using UnityEngine;
using System.Collections;

public class Main_Menu_Script : MonoBehaviour {
	public int ButtonHeight = 50;
	public int ButtonWidth = 150;
	public int Spacing = 37;
	public Texture dreams;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// GUI buttons for loading stuff
	void OnGUI () {
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), dreams);

		GUILayout.BeginArea (new Rect(Screen.width / 2 - ButtonWidth / 2, Screen.height / 2 - 200, ButtonWidth, 400));
		if(GUILayout.Button("Begin Simulation", GUILayout.Height(ButtonHeight))){
			//do nothing for now
			Application.LoadLevel("Claustrophobia");
		}

		GUILayout.EndArea ();
	}
}
