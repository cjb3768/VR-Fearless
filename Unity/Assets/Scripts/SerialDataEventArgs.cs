using UnityEngine;
using System;
using System.Collections;

public class SerialDataEventArgs : EventArgs {

	byte [] Data;

	public SerialDataEventArgs (byte[] dataFromEvent){
		Data = dataFromEvent;
	}
}
