using UnityEngine;
using System;
using System.Collections;

public class DataPacket {

	byte messageID;
	byte payloadSize;
	byte[] payload;
	byte crc;
	byte terminatingByte;

	private int CRC8_POLY = 0x8C;
	private CRC8 packetCRC;
	
	public DataPacket(byte _mid, byte _pls, byte[] _pl, byte _crc, byte _tb){
		messageID = _mid;
		payloadSize = _pls;
		payload = new byte[payloadSize];
		Array.Copy (_pl, payload, _pl.Length);
		crc = _crc;
		terminatingByte = _tb;

		packetCRC = new CRC8 (CRC8_POLY);
	}

	public byte getMessageID(){
		return messageID;
	}

	public byte getPayloadSize(){
		return payloadSize;
	}

	public byte[] getPayload(){
		return payload;
	}

	public byte getCRC(){
		return crc;
	}

	public byte getTerminatingByte() {
		return terminatingByte;
	}

	public bool checkCRC(){
		byte[] crcCalc = new byte[payloadSize];
		byte crcResult = 0x00;

		Array.Copy (payload, crcCalc, payloadSize);
		crcResult = packetCRC.Calculate (crcCalc);

		if (crc == crcResult) {
			return true;
		} 
		else {
			return false;
		}
	}
}
