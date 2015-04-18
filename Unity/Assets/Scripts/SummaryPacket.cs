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

	int currentScene;

	//constructor
	public SummaryPacket(byte _mid, byte _pls, byte[] _pl, byte _crc, byte _tb, int _scene) : base (_mid, _pls, _pl, _crc, _tb) {
		currentScene = _scene; //used to determine what scene we were in when packet was received
	}

	public int getCurrentScene(){
		return currentScene;
	}

	//------------------getter methods for info from payload--------------------

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
		return base.getPayload() [24];
	}

	public int getBreathingWaveAmplitude(){
		return (base.getPayload() [25] 
		    	+ (256 * base.getPayload() [26]));
	}

	public int getBreathingWaveNoise(){
		return (base.getPayload() [27] 
		    	+ (256 * base.getPayload() [28]));
	}

	public byte getBreathingRateConfidence(){
		return (base.getPayload() [29] );
	}

	public float getEcgAmplitude(){
		return (float)(base.getPayload () [30]
				+ (256 * base.getPayload () [31])) / 100000f;
	}

	public float getEcgNoise(){
		return (float)(base.getPayload () [32]
				+ (256 * base.getPayload () [33])) / 100000f;

	}

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

	/**
	 * Note: this gives the numeric value of the ROG bytes, not a simple red, orange, or green
	 */
	public int getRog(){
		return (base.getPayload() [40] 
		        + (256 * base.getPayload() [41]));
	}


	public float getVerticalAxisAccelerationMin(){
		return (base.getPayload() [42] 
		    	+ (256 * base.getPayload() [43]))/100f;
	}

	public float getVerticalAxisAccelerationPeak(){
		return (base.getPayload () [44] 
				+ (256 * base.getPayload () [45])) / 100f;
	}

	public float getLateralAxisAccelerationMin(){
		return (base.getPayload () [46] 
				+ (256 * base.getPayload () [47])) / 100f;
	}

	public float getLateralAxisAccelerationPeak(){
		return (base.getPayload () [48] 
				+ (256 * base.getPayload () [49])) / 100f;
	}

	public float getSagittalAxisAccelerationMin(){
		return (base.getPayload () [50] 
				+ (256 * base.getPayload () [51])) / 100f;
	}

	public float getSagittalAxisAccelerationPeak(){
		return (base.getPayload () [52] 
				+ (256 * base.getPayload () [53])) / 100f;
	}

	public float getDeviceInternalTemp(){
		return (base.getPayload () [54] 
		        + (256 * base.getPayload () [55])) / 10f;
	}

	/**
	 * Note: this gives the numeric value of the status info bytes, not a detailed log of status information
	 */
	public int getStatusInfo(){
		return (base.getPayload () [56] 
				+ (256 * base.getPayload () [57]));
	}

	public byte getLinkQuality(){
		return base.getPayload () [58];
	}
	
	public byte getRssi(){
		return base.getPayload () [59];
	}

	public byte getTxPower(){
		return base.getPayload () [60];
	}

	public float getEstimatedCoreTemperature(){
		return (base.getPayload () [61] 
		        + (256 * base.getPayload () [62]));
	}

	public int getAuxiliaryAdcChannel1(){
		return (base.getPayload () [63] 
		        + (256 * base.getPayload () [64]));
	}

	public int getAuxiliaryAdcChannel2(){
		return (base.getPayload () [65] 
		        + (256 * base.getPayload () [66]));
	}

	public int getAuxiliaryAdcChannel3(){
		return (base.getPayload () [67] 
		        + (256 * base.getPayload () [68]));
	}
	
	//------------------getter methods for payload info validity checking--------------------

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
		if (getBreathingWaveAmplitude() == 65535) {
			//invalid breathingWaveAmplitude, return false
			return false;
		} 
		else {
			return true;
		}
	}

	public bool getValidBreathingWaveNoise(){
		if (getBreathingWaveNoise () == 65535) {
			//invalid breathingWaveNoise, return false
			return false;
		} 
		else {
			return true;
		}
	}

	public bool getValidBreathingRateConfidence(){
		if (getBreathingRateConfidence() == 255) {
			//invalid breathingRateConfidence, return false
			return false;
		}
		else{
			return true;
		}
	}

	public bool getValidEcgAmplitude(){
		if (getEcgAmplitude() == 0.065535) {
			//invalid ecgAmplitude, return false
			return false;
		} 
		else {
			return true;
		}
	}

	public bool getValidEcgNoise(){
		if (getEcgNoise() == 0.065535) {
			//invalid ecgNoise, return false
			return false;
		} 
		else {
			return true;
		}
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
		if (getRog() == 0) {
			//invalid rog, return false
			return false;
		}
		else {
			return true;
		}
	}

	public bool getValidVerticalAxisAccelerationMin(){
		if (getVerticalAxisAccelerationMin () == -327.68) {
			//invalid verticalAxisAccelerationMin, return false
			return false;
		}
		else {
			return true;
		}
	}

	public bool getValidVerticalAxisAccelerationPeak(){
		if (getVerticalAxisAccelerationPeak () == -327.68) {
			//invalid verticalAxisAccelerationPeak, return false
			return false;
		}
		else {
			return true;
		}
	}

	public bool getValidLateralAxisAccelerationMin(){
		if (getLateralAxisAccelerationMin () == -327.68) {
			//invalid lateralAxisAccelerationMin, return false
			return false;
		}
		else {
			return true;
		}
	}
	
	public bool getValidLateralAxisAccelerationPeak(){
		if (getLateralAxisAccelerationPeak () == -327.68) {
			//invalid lateralAxisAccelerationPeak, return false
			return false;
		}
		else {
			return true;
		}
	}

	public bool getValidSagittalAxisAccelerationMin(){
		if (getSagittalAxisAccelerationMin () == -327.68) {
			//invalid sagittalAxisAccelerationMin, return false
			return false;
		}
		else {
			return true;
		}
	}
	
	public bool getValidSagittalAxisAccelerationPeak(){
		if (getSagittalAxisAccelerationPeak () == -327.68) {
			//invalid sagittalAxisAccelerationPeak, return false
			return false;
		}
		else {
			return true;
		}
	}

	public bool getValidEstimatedCoreTemperature(){
		if (getEstimatedCoreTemperature() == 6553.5) {
			//invalid estimatedCoreTemperature, return false
			return false;
		}
		else {
			return true;
		}
	}

	public bool getValidAuxiliaryAdcChannel1(){
		if (getAuxiliaryAdcChannel1 () == 65535) {
			//invalid auxiliaryAdcChannel1, return false
			return false;
		} 
		else {
			return true;
		}
	}

	public bool getValidAuxiliaryAdcChannel2(){
		if (getAuxiliaryAdcChannel2 () == 65535) {
			//invalid auxiliaryAdcChannel2, return false
			return false;
		} 
		else {
			return true;
		}
	}

	public bool getValidAuxiliaryAdcChannel3(){
		if (getAuxiliaryAdcChannel3 () == 65535) {
			//invalid auxiliaryAdcChannel3, return false
			return false;
		} 
		else {
			return true;
		}
	}

	public bool getValidBatteryVoltage(){
		if (getBatteryVoltage() == 65.535) {
			//invalid batteryVoltage, return false
			return false;
		} 
		else {
			return true;
		}
	}

	public bool getValidBatteryLevel(){
		if (getBatteryLevel() == 255) {
			//invalid batteryLevel, return false
			return false;
		} 
		else {
			return true;
		}
	}

	public bool getValidStatusInfo(){
		if (getStatusInfo() == 0) {
			//invalid statusInfo, return false
			return false;
		} 
		else {
			return true;
		}
	}

	public bool getValidLinkQuality(){
		if (getLinkQuality() == 255) {
			//invalid linkQuality, return false
			return false;
		} 
		else {
			return true;
		}
	}

	public bool getValidRssi(){
		if (getRssi() == -128) {
			//invalid rssi, return false
			return false;
		} 
		else {
			return true;
		}
	}

	public bool getValidTxPower(){
		if (getTxPower() == -128) {
			//invalid txPower, return false
			return false;
		} 
		else {
			return true;
		}
	}
}
