using UnityEngine;
using System.Collections;

public class Main_Menu_Script : MonoBehaviour {
	int mainpage = 0;
	public int ButtonHeight = 50;
	public int ButtonWidth = 150;
	public int Spacing = 37;
	public Texture dreams;
	float hslidervalue = 1.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// GUI buttons for loading stuff
	void OnGUI () {
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), dreams);
		if (mainpage == 0) {
			GUILayout.BeginArea (new Rect (Screen.width / 2 - ButtonWidth / 2, Screen.height / 2 - 200, ButtonWidth, 400));
			if (GUILayout.Button ("Begin Simulation", GUILayout.Height (ButtonHeight))) {
				//do nothing for now
				mainpage = 1;
			}
			if (GUILayout.Button ("Exit", GUILayout.Height(ButtonHeight))){
				// Nothing for now
				Application.Quit();
			}

			GUILayout.EndArea ();
		}
		if (mainpage == 1) {
			GUILayout.BeginArea (new Rect (Screen.width / 2 - ButtonWidth / 2, Screen.height / 2 - 200, ButtonWidth, 400));
			//hslidervalue = GUILayout.HorizontalSlider(hslidervalue, 1.0f, 2.0f);
			if (GUILayout.Button ("Begin Acclimation", GUILayout.Height (ButtonHeight))) {
				Application.LoadLevel (1);
			}
			if(GUILayout.Button("Back", GUILayout.Height(ButtonHeight))) {
				mainpage = 0;
			}
			
			GUILayout.EndArea ();
			
		}
		if (mainpage == 2) {
			GUILayout.BeginArea (new Rect (Screen.width / 2 - ButtonWidth / 2, Screen.height / 2 - 200, ButtonWidth, 400));
			hslidervalue = GUILayout.HorizontalSlider(hslidervalue, 1.0f, 2.0f);
			if (GUILayout.Button ("Begin Simulation", GUILayout.Height (ButtonHeight))) {
				Application.LoadLevel ((int)hslidervalue);
			}
			if(GUILayout.Button("Back", GUILayout.Height(ButtonHeight))) {
				mainpage = 0;
			}
			
			GUILayout.EndArea ();

		}
}
}
