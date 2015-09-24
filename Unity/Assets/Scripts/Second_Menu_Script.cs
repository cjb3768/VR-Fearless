using UnityEngine;
using System.Collections;

public class Second_Menu_Script : MonoBehaviour {
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

	void Awake(){
		Cursor.visible = true;
		Screen.lockCursor = false;
	}

	// GUI buttons for loading stuff
	void OnGUI () {
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), dreams);
		if (mainpage == 0) {
			GUILayout.BeginArea (new Rect (Screen.width / 2 - ButtonWidth / 2, Screen.height / 2 - 200, ButtonWidth, 400));
			if (GUILayout.Button ("Begin Height", GUILayout.Height (ButtonHeight))) {
				//do nothing for now
				mainpage = 1;
			}
			if(GUILayout.Button ("Begin Claustrophobia", GUILayout.Height(ButtonHeight))){
				mainpage = 2;
			}
			if (GUILayout.Button ("Exit", GUILayout.Height(ButtonHeight))){
				// Nothing for now
				Application.Quit();
			}

			GUILayout.EndArea ();
		}
		if (mainpage == 1) {
			//hslidervalue = EditorGUI.IntSlider(Rect(Screen.width/ 2 - ButtonWidth/2, Screen.height/2 - 100, ButtonWidth, 20), hslidervalue, 1.0f, 2.0f);
			GUILayout.BeginArea (new Rect (Screen.width / 2 - ButtonWidth / 2, Screen.height / 2 - 200, ButtonWidth, 400));
			//hslidervalue = GUILayout.HorizontalSlider(hslidervalue, 1.0f, 2.0f);
			//hslidervalue = (int)hslidervalue;
			if (GUILayout.Button ("Start", GUILayout.Height (ButtonHeight))) {
				Application.LoadLevel ("heightterrain");
			}
			if(GUILayout.Button("Back", GUILayout.Height(ButtonHeight))) {
				mainpage = 0;
			}
			
			GUILayout.EndArea ();
			
		}
		if (mainpage == 2) {
			GUILayout.BeginArea (new Rect (Screen.width / 2 - ButtonWidth / 2, Screen.height / 2 - 200, ButtonWidth, 400));
			//hslidervalue = GUILayout.HorizontalSlider(hslidervalue, 1.0f, 2.0f);
			//hslidervalue = (int)hslidervalue;
			if (GUILayout.Button ("Start", GUILayout.Height (ButtonHeight))) {
				Application.LoadLevel ("claustrophobia2");
			}
			if(GUILayout.Button("Back", GUILayout.Height(ButtonHeight))) {
				mainpage = 0;
			}
			
			GUILayout.EndArea ();

		}
}
}
