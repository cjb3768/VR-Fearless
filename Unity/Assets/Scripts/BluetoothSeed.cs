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

	//here are the packets we currently need to send to the device; may need to convert to match endianness
	public byte[] lifesignMessage = {0x02, 0x23, 0x00, 0x00, 0x03};
	public byte[] invertedLifesignMessage = {0x03, 0x00, 0x00, 0x23, 0x02};
	public byte[] summaryPacketsStartRequest = {0x02, (byte)0xBD , 0x02, 0x01, 0x00, 0x00, 0x03};
	public byte[] summaryPacketsStopRequest = {0x02, (byte)0xBD , 0x02, 0x00, 0x00, 0x00, 0x03};

	private static byte STX = 0x02;
	private static byte ETX = 0x03;
	private static byte ACK = 0x06;
	private static byte NAK = 0x15;

	private static int CRC8_POLY = 0x8C;
	public CRC8 crc = new CRC8(CRC8_POLY);

	void Start() {
		
		OpenConnection();
		//send lifesign message
		//serial.Write (lifesignMessage, 0, 5);
		//writeToConnection (lifesignMessage);
		//writeToConnection (invertedLifesignMessage);
	}

	void Update() { 

		InvokeRepeating ("sendLifesign", 0, 2);
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


	public void sendLifesign(){
		serial.Write (invertedLifesignMessage, 0, invertedLifesignMessage.Length);
	}
	/**
	 * Write a passed in packet to the serial port
	 */
	public void writeToConnection(byte[] packet){
		//determine packet type by message id
		if (packet [1] == 0x23) {
			//lifesign
			serial.Write (packet, 0, packet.Length);
		}
		else if (packet[1] == (byte) 0xBD){
			//summary packet start or stop request
			byte[] crcCalc = new byte[2];
			Array.Copy(packet, 3, crcCalc, 0, 2);
			packet[5] = crc.Calculate(crcCalc);
			serial.Write (packet, 0, packet.Length);
		}
	}
	
	
	
	void OnApplicationQuit() {
		serial.Close();
	}
}
