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
	
	public byte Calculate(byte[] bytes)
	{
		int crc = 0;
		
		for(int index = 0; index< bytes.length; index++)
		{
			crc =  (crc ^ bytes[index]) & 255;
			for (int loop=0; loop<8; loop++)
			{
				if ((crc & 1) == 1)
					crc = ((crc >> 1) ^ _crc8Poly);
				else
					crc = (crc >> 1);
			}
			crc = crc & 255;
		}
		
		return (byte) crc;
	}
}
