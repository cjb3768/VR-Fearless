using UnityEngine;
using System.Collections;

/**
 * Defines a class to handle error checking in packets
 * C# implementation of CRC8.java, in Bioharness SDK sample android project
 */

public class CRC8 {
	
	private int _crc8Poly;
	
	public CRC8(int crc8Poly)
	{
		_crc8Poly = crc8Poly;
	}

	public void reverseEndianness(byte[] sourcePacket, byte[] reversedPacket){
		int length = sourcePacket.Length;
		//byte[] rPacket = new byte[length];
		for (int i = 0; i < length; i++){
			reversedPacket[i] = sourcePacket[length-(i+1)];
		}
	}

	public byte Calculate(byte[] bytes)
	{
		//byte[] reversedBytes = new byte[bytes.Length];
		//reverseEndianness (bytes, reversedBytes);
		int crc = 0;
		
		for(int index = 0; index< bytes.Length; index++)
		{
			crc =  (crc ^ bytes[index]) & 255;
			//crc =  (crc ^ reversedBytes[index]) & 255;
			for (int loop=0; loop<8; loop++)
			{
				if ((crc & 1) == 1)
					crc = ((crc >> 1) ^ _crc8Poly);
				else
					crc = (crc >> 1);
				//Debug.Log ("CRC Interior Step Result: " + crc + "\n");
			}
			crc = crc & 255;
			//Debug.Log ("CRC Step Result: " + crc + "\n");
		}
		
		return (byte) crc;
	}
}
