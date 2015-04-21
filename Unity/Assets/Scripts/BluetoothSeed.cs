using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;

public class BluetoothSeed : MonoBehaviour {

	public RuntimePlatform currentPlatform;

	//public static SerialPort serial = new SerialPort(serialPortName, 115200, Parity.None, 8, StopBits.One);
	public SerialPort serial;
	string serialPortName;

	//public string message;
	public string filePath;
	public System.IO.StreamWriter outputFile;

	public List<SummaryPacket> summaryPackets;
	//public string receivedData = "";

	private static byte STX = 0x02;
	private static byte ETX = 0x03;
	private static byte ACK = 0x06;
	private static byte NAK = 0x15;

	//here are the packets we currently need to send to the device
	public static byte[] lifesignMessage = {STX, 0x23, 0x00, 0x00, ETX};
	public static byte[] summaryPacketsStartRequest = {STX, (byte)0xBD , 0x02, 0x01, 0x00, 0x00, ETX};
	public static byte[] summaryPacketsStopRequest = {STX, (byte)0xBD , 0x02, 0x00, 0x00, 0x00, ETX};
	public static byte[] btLinkConfigPacket = {STX, (byte) 0xA4, 0x04, (byte) 0x0e, 0x00, 0x00, 0x00, 0x00, ETX}; 

	/* for people who may be reading this code in the future: look in ZephyrPacket.cs for sample packets for ECG, breathing rate, etc */
	/* specific information about the format of the packets can be found in the Bioharness Bluetooth Comms Link Specification document that comes with the bioharness SDK */
	
	private static int CRC8_POLY = 0x8C;
	public CRC8 crc = new CRC8(CRC8_POLY);

	void Start() {

		currentPlatform = Application.platform;

		if (currentPlatform == RuntimePlatform.WindowsEditor || currentPlatform == RuntimePlatform.WindowsPlayer) {
			Debug.Log ("Running on Windows");
			serialPortName = "COM4";
			filePath = "Logs\\";
		} else if (currentPlatform == RuntimePlatform.OSXEditor || currentPlatform == RuntimePlatform.OSXPlayer) {
			Debug.Log ("Running on OSX");
			//get port name
			filePath = "Logs/";
		}

		serial = new SerialPort(serialPortName, 115200, Parity.None, 8, StopBits.One);

		DontDestroyOnLoad (transform.gameObject);

		//filePath = "Logs\\";

		summaryPackets = new List<SummaryPacket>();

		serial.ReadBufferSize = 4096;	
		serial.WriteBufferSize = 2048;

		OpenConnection (serial);
		sendSummaryPacketStartRequest ();
		InvokeRepeating ("printBytesToRead", 1, 1);
		InvokeRepeating ("sendLifesign", 0, 10);
	}

	void Update() { 

	} 
	
	void OnGUI()    {

		//GUI.Label (new Rect (10, 160, 100, 220), "Port Status: " + message);
		
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
				//Array.Copy (input, inputIterator - 1, payload, 0, payloadLength);
				Array.Copy (input, inputIterator, payload, 0, payloadLength);
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
					SummaryPacket sp = new SummaryPacket(messageID, payloadLength, payload, crcValue, terminatingByte, Application.loadedLevel);
					packets.Add(sp);
					//Debug.Log ("Packet added: packets.Length = " + packets.Count);
					Debug.Log ("Packet is of message type : " + messageID);
					Debug.Log ("Packet sequence number : "  + sp.getSequenceNumber());
					Debug.Log ("Packet heartrate data = " + sp.getHeartRate());
					Debug.Log ("Packet heartrate confidence = " + (byte) sp.getHeartRateConfidence());
					Debug.Log ("Packet heartrate variability = " + sp.getHeartRateVariability());
					Debug.Log ("Packet respiration rate data = " + sp.getRespirationRate());
					Debug.Log ("Packet breathing rate confidence = " + (byte) sp.getBreathingRateConfidence());
					Debug.Log ("Packet system confidence = " + sp.getSystemConfidence());
					Debug.Log ("Packet gsr data = " + sp.getGsr());
				}
				else{
					packets.Add (new DataPacket(messageID, payloadLength, payload, crcValue, terminatingByte));
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
		for (int i = 0; i < packets.Count; i++) {
			if (packets[i].getMessageID() == (byte)0x2B){
				summaryPackets.Add 
					(new SummaryPacket(packets[i].getMessageID(),
					                   packets[i].getPayloadSize(),
					                   packets[i].getPayload (),
					                   packets[i].getCRC (),
					                   packets[i].getTerminatingByte(),
					                   Application.loadedLevel));
			}
		}
		return packets;
	}

	public void writeLogToFile(List<SummaryPacket> packets){
		long hrs;
		long min;
		long sec;

		DateTime rightNow = DateTime.Now;
		filePath += "log-" + rightNow.Month + "-" + rightNow.Day + "-"
			+ rightNow.Year + "-" + rightNow.Hour + "-" 
			+ rightNow.Minute + "-" + rightNow.Second + ".txt";

		outputFile = new System.IO.StreamWriter(@filePath);

		using (outputFile){
			foreach (SummaryPacket sp in packets){
				hrs = (sp.getTimestampMilliseconds () / 3600000);
				min = ((sp.getTimestampMilliseconds () % 3600000) / 60000);
				sec = (((sp.getTimestampMilliseconds () % 3600000) % 60000) / 1000);
				string lineToPrint = sp.getTimestampMonth() + "/" 
					+ sp.getTimestampDay() + "/"
					+ sp.getTimestampYear() + ","
					+ hrs + ":" + min + ":" + sec + ","
					+ sp.getHeartRate() + ","
					+ sp.getHeartRateVariability() + ","
					+ sp.getHeartRateConfidence() + ","
					+ sp.getCurrentScene() + "\n";
				outputFile.WriteLine(lineToPrint);
			}
		}
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
				//message = "Closing port, because it was already open!";
			}
			else 
			{
				sp.Open(); 
				sp.ReadTimeout = 1000;  
				//message = "Port Opened!";
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
			serial.Write (packet, 0, packetLength);
		} else if (packet [1] == (byte)0xBD) {
			//summary packet start or stop request
			byte[] crcCalc = new byte[2];
			Array.Copy (packet, 3, crcCalc, 0, 2);
			packet [5] = crc.Calculate (crcCalc);
			serial.Write (packet, 0, packet.Length);
		} 
		else if (packet [1] == (byte)0xA4) {
			//bt config set packet
			//printPacket (packet);
			byte[] crcCalc = new byte[4];
			Array.Copy (packet, 3, crcCalc, 0, 4);
			packet [7] = crc.Calculate (crcCalc);
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
		writeLogToFile (summaryPackets);
		serial.Close();
	}
}
