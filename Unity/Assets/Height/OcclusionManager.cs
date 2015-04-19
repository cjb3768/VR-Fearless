using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class OcclusionManager : MonoBehaviour {

	private List< GameObject[] > Railings;

	// Use this for initialization
	void Start () {
		Railings = new List<GameObject[]> ();
		SetInitialGroups ();
		VanishAllGroups ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Unity does not allow an object to have more than one tag at once.
	//By having an empty subobject hold the tag, we can fake an object having more than one tag
	//Just find the parent of the tagholder
	void SetInitialGroups()
	{
		GameObject[] g = GameObject.FindGameObjectsWithTag ("Railings1");
		for (int i=0; i<g.Length; i++)
			g [i] = g [i].transform.parent.gameObject;
		Railings.Add (g);

		g = GameObject.FindGameObjectsWithTag ("Railings2");
		for (int i=0; i<g.Length; i++)
			g [i] = g [i].transform.parent.gameObject;
		Railings.Add (g);

		g = GameObject.FindGameObjectsWithTag ("Railings3");
		for (int i=0; i<g.Length; i++)
			g [i] = g [i].transform.parent.gameObject;
		Railings.Add (g);

		g = GameObject.FindGameObjectsWithTag ("Railings4");
		for (int i=0; i<g.Length; i++)
			g [i] = g [i].transform.parent.gameObject;
		Railings.Add (g);

		g = GameObject.FindGameObjectsWithTag ("Railings5");
		for (int i=0; i<g.Length; i++)
			g [i] = g [i].transform.parent.gameObject;
		Railings.Add (g);

		g = GameObject.FindGameObjectsWithTag ("Railings6");
		for (int i=0; i<g.Length; i++)
			g [i] = g [i].transform.parent.gameObject;
		Railings.Add (g);

		g = GameObject.FindGameObjectsWithTag ("Railings7");
		for (int i=0; i<g.Length; i++)
			g [i] = g [i].transform.parent.gameObject;
		Railings.Add (g);

		g = GameObject.FindGameObjectsWithTag ("Railings8");
		for (int i=0; i<g.Length; i++)
			g [i] = g [i].transform.parent.gameObject;
		Railings.Add (g);

		g = GameObject.FindGameObjectsWithTag ("Railings9");
		for (int i=0; i<g.Length; i++)
			g [i] = g [i].transform.parent.gameObject;
		Railings.Add (g);

		g = GameObject.FindGameObjectsWithTag ("Railings10");
		for (int i=0; i<g.Length; i++)
			g [i] = g [i].transform.parent.gameObject;
		Railings.Add (g);
	}

	void VanishAllGroups()
	{
		foreach(GameObject[] railGroup in Railings)
		{
			foreach(GameObject obj in railGroup)
				obj.SetActive(false);
		}
	}

	void EnableDisableGroups(int group)
	{
		//Must disable before enabling unless you want to risk re-disabling the group
		VanishAllGroups ();

		//Now enable
		foreach(GameObject railGroup in Railings[group])
			railGroup.SetActive(true);

	}

	void OnTriggerEnter(Collider other) {
		if (other.tag.Equals ("VBox1")) {
			//Debug.Log ("Entered VBox1");
			EnableDisableGroups(0);
		}
		if (other.tag.Equals ("VBox2")) {
			//Debug.Log ("Entered VBox2");
			EnableDisableGroups(1);
		}
		if (other.tag.Equals ("VBox3")) {
			//Debug.Log ("Entered VBox3");
			EnableDisableGroups(2);
		}
		if (other.tag.Equals ("VBox4")) {
			//Debug.Log ("Entered VBox4");
			EnableDisableGroups(3);
		}
		if (other.tag.Equals ("VBox5")) {
			//Debug.Log ("Entered VBox5");
			EnableDisableGroups(4);
		}
		if (other.tag.Equals ("VBox6")) {
			//Debug.Log ("Entered VBox6");
			EnableDisableGroups(5);
		}
		if (other.tag.Equals ("VBox7")) {
			//Debug.Log ("Entered VBox7");
			EnableDisableGroups(6);
		}
		if (other.tag.Equals ("VBox8")) {
			//Debug.Log ("Entered VBox8");
			EnableDisableGroups(7);
		}
		if (other.tag.Equals ("VBox9")) {
			//Debug.Log ("Entered VBox9");
			EnableDisableGroups(8);
		}
		if (other.tag.Equals ("VBox10")) {
			//Debug.Log ("Entered VBox10");
			EnableDisableGroups(9);
		}

	}



}
