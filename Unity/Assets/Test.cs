using UnityEngine;
using System;
using System.Collections;
using weka;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log ("Testing...");
		classifyTest ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void classifyTest()
	{

		//Debug.Log ("Attempting to read file...");
		//Read a .arff for usage
		weka.core.Instances insts = new weka.core.Instances(new java.io.FileReader("ConvertedLogs/3.arff"));

		//Debug.Log ("Attempting to set class index...");

		insts.setClassIndex(insts.numAttributes() - 1);

		//Debug.Log ("Loading classifier...");

		//Specify model to be used here
		//A model is a pre-trained 
		weka.classifiers.Classifier kstar = (weka.classifiers.Classifier) weka.core.SerializationHelper.read ("Models/kstar.model");
		//Debug.Log ("Performing KStar evaluation");

		//Perform the evaluation on the .arff specified
		//weka.classifiers.Evaluation dataTest = new weka.classifiers.Evaluation(insts);
		//dataTest.evaluateModel(kstar,insts);

		weka.core.Instance topInst = insts.instance (0);
		double predictedClass = kstar.classifyInstance (topInst);

		//The prediction for the top instance of data.
		Debug.Log (predictedClass);


	}




}
