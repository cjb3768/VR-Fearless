using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;


/**
 * Defines a class to handle Bioharness packet data
 * C# implementation of ZephyrPacket.java, in Bioharness SDK sample android project
 */

public class ZephyrPacket {
	private byte STX = 0x02;
	private byte ETX = 0x03;
	private byte ACK = 0x06;
	private byte NAK = 0x15;
	
	//private ByteArrayOutputStream _buffer;
	private MemoryStream _buffer;
	
	private int CRC8_POLY = 0x8C;
	
	private CRC8 _crc8;

	static private int MINIMUM_LENGTH = 5;
	static private int POS_STX = 0;
	static private int POS_MSG = POS_STX + 1;
	static private int POS_DLC = POS_MSG + 1;
	static private int POS_PAYLOAD = POS_DLC + 1;
	static private byte FALSE = 0;
	static private byte TRUE = 1;
	private int _length;
	
	public ZephyrPacket()
	{
		_length = -1;
		_crc8 = new CRC8(CRC8_POLY);
		//_buffer = new ByteArrayOutputStream();
		_buffer = new MemoryStream();
	}
	
	public List<byte[]> Serialize(byte[] inputStream)
	{
		List<byte[]> packets = new List<byte[]>();
		
		for(int index = 0; index < inputStream.Length; index++)
		{
			if (_length < 0)
			{
				if (inputStream[index] == STX)
				{
					//_buffer.reset();
					_buffer.SetLength (0);
					_length = MINIMUM_LENGTH;
				}
			}
			
			if (_length >= 0)
			{
				if (_buffer.Length <= _length)
					//_buffer.write(inputStream[index]);
					_buffer.WriteByte(inputStream[index]);
				
				if (_buffer.Length == (POS_DLC + 1))
					_length += inputStream[index];
				
				if (_buffer.Length >= _length)
				{
					//packets.Add(_buffer.toByteArray());
					packets.Add(_buffer.ToArray());
					_buffer.SetLength (0);
					_length = -1;
				}
			}
		}
		
		return packets;
	}
	
	//public ZephyrPacketArgs Parse(byte[] packet) throws Exception
	public ZephyrPacketArgs Parse(byte[] packet)
	{
		ZephyrPacketArgs result = null;
		byte crcFailStatus;
		crcFailStatus = FALSE;
		if (packet.Length <= 0)
		{
			throw new Exception("Empty packet.");
		}
		if (packet[POS_STX] != STX)
		{
			throw new Exception("Not a STX.");
		}
		if (packet.Length < MINIMUM_LENGTH)
		{
			throw new Exception("Too short.");
		}
		byte dlc = packet[POS_DLC];
		if (packet.Length < dlc + MINIMUM_LENGTH)
		{
			throw new Exception("Wrong length.");
		}
		byte crc = packet[POS_PAYLOAD + dlc];
		byte[] payload = new byte[dlc];
		Array.Copy(packet, POS_PAYLOAD, payload, 0, dlc);
		if (_crc8.Calculate(payload) != crc)
			//throw new Exception("Wrong CRC.");
			crcFailStatus = TRUE;
		
		byte end = packet[POS_PAYLOAD + dlc + 1];
		if ((end != ACK) && (end != NAK) && (end != ETX))
			throw new Exception("Wrong end.");
		
		// Packet well form
		//result = new ZephyrPacketArgs(packet[POS_MSG], payload, end);
		result = new ZephyrPacketArgs(packet[POS_MSG], payload, end,dlc,crcFailStatus);
		
		return result;
	}
	//This is the link alive packet sent to inform BH that the link is connected
	public byte[] getLifeSignMessage()
	{
		return new byte[] {0x02, 0x23, 0x00, 0x00, 0x03};
	}
	
	public byte[] getSetGeneralPacketMessage(bool activate)
	{
		byte[] message = new byte[] {0x02, 0x14, 0x01, 0x00, 0x00, 0x03};
		
		if (activate == true)
			message[3] = 0x01;
		byte[] crcCalc = new byte[1];
		Array.Copy(message, 3, crcCalc, 0, 1);
		message[4] = _crc8.Calculate(crcCalc);
		
		return message;
	}
	
	public byte[] getSetECGPacketMessage(bool activate)
	{
		byte[] message = new byte[] {0x02, 0x16, 0x01, 0x00, 0x00, 0x03};
		
		if (activate == true)
			message[3] = 0x01; 
		byte[] crcCalc = new byte[1];
		Array.Copy(message, 3, crcCalc, 0, 1);
		message[4] = _crc8.Calculate(crcCalc);
		
		return message;
	}
	
	public byte[] getSetBreathingPacketMessage(bool activate)
	{
		byte[] message = new byte[] {0x02, 0x15, 0x01, 0x00, 0x00, 0x03};
		
		if (activate == true)
			message[3] = 0x01; 
		byte[] crcCalc = new byte[1];
		Array.Copy(message, 3, crcCalc, 0, 1);
		message[4] = _crc8.Calculate(crcCalc);
		
		return message;
	}
	public byte[] getSetRtoRPacketMessage(bool activate)
	{
		byte[] message = new byte[] {0x02, 0x19, 0x01, 0x00, 0x00, 0x03};
		
		if (activate == true)
			message[3] = 0x01; 
		byte[] crcCalc = new byte[1];
		Array.Copy(message, 3, crcCalc, 0, 1);
		message[4] = _crc8.Calculate(crcCalc);
		
		return message;
	}
	public byte[] getSetAccelerometerPacketMessage(bool activate)
	{
		byte[] message = new byte[] {0x02, (byte)0xBC, 0x01, 0x00, 0x00, 0x03};
		
		if (activate == true)
			message[3] = 0x01; 
		byte[] crcCalc = new byte[1];
		Array.Copy(message, 3, crcCalc, 0, 1);
		message[4] = _crc8.Calculate(crcCalc);
		
		return message;
	}
	public byte [] getSetSerialNumberMessage(bool activate)
	{
		byte[] message = new byte[] {0x02, 0x0B, 0x01, 0x00, 0x00, 0x03};
		
		if (activate == true)
			message[3] = 0x01; 
		byte[] crcCalc = new byte[1];
		Array.Copy(message, 3, crcCalc, 0, 1);
		message[4] = _crc8.Calculate(crcCalc);
		
		return message;
	}
	public byte [] getSetSummaryPacketMessage(bool activate)
	{
		byte[] message = new byte[] {0x02, (byte)0xBD , 0x02, 0x00, 0x00,0x00, 0x03};
		
		if (activate == true)
			message[3] = 0x01; 
		byte[] crcCalc = new byte[2];
		//Array.Copy(message, 3, crcCalc, 0, 2);
		Array.Copy(message, 3, crcCalc, 0, 2);
		message[5] = _crc8.Calculate(crcCalc);
		
		return message;
	}
	
	public byte [] getSetEventDataPacketMessage(bool activate)
	{
		byte[] message = new byte[] {0x02, (byte)0x2C , 0x01, 0x00, 0x00, 0x03};
		
		if (activate == true)
			message[3] = 0x01; 
		byte[] crcCalc = new byte[1];
		Array.Copy(message, 3, crcCalc, 0, 1);
		message[4] = _crc8.Calculate(crcCalc);
		
		return message;
	}
	public byte [] getSetLoggingDataMessage(bool activate)
	{
		byte[] message = new byte[] {0x02, (byte)0x4B , 0x00, 0x00, 0x00, 0x03};
		
		if (activate == true)
			message[3] = 0x01; 
		byte[] crcCalc = new byte[1];
		Array.Copy(message, 3, crcCalc, 0, 1);
		message[4] = _crc8.Calculate(crcCalc);
		
		return message;
	}
}

