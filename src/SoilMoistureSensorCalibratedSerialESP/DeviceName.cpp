#include <Arduino.h>
#include <EEPROM.h>

#include "DeviceName.h"
#include "Common.h"
#include "EEPROMHelper.h"

#define DEVICE_NAME_EEPROM_FLAG_ADDRESS 210
#define DEVICE_NAME_EEPROM_ADDRESS 211

String deviceName = "NewSMMonitorW";

void setDeviceName(char* newName)
{
  Serial.print("Device name: ");
  Serial.println(newName);
  
  deviceName = newName;
  
  EEPROMWriteCharsAndSetFlag(DEVICE_NAME_EEPROM_FLAG_ADDRESS, DEVICE_NAME_EEPROM_ADDRESS, newName);
}

void loadDeviceNameFromEEPROM()
{
  if (isDebugMode)
    Serial.println("  Reading device name from EEPROM");
	  
  if (EEPROMFlagIsSet(DEVICE_NAME_EEPROM_FLAG_ADDRESS))
  {
    deviceName = EEPROMReadString(DEVICE_NAME_EEPROM_ADDRESS);
  }
  else
  {
    if (isDebugMode)
      Serial.println("  Device name has not been set to EEPROM. Using default.");
  }

  Serial.print("  Device name: ");
  Serial.println(deviceName);
}

