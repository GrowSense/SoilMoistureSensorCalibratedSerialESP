#include <Arduino.h>
#include <EEPROM.h>

#include "Common.h"
#include "SoilMoistureSensor.h"
#include "DeviceName.h"

void serialPrintDeviceInfo()
{
  Serial.println("");
  Serial.println("-- Start Device Info");
  Serial.println("Family: GrowSense");
  Serial.println("Group: monitor");
  Serial.println("Project: SoilMoistureSensorCalibratedSerialESP");
  Serial.print("Board: ");
  Serial.println(BOARD_TYPE);
  Serial.print("Device name: ");
  Serial.println(deviceName);
  Serial.print("Version: ");
  Serial.println(VERSION);
  Serial.println("ScriptCode: soil-moisture-monitor");
  Serial.println("-- End Device Info");
  Serial.println("");
}

void serialPrintData()
{
  bool isTimeToPrintData = millis() - lastSerialOutputTime >= secondsToMilliseconds(serialOutputIntervalInSeconds)
      || lastSerialOutputTime == 0;

  bool isReadyToPrintData = isTimeToPrintData && soilMoistureSensorReadingHasBeenTaken;

  if (isReadyToPrintData)
  {
    lastSerialOutputTime = millis();
    
    if (isDebugMode)
      Serial.println("Ready to serial print data");
  
    //long numberOfSecondsOnline = millis()/1000;

    Serial.print("D;"); // This prefix indicates that the line contains data.
    //Serial.print("T:");
    //Serial.print(numberOfSecondsOnline);
    //Serial.print(";");
    Serial.print("R:");
    Serial.print(soilMoistureLevelRaw);
    Serial.print(";");
    Serial.print("C:");
    Serial.print(soilMoistureLevelCalibrated);
    Serial.print(";");
    Serial.print("I:");
    Serial.print(soilMoistureSensorReadingIntervalInSeconds);
    Serial.print(";");
    Serial.print("D:");
    Serial.print(drySoilMoistureCalibrationValue);
    Serial.print(";");
    Serial.print("W:");
    Serial.print(wetSoilMoistureCalibrationValue);
    Serial.print(";");
    Serial.print("V:");
    Serial.print(VERSION);
    Serial.print(";;");
    Serial.println();
  }
  else
  {
    //if (isDebugMode)
    //{    
    //  Serial.println("Not ready to serial print data");

    //  Serial.print("  Is time to serial print data: ");
    //  Serial.println(isTimeToPrintData);
    //  if (!isTimeToPrintData)
    //  {
    //    Serial.print("    Time remaining before printing data: ");
    //    Serial.print(millisecondsToSecondsWithDecimal(lastSerialOutputTime + secondsToMilliseconds(serialOutputIntervalInSeconds) - millis()));
    //    Serial.println(" seconds");
    //  }
    //  Serial.print("  Soil moisture sensor reading has been taken: ");
    //  Serial.println(soilMoistureSensorReadingHasBeenTaken);
    //  Serial.print("  Is ready to print data: ");
    //  Serial.println(isReadyToPrintData);

    //}
  }
}

void forceSerialOutput()
{
    // Reset the last serial output time 
    lastSerialOutputTime = 0;//millis()-secondsToMilliseconds(serialOutputIntervalInSeconds);
}
