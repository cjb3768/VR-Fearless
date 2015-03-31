using UnityEngine;
using System.Collections;

/**
 * Defines a class to handle Bioharness packet metadata
 * C# implementation of ZephyrPacketArgs.java, in Bioharness SDK sample android project
 */

public class ZephyrPacketArgs {

	private byte[] packetBytes;
	public byte[] getBytes() {
		return packetBytes;
	}
		
	private int messageID;
	public int getID() {
		return messageID;
	}
		
	private byte packetStatus;
	public byte getStatus() {
		return packetStatus;
	}
		
	private byte numReceivedBytes;
	public byte getNumReceivedBytes() {
		return numReceivedBytes;
	}
		
	private byte crcStatus;
	public byte getCrcStatus() {
		return crcStatus;
	}

	/**
	 * Class constructor
	 */
	public ZephyrPacketArgs(int msgID, byte[] data, byte status, byte NumRcvdBytes, byte CrcFailStatus)
	{
		messageID = msgID;
		packetBytes = data;
		packetStatus = status;	
		numReceivedBytes = NumRcvdBytes;
		crcStatus = CrcFailStatus;
	}

}
