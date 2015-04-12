using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;

public class BluetoothSeed : MonoBehaviour {

	public struct dataPacket{
		byte messageID;
		byte payloadSize;
		byte[] payload;
		byte crc;
		byte terminatingByte;

		public dataPacket(byte _mid, byte _pls, byte[] _pl, byte _crc, byte _tb){
			messageID = _mid;
			payloadSize = _pls;
			payload = new byte[payloadSize];
			Array.Copy (_pl, payload, _pl.Length);
			crc = _crc;
			terminatingByte = _tb;
		}
	};

	public static SerialPort serial = new SerialPort("COM4", 115200, Parity.None, 8, StopBits.One);
	//public static SerialPort serialIn = new SerialPort("COM5", 115200, Parity.None, 8, StopBits.One);


	public string message, message1;
	public string message2;
	//public string receivedData = "";
	public int ticksTillNextMessage = 2;
	//public event EventHandler<SerialDataEventArgs> NewSerialDataReceived;

	private static byte STX = 0x02;
	private static byte ETX = 0x03;
	private static byte ACK = 0x06;
	private static byte NAK = 0x15;

	//here are the packets we currently need to send to the device; may need to convert to match endianness
	//public static byte[] lifesignMessage = {0x02, 0x23, 0x00, 0x00, 0x03};
	public static byte[] lifesignMessage = {STX, 0x23, 0x00, 0x00, ETX};
	//public static byte[] lifesignMessage = {ETX, 0x00, 0x00, 0x23, STX};
	//public static byte[] summaryPacketsStartRequest = {0x02, (byte)0xBD , 0x02, 0x01, 0x00, 0x00, 0x03};
	public static byte[] summaryPacketsStartRequest = {STX, (byte)0xBD , 0x02, 0x01, 0x00, 0x00, ETX};
	//public static byte[] summaryPacketsStartRequest = {0x03, 0x00 , 0x00, 0x01, 0x02, (byte)0xBD, 0x02};
	//public static byte[] summaryPacketsStartRequest = {ETX, 0x00 , 0x00, 0x01, 0x02, (byte)0xBD, STX};
	//public static byte[] summaryPacketsStopRequest = {0x02, (byte)0xBD , 0x02, 0x00, 0x00, 0x00, 0x03};
	public static byte[] summaryPacketsStopRequest = {STX, (byte)0xBD , 0x02, 0x00, 0x00, 0x00, ETX};
	//public static byte[] summaryPacketsStopRequest = {0x03, 0x00 , 0x00, 0x00, 0x02, (byte)0xBD, 0x02};
	//public static byte[] summaryPacketsStopRequest = {ETX, 0x00 , 0x00, 0x00, 0x02, (byte)0xBD, STX};
	public static byte[] btLinkConfigPacket = {STX, (byte) 0xA4, 0x04, (byte) 0x0e, 0x00, 0x00, 0x00, 0x00, ETX}; 
	//public static byte[] btLinkConfigPacket = {ETX, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, (byte)0xA4, STX};

	/* for people who may be reading this code in the future: look in ZephyrPacket.cs for sample packets for ECG, breathing rate, etc */
	/* specific information about the format of the packets can be found in the Bioharness Bluetooth Comms Link Specification document that comes with the bioharness SDK */
	
	private static int CRC8_POLY = 0x8C;
	public CRC8 crc = new CRC8(CRC8_POLY);

	void Start() {
		serial.ReadBufferSize = 4096;	
		serial.WriteBufferSize = 2048;

		//serial.DataReceived += new SerialDataReceivedEventHandler (DataReceivedEventHandler);
		//OpenConnection(serialIn);
		OpenConnection (serial);
		//send lifesign message
		//serial.Write (lifesignMessage, 0, 5);
		//writeToConnection (lifesignMessage);
		//writeToConnection (summaryPacketsStartRequest);
		//writeToConnection (btLinkConfigPacket);
		//InvokeRepeating ("sendJunkPacket", 0, 4);
		//InvokeRepeating ("sendLifesign", 0, 5);
		//InvokeRepeating ("sendBTLinkConfig", 0, 5);
		//InvokeRepeating ("sendSummaryPacketStartRequest", 0, 5);
		sendSummaryPacketStartRequest ();
		InvokeRepeating ("printBytesToRead", 1, 1);
		InvokeRepeating ("sendLifesign", 0, 10);
	}

	void Update() { 


		//serial.DataReceived += new SerialDataReceivedEventHandler (DataReceivedEventHandler);
		//serial.DataReceived += new SerialDataReceivedEventHandler (DataReceivedEventHandler);
		//InvokeRepeating ("sendBTLinkConfig", 0, 4);
		//InvokeRepeating ("sendJunkPacket", 0, 4);
		//InvokeRepeating ("sendLifesign", 0, 8);
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

	public void printBytesToRead(){
		//
		Debug.Log ("Test");
		//Debug.Log ("Bytes to read: " + serial.BytesToRead);
		int bytes = serial.ReadBufferSize;
		//char[] s = new char[bytes]; //this one got us 5 byte returns in response to btlinkConfig
		byte[] s = new byte[bytes];
		int r=serial.Read(s,0,bytes);
		//int r = serial.ReadExisting(
		Debug.Log ("read "+r+" bytes: ");
		for (int i=0; i<r; i++) {
			/*
			if ((byte)s[i] == STX){
				Debug.Log ("Start of new packet");
			}
			*/
			//Debug.Log ((byte)s[i]);
			/*
			if ((byte)s[i] == ETX){
				Debug.Log ("End of packet ETX");
			}
			else if ((byte)s[i] == ACK){
				Debug.Log ("End of packet ACK");
			}
			else if ((byte)s[i] == NAK){
				Debug.Log ("End of packet NAK");
			}
			*/
		}
		List<DataPacket> parsedPackets = parseBluetoothInput (s, r);
		Debug.Log ("After parsing input into separate packets, number of packets: " + parsedPackets.Count);
	}

	/**
	 * Parse raw input from bluetooth into separate packets
	 * Returns a list of dataPackets
	 */ 
	public List<DataPacket> parseBluetoothInput(byte[] input, int inputLength){
		//Debug.Log ("inputLength: " + inputLength);
		int inputIterator = 0;

		byte messageID = 0x00;
		byte payloadLength = 0x00;
		byte crcValue = 0x00;
		byte terminatingByte = 0x00;

		List <DataPacket> packets = new List<DataPacket> ();

		while (inputIterator < inputLength){
			//Debug.Log ("inputIterator: " + inputIterator);
			//stx
			if (input[inputIterator] == STX){
				//Debug.Log("Packet found at : " + inputIterator);
				inputIterator ++;

				//get messageID
				messageID = input[inputIterator];
				inputIterator ++;

				//get payloadLength
				payloadLength = input[inputIterator];
				inputIterator ++;

				//get payload
				byte[] payload = new byte[payloadLength];
				//Debug.Log ("About to copy payload");
				Array.Copy (input, inputIterator - 1, payload, 0, payloadLength);
				inputIterator += payloadLength;

				//get crc
				crcValue = input[inputIterator];
				inputIterator ++;

				//get terminating byte
				terminatingByte = input[inputIterator]; 

				if (terminatingByte == ETX){
					//Debug.Log ("Packet ends with etx");
				}
				else if (terminatingByte == ACK){
					//Debug.Log ("Packet ends with ack");
				}
				else if (terminatingByte == NAK){
					//Debug.Log ("Packet ends with nak");
				}

				inputIterator ++;
				//Debug.Log ("Iterator value: " + inputIterator);

				//store data in packet
				if (messageID == (byte) 0x2B){
					SummaryPacket sp = new SummaryPacket(messageID, payloadLength, payload, crcValue, terminatingByte);
					packets.Add(sp);
					Debug.Log ("Packet added: packets.Length = " + packets.Count);
					Debug.Log ("Packet heartrate data = " + sp.getHeartRate());
					Debug.Log ("Packet heartrate confidence = " + (byte) sp.getHeartRateConfidence());
					Debug.Log ("raw heartrate confidence = " + (byte) payload[37]);
					Debug.Log ("Packet heartrate variability = " + sp.getHeartRateVariability());
					Debug.Log ("Packet system confidence = " + sp.getSystemConfidence());
					Debug.Log ("Packet gsr data = " + sp.getGsr());
				}
				//packets.Add(new DataPacket(messageID, payloadLength, payload, crcValue, terminatingByte));
				//Debug.Log ("Packet added: packets.Length = " + packets.Count);

			}
			else{
				Debug.Log ("Bad packet");
				Debug.Log ("expected 2 at " + inputIterator + ", instead got: " + input[inputIterator]);
				//return; <- FIX ME WHEN WE START GETTING BAD PACKETS
			}

		}
		//return packets
		return packets;

	}

	public byte getMsgID (byte[] packet){
		return packet [1];
	}

	public byte getDLC (byte[] packet){
		return packet [2];
	}
	
	public void OpenConnection(SerialPort sp) {
		if (sp != null) 
		{
			if (sp.IsOpen) 
			{
				sp.Close();
				message = "Closing port, because it was already open!";
			}
			else 
			{
				sp.Open(); 
				sp.ReadTimeout = 1000;  
				message = "Port Opened!";
			}
		}
		else 
		{
			if (sp.IsOpen)
			{
				print("Port is already open");
			}
			else 
			{
				print("Port == null");
			}
		}
	}
	/*
	private void DataReceivedEventHandler(object sender, SerialDataReceivedEventArgs e){
		//SerialPort sp = (SerialPort)sender;
		int numBytes = serial.BytesToRead;
		Debug.Log ("Num bytes to read = " + numBytes);
		byte[] data = new byte[numBytes];
		int readData = serial.Read (data, 0, numBytes);
		if (readData == 0) {
			Debug.Log ("No data read");
			return;
		} else {
			Debug.Log ("Packet Received Via Bluetooth");
			printPacket (data);
		}
		////fix tomorrow
		if (NewSerialDataReceived != null) {
			NewSerialDataReceived(this, new SerialDataEventArgs(data));
		}
	}

	public void reversePacket(byte[] sourcePacket, byte[] reversedPacket){
		int length = sourcePacket.Length;
		//byte[] rPacket = new byte[length];
		for (int i = 0; i < length; i++){
			reversedPacket[i] = sourcePacket[length-(i+1)];
		}
	}
	*/
	public void byteToInt(byte[] source, int[] destination){
		int length = source.Length;
		for (int i = 0; i < length; i++){
			destination[i] = source[i];
		}
	}

	public void printPacket(byte[] source){
		int length = source.Length;
		int[] convertedBytes = new int[length];
		byteToInt (source, convertedBytes);
		Debug.Log ("Packet contains: (");
		for (int i = 0; i < length; i++) {
			Debug.Log (convertedBytes[i]+", ");
		}
		Debug.Log ("\n");

	}

	public void sendJunkPacket(){
		byte[] junkPacket = {0x00, 0x00, 0x00, 0x00};
		byte[] reversedPacket = new byte[junkPacket.Length];
		//reversePacket (junkPacket, reversedPacket);
		serial.Write (reversedPacket, 0, reversedPacket.Length);
		Debug.Log ("Sent Z Junk Packet");
	}

	public void sendLifesign(){
		//Debug.Log ("in function lifesign message length = " + lifesignMessage.Length + "\n");
		//serial.Write (lifesignMessage, 0, lifesignMessage.Length);
		writeToConnection (lifesignMessage);
	}

	public void sendBTLinkConfig(){
		//Debug.Log ("in function lifesign message length = " + lifesignMessage.Length + "\n");
		//serial.Write (lifesignMessage, 0, lifesignMessage.Length);
		writeToConnection (btLinkConfigPacket);
	}

	public void sendSummaryPacketStartRequest(){
		writeToConnection (summaryPacketsStartRequest);
	}

	/**
	 * Write a passed in packet to the serial port
	 */
	public void writeToConnection(byte[] packet){
		int packetLength = packet.Length;
		byte[] reversedPacket = new byte[packetLength];
		//Debug.Log ("packetlength = " + packetLength + "\n");
		//determine packet type by message id
		if (packet [1] == 0x23) {
			//lifesign
			//reversePacket (packet, reversedPacket);
			//Debug.Log ("size of newPacket = " + packetLength + "\n");
			//serial.Write (reversedPacket, 0, packetLength);
			serial.Write (packet, 0, packetLength);
		} else if (packet [1] == (byte)0xBD) {
			//summary packet start or stop request
			//printPacket(packet);
			byte[] crcCalc = new byte[2];
			//int[] byteValue = new int[packetLength];

			Array.Copy (packet, 3, crcCalc, 0, 2);
			packet [5] = crc.Calculate (crcCalc);
			//reversePacket (packet, reversedPacket);
			//Debug.Log ("Packet reversed\n");
			//printPacket (reversedPacket);
			//Debug.Log ("size of newPacket = " + reversedPacket.Length + "\n");
			//serial.Write (reversedPacket, 0, reversedPacket.Length);
			serial.Write (packet, 0, packet.Length);
		} 
		else if (packet [1] == (byte)0xA4) {
			//bt config set packet
			//printPacket (packet);
			byte[] crcCalc = new byte[4];
			Array.Copy (packet, 3, crcCalc, 0, 4);
			packet [7] = crc.Calculate (crcCalc);
			//reversePacket (packet, reversedPacket);
			//Debug.Log ("Packet reversed\n");
			//printPacket (reversedPacket);
			//Debug.Log ("size of newPacket = " + reversedPacket.Length + "\n");
			//serial.Write (reversedPacket, 0, reversedPacket.Length);
			serial.Write (packet, 0, packet.Length);
		} 
		else if (packet [1] == 0x14) {
			//general config set packet
			byte[] crcCalc = new byte[1];
			Array.Copy (packet, 3, crcCalc, 0, 1);
			packet [4] = crc.Calculate (crcCalc);
			serial.Write (packet, 0, packet.Length);
		}
	}
	
	void OnApplicationQuit() {
		serial.Close();
	}
}
