using UnityEngine;
using System.Collections;

public class PositionTracker : MonoBehaviour {

	public struct playerPosition{
		public float xPos, yPos, zPos;
		public float wRot, xRot, yRot, zRot;
		public string screenshotName;
	};

	public GameObject player;
	public bool inModule = false;
	public int numFramesSinceScreenCap = 0;
	public int framesBetweenScreenCaps;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (transform.gameObject);

	}
	
	// Update is called once per frame
	void Update () {
		if (inModule == false){
			if (Application.loadedLevel == 1 || Application.loadedLevel == 3 || Application.loadedLevel == 4) {
				//simulation loaded
				player = GameObject.Find ("OVRPlayerController");
				playerPosition p;
				p.xPos = player.transform.position.x;
				p.yPos = player.transform.position.y;
				p.zPos = player.transform.position.z;
				p.wRot = player.transform.rotation.w;
				p.xRot = player.transform.rotation.x;
				p.yRot = player.transform.rotation.y;
				p.zRot = player.transform.rotation.z;
				/*
				Debug.Log ("Player position: (" + player.transform.position.x + ", " 
				           + player.transform.position.y + ", "
				           + player.transform.position.z + ")\t" 
						   + "Player rotation: (" + player.transform.rotation.w + ", "
				           + player.transform.rotation.x + ", "
				           + player.transform.rotation.y + ", "
				           + player.transform.rotation.z + ")\n");
				*/

				Debug.Log ("Player position: (" + p.xPos + ", " + p.yPos + ", " + p.zPos + ")\t" 
				           + "Player rotation: (" + p.wRot + ", " + p.xRot + ", " + p.yRot + ", " + p.zRot + ")\n");
				//inModule = true;
			}
			else{
				Debug.Log ("In a menu");
			}

		}
		else{
			if (Application.loadedLevel == 0 || Application.loadedLevel == 2){
				//menus
			}
			else{
				//simulation
			}
		}
	}

}
