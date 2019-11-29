#include <Arduino.h>
#include <EEPROM.h>
#include <stdlib.h>

#include <duinocom2.h>

#include "Common.h"
#include "EEPROMHelper.h"
#include "WiFi.h"
#include "MQTT.h"
#include "Commands.h"
#include "SoilMoistureSensor.h"
#include "SerialOutput.h"
#include "DeviceName.h"

void setup()
{
  Serial.begin(9600);

  EEPROM.begin(512);
    
  Serial.println("Starting WiFi soil moisture monitor");
  
  loadDeviceNameFromEEPROM();
  
  serialPrintDeviceInfo();

  Serial.println("Device started...");
  
  //setupWiFi();
  
  //setupMqtt();

  setupSoilMoistureSensor();

  serialOutputIntervalInSeconds = soilMoistureSensorReadingIntervalInSeconds;
  
  delay(200);
}


void loop()
{
  if (isDebugMode)
    loopNumber++;

  serialPrintLoopHeader();
  
  checkCommand();
  
  loopWiFi();
  
  loopMqtt();

  takeSoilMoistureSensorReading();

  serialPrintData();
  
  mqttPublishData();

  // Reset flag for this loop
  soilMoistureSensorReadingHasBeenTaken = false;

  serialPrintLoopFooter();

  delay(1);
}
