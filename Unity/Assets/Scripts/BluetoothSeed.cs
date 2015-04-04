using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;

public class BluetoothSeed : MonoBehaviour {

	public static SerialPort serial = new SerialPort("COM4", 115200, Parity.None, 8, StopBits.One);
	public string message, message1;
	public string message2;
	//public string receivedData = "";
	public int ticksTillNextMessage = 2;

	private static byte STX = 0x02;
	private static byte ETX = 0x03;
	private static byte ACK = 0x06;
	private static byte NAK = 0x15;

	//here are the packets we currently need to send to the device; may need to convert to match endianness
	public static byte[] lifesignMessage = {0x02, 0x23, 0x00, 0x00, 0x03};
	//public static byte[] lifesignMessage = {ETX, 0x00, 0x00, 0x23, STX};
	//public static byte[] summaryPacketsStartRequest = {0x02, (byte)0xBD , 0x02, 0x01, 0x00, 0x00, 0x03};
	public static byte[] summaryPacketsStartRequest = {STX, (byte)0xBD , 0x02, 0x01, 0x00, 0x00, ETX};
	//public static byte[] summaryPacketsStartRequest = {0x03, 0x00 , 0x00, 0x01, 0x02, (byte)0xBD, 0x02};
	//public static byte[] summaryPacketsStartRequest = {ETX, 0x00 , 0x00, 0x01, 0x02, (byte)0xBD, STX};
	//public static byte[] summaryPacketsStopRequest = {0x02, (byte)0xBD , 0x02, 0x00, 0x00, 0x00, 0x03};
	public static byte[] summaryPacketsStopRequest = {STX, (byte)0xBD , 0x02, 0x00, 0x00, 0x00, ETX};
	//public static byte[] summaryPacketsStopRequest = {0x03, 0x00 , 0x00, 0x00, 0x02, (byte)0xBD, 0x02};
	//public static byte[] summaryPacketsStopRequest = {ETX, 0x00 , 0x00, 0x00, 0x02, (byte)0xBD, STX};
	public static byte[] btLinkConfigPacket = {STX, (byte) 0xA4, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, ETX}; 
	//public static byte[] btLinkConfigPacket = {ETX, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, (byte)0xA4, STX};

	private static int CRC8_POLY = 0x8C;
	public CRC8 crc = new CRC8(CRC8_POLY);

	void Start() {
		
		OpenConnection();
		//send lifesign message
		//serial.Write (lifesignMessage, 0, 5);
		//writeToConnection (lifesignMessage);
		//writeToConnection (summaryPacketsStartRequest);
		//writeToConnection (btLinkConfigPacket);
	}

	void Update() { 

		InvokeRepeating ("sendLifesign", 0, 8);
		//InvokeRepeating ("writeToConnection(lifesignMessage)", 0, 1);
		/*
		//int mess = serial.ReadByte ();
		//message2 = (string) mess;
		ticksTillNextMessage --;

		//try to send a packet for lifesigns
		if (ticksTillNextMessage == 0) {
			//message2 = serial.ReadLine ();
			//byte[] receivedData = serial.ReadExisting ();
			//int messageType = (int) receivedData[1];
			//Debug.Log ("Message ID: " + messageType + "\n");
			//writeToConnection (lifesignMessage);
			writeToConnection (invertedLifesignMessage);
			ticksTillNextMessage = 2;

		}
		*/
	} 
	
	void OnGUI()    {

		GUI.Label(new Rect(10, 160, 100, 220), "Port Status: " + message);
		GUI.Label(new Rect(10, 180, 100, 220), "Sensor1: " + message2);
		
	}

	public byte getMsgID (byte[] packet){
		return packet [1];
	}

	public byte getDLC (byte[] packet){
		return packet [2];
	}
	
	public void OpenConnection() {
		if (serial != null) 
		{
			if (serial.IsOpen) 
			{
				serial.Close();
				message = "Closing port, because it was already open!";
			}
			else 
			{
				serial.Open(); 
				serial.ReadTimeout = 1000;  
				message = "Port Opened!";
			}
		}
		else 
		{
			if (serial.IsOpen)
			{
				print("Port is already open");
			}
			else 
			{
				print("Port == null");
			}
		}
	}

	public void reversePacket(byte[] sourcePacket, byte[] reversedPacket){
		int length = sourcePacket.Length;
		//byte[] rPacket = new byte[length];
		for (int i = 0; i < length; i++){
			reversedPacket[i] = sourcePacket[length-(i+1)];
		}
	}


	public void sendLifesign(){
		Debug.Log ("in function lifesign message length = " + lifesignMessage.Length + "\n");
		//serial.Write (lifesignMessage, 0, lifesignMessage.Length);
		writeToConnection (lifesignMessage);
	}
	/**
	 * Write a passed in packet to the serial port
	 */
	public void writeToConnection(byte[] packet){
		int packetLength = packet.Length;
		byte[] reversedPacket = new byte[packetLength];
		Debug.Log ("packetlength = " + packetLength + "\n");
		//determine packet type by message id
		if (packet [1] == 0x23) {
			//lifesign
			reversePacket (packet, reversedPacket);
			Debug.Log ("size of newPacket = " + reversedPacket.Length + "\n");
			serial.Write (reversedPacket, 0, reversedPacket.Length);
		} 
		else if (packet [1] == (byte)0xBD) {
			//summary packet start or stop request
			byte[] crcCalc = new byte[2];
			Array.Copy (packet, 3, crcCalc, 0, 2);
			packet [5] = crc.Calculate (crcCalc);
			reversePacket (packet, reversedPacket);
			Debug.Log ("size of newPacket = " + reversedPacket.Length + "\n");
			serial.Write (reversedPacket, 0, reversedPacket.Length);
		} 
		else if (packet [1] == (byte)0xA4) {
			//bt config set packet
			byte[] crcCalc = new byte[4];
			Array.Copy (packet, 3, crcCalc, 0, 4);
			packet [7] = crc.Calculate (crcCalc);
			reversePacket (packet, reversedPacket);
			Debug.Log ("size of newPacket = " + reversedPacket.Length + "\n");
			serial.Write (reversedPacket, 0, reversedPacket.Length);
			//serial.Write (packet, 0, packet.Length);
		}
	}
	
	
	
	void OnApplicationQuit() {
		serial.Close();
	}
}
