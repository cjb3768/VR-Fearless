using UnityEngine;
using System;
using System.Collections;

/**
 * Packet class to handle communication of all possible data from a summary packet
 * Extends DataPacket
 * NOTE: Class uses Zephyr Bioharness 3 with the following version information. You may need to change this class as versions change.
 * Boot Software: v1.3.1.0
 * App Software: v1.4.5.0
 */

public class SummaryPacket : DataPacket {

	//payload information
	float batteryVoltage;

	//booleans for valid data

	bool validActivity;
	bool validPeakAcceleration;
	bool validBreathingWaveAmplitude;
	bool validBreathingWaveNoise;
	bool validBreathingRateConfidence;
	bool validEcgAmplitude;
	bool validEcgNoise;
	bool validHeartRateConfidence;
	bool validHeartRateVariability;
	bool validSystemConfidence;
	bool validRog;
	bool validAccelerationMin;
	bool validAccelerationPeak;
	bool validEstimatedCoreTemperature;
	bool validAuxiliaryAdcChannel1;
	bool validAuxiliaryAdcChannel2;
	bool validAuxiliaryAdcChannel3;
	bool validBatteryVoltage;
	bool validBatteryLevel;
	bool validStatusInfo;
	bool validLinkQuality;
	bool validRssi;
	bool validTxPower;

	public SummaryPacket(byte _mid, byte _pls, byte[] _pl, byte _crc, byte _tb) : base (_mid, _pls, _pl, _crc, _tb){
		//setSummaryInfo ();
	}

	/**
	 * Goes through the payload and stores relevant data in packet member variables
	 * TO-DO: Finish setting values
	 */
	public void setSummaryInfo(){
		//byte [] summaryPayload = new byte[base.getPayloadSize ()];
		//Array.Copy (base.getPayload (), base.getPayload(), base.getPayloadSize ());

		//set batteryVoltage
		batteryVoltage = (float) (base.getPayload() [25] + (256 * base.getPayload() [26]))/1000;
		//check for valid batteryVoltage
		if (batteryVoltage == 65.535) {
			//invalid batteryVoltage, set validBatteryVoltage to false
			validBatteryVoltage = false;
		} 
		else {
			validBatteryVoltage = true;
		}

		//set batteryLevel

		//set breathingWaveAmplitude

		//set breathingWaveNoise

		//set breathingRateConfidence

		//set ecgAmplitude

		//set ecgNoise

		//set rog
		//set verticalAxisAccelerationMin
		//set verticalAxisAccelerationPeak
		//set lateralAxisAccelerationMin
		//set lateralAxisAccelerationPeak
		//set sagittalAxisAccelerationMin
		//set sagittalAxisAccelerationPeak
		//set deviceInternalTemp
		//set statusInfo
		//set linkQuality
		//set rssi
		//set txPower
		//set estimatedCoreTemperature
		//set auxiliaryAdcChannel1
		//set auxiliaryAdcChannel2
		//set auxiliaryAdcChannel3
	}

	public byte getSequenceNumber(){
		return base.getPayload() [0];
	}

	public int getTimestampYear(){
		return (base.getPayload() [1] 
		        + (256 * base.getPayload() [2])); 
	}
	
	public int getTimestampMonth(){
		return base.getPayload() [3];
	}

	public int getTimestampDay(){
		return base.getPayload() [4];
	}

	public long getTimestampMilliseconds(){
		return (base.getPayload () [5] 
		        + (256 * base.getPayload () [6]) 
		        + (256 * 256 * base.getPayload () [7]) 
		        + (256 * 256 * 256 * base.getPayload () [8]));
	}

	public byte getVersionNumber(){
		return base.getPayload() [9];
	}
	
	public int getHeartRate(){
	//public short getHeartRate(){
		//return BitConverter.ToInt16 (base.getPayload (), 13);
		return (base.getPayload () [10] 
		        + (256 * base.getPayload() [11]));
	}

	public float getRespirationRate(){
		return (float) (base.getPayload() [12] 
		                 + (256 * base.getPayload() [13])) / 10f;
	}

	public float getSkinTemperature(){
		return (float)(base.getPayload () [14] 
		                + (256 * base.getPayload () [15])) / 10f;
	}

	public int getPosture(){
		return (base.getPayload () [16] 
		        + (256 * base.getPayload () [17]));
	}
	
	public float getActivity(){
		return (float) (base.getPayload() [18] 
		                + (256 * base.getPayload() [19])) / 100f;
	}

	public float getPeakAcceleration(){
		return (float) (base.getPayload() [20] 
		                + (256 * base.getPayload() [21])) / 100f;
	}

	public float getBatteryVoltage(){
		return (float) (base.getPayload() [22] 
		                + (256 * base.getPayload() [23])) / 1000f;
	}

	public byte getBatteryLevel(){
		//fix later
		return 0;
	}

	public int getBreathingWaveAmplitude(){
		return (base.getPayload() [25] 
		         + (256 * base.getPayload() [26]));
	}

	public int breathingWaveNoise(){
		return (base.getPayload() [27] 
		        + (256 * base.getPayload() [28]));
	}

	public byte getBreathingRateConfidence(){
		return (base.getPayload() [29] );
	}
	/*
	int ecgAmplitude;
	int ecgNoise;
	*/
	public byte getHeartRateConfidence(){
		return base.getPayload() [34];
	}

	public int getHeartRateVariability(){
		return (base.getPayload() [35] 
		        + (256 * base.getPayload() [36]));
	}

	public byte getSystemConfidence(){
		return base.getPayload() [37];
	}

	public int getGsr(){
		return (base.getPayload() [38] 
		        + (256 * base.getPayload() [39]));
	}
	/*
	int rog;
	int verticalAxisAccelerationMin;
	int verticalAxisAccelerationPeak;
	int lateralAxisAccelerationMin;
	int lateralAxisAccelerationPeak;
	int sagittalAxisAccelerationMin;
	int sagittalAxisAccelerationPeak;
	int deviceInternalTemp;
	int statusInfo;
	byte linkQuality;
	byte rssi;
	byte txPower;
	int estimatedCoreTemperature;
	int auxiliaryAdcChannel1;
	int auxiliaryAdcChannel2;
	int auxiliaryAdcChannel3;
	*/

	public bool getValidHeartRate(){
		if (getHeartRate() == 65535) {
			//invalid heartRate, return false
			return false;
		} 
		else {
			return true;
		}
	}

	public bool getValidRespirationRate(){
		if (getRespirationRate() == 6553.5) {
			//invalid respirationRate, return false
			return false;
		} 
		else {
			return true;
		}
	}

	public bool getValidSkinTemperature(){
		if (getSkinTemperature() == -3276.8) {
			//invalid skinTemperature, return false
			return false;
		} 
		else {
			return true;
		}
	}

	public bool getValidPosture(){
		if (getPosture() == -32768) {
			//invalid posture, return false
			return false;
		} 
		else {
			return true;
		}
	}

	public bool getValidActivity(){
		if (getActivity() == 655.35) {
			//invalid activity, return false
			return false;
		} 
		else {
			return true;
		}
	}

	public bool getValidPeakAcceleration(){
			if (getPeakAcceleration() == 655.35) {
				//invalid peakAcceleration, return false
				return false;
			} 
			else {
				return true;
			}
	}

	public bool getValidBreathingWaveAmplitude(){
		return validBreathingWaveAmplitude;
	}

	public bool getValidBreathingWaveNoise(){
		return validBreathingWaveNoise;
	}

	public bool getValidBreathingRateConfidence(){
		return validBreathingRateConfidence;
	}

	public bool getValidEcgAmplitude(){
		return validEcgAmplitude;
	}

	public bool getValidEcgNoise(){
		return validEcgNoise;
	}

	public bool getValidHeartRateConfidence(){
		if (getHeartRateConfidence() == 255) {
			//invalid heartRateConfidence, return false
			return false;
		}
		else{
			return true;
		}
	}
	
	public bool getValidHeartRateVariability(){
		if (getHeartRateVariability() == 65535) {
			//invalid heartRateVariability, return false
			return false;
		} 
		else {
			return true;
		}
	}

	public bool getValidSystemConfidence(){
		if (getSystemConfidence() == 255) {
			//invalid systemConfidence, return false
			return false;
		}
		else{
			return true;
		}
	}

	public bool getValidGsr(){
		if (getGsr() == 65535) {
			//invalid gsr, return false
			return false;
		}
		else {
			return true;
		}
	}

	public bool getValidRog(){
		return validRog;
	}

	public bool getValidAccelerationMin(){
		return validAccelerationMin;
	}

	public bool getValidAccelerationPeak(){
		return validAccelerationPeak;
	}

	public bool getValidEstimatedCoreTemperature(){
		return validEstimatedCoreTemperature;
	}

	public bool getValidAuxiliaryAdcChannel1(){
		return validAuxiliaryAdcChannel1;
	}

	public bool getValidAuxiliaryAdcChannel2(){
		return validAuxiliaryAdcChannel2;
	}

	public bool getValidAuxiliaryAdcChannel3(){
		return validAuxiliaryAdcChannel3;
	}

	public bool getValidBatteryVoltage(){
		return validBatteryVoltage;
	}

	public bool getValidBatteryLevel(){
		return validBatteryLevel;
	}

	public bool getValidStatusInfo(){
		return validStatusInfo;
	}

	public bool getValidLinkQuality(){
		return validLinkQuality;
	}

	public bool getValidRssi(){
		return validRssi;
	}

	public bool getValidTxPower(){
		return validTxPower;
	}
}
