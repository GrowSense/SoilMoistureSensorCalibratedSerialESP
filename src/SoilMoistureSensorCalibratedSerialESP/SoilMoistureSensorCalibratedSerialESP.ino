#include <Arduino.h>
#include <EEPROM.h>
#include <stdlib.h>

#include <ESP8266WiFi.h>
#include <PubSubClient.h>

#include <NTPClient.h>
#include <WiFiUdp.h>

#include <duinocom.h>

#include "Common.h"
#include "SoilMoistureSensor.h"

#define NTP_OFFSET   60 * 60      // In seconds
#define NTP_INTERVAL 60 * 1000    // In miliseconds
#define NTP_ADDRESS  "europe.pool.ntp.org"

WiFiUDP ntpUDP;
NTPClient timeClient(ntpUDP, NTP_ADDRESS, NTP_OFFSET, NTP_INTERVAL);

#define SERIAL_MODE_CALIBRATED 1
#define SERIAL_MODE_RAW 2
#define SERIAL_MODE_CSV 3
#define SERIAL_MODE_QUERYSTRING 4

#define VERSION "1-0-0-1"
#define BOARD_TYPE "esp"

int serialMode = SERIAL_MODE_CSV;

#define WIFI_NAME "accesspoint"
#define WIFI_PASSWORD "password"

#define MQTT_HOST "garden"
#define MQTT_PORT 1883
#define MQTT_USERNAME "username"
#define MQTT_PASSWORD "password"
#define MQTT_DEVICE_NAME "wiFiMonitor1"

int totalSubscribeTopics = 4;
String subscribeTopics[] = {"D", "W", "I", "F"};

WiFiClient espClient;
PubSubClient client(espClient);

void setup()
{
  Serial.begin(9600);

  EEPROM.begin(512);
  
  Serial.println("Starting WiFi soil moisture monitor");
  
  serialPrintDeviceInfo();

  setupWiFi();

  setupSoilMoistureSensor();

  serialOutputIntervalInSeconds = soilMoistureSensorReadingIntervalInSeconds;
  
  timeClient.begin();
}

void setupWiFi()
{
  WiFi.begin(WIFI_NAME, WIFI_PASSWORD);
   
  Serial.print("WiFi Network: ");
  Serial.println(WIFI_NAME);
  Serial.print("Connecting to WiFi...");
  while (WiFi.status() != WL_CONNECTED) {
    delay(250);
    Serial.print(".");
  }

  Serial.println();

  Serial.println("Connected to the WiFi network");

  setupMqtt();
  
  Serial.println("Setup complete!");
}

void setupMqtt()
{
  client.setServer(MQTT_HOST, MQTT_PORT);

  client.setCallback(callback);

  while (!client.connected()) {
    Serial.println("Connecting to MQTT...");
    Serial.print("Host: ");
    Serial.println(MQTT_HOST);
    Serial.print("Port: ");
    Serial.println(MQTT_PORT);
    Serial.print("Device name: ");
    Serial.println(MQTT_DEVICE_NAME);
    Serial.print("MQTT Username: ");
    Serial.println(MQTT_USERNAME);
 
    if (client.connect(MQTT_DEVICE_NAME, MQTT_USERNAME, MQTT_PASSWORD )) {
 
      Serial.println("connected");  
 
    } else {
 
      Serial.print("failed with state ");
      Serial.println(client.state());
      delay(2000);
 
    }
  }

  setupMqttSubscriptions();
}

void setupMqttSubscriptions()
{
  Serial.println("Setting up subscriptions...");

  String baseTopic = "/";
  baseTopic += MQTT_DEVICE_NAME;
  baseTopic += "/";

    //Serial.println(subscribeTopics[0]);
  for (int i = 0; i < totalSubscribeTopics; i++)
  {
    String topic = baseTopic + subscribeTopics[i] + "/in";

    Serial.println(topic);

    client.subscribe(topic.c_str());
  }

  Serial.println();
}

void callback(char* topic, byte* payload, unsigned int length) {
 
  if (isDebugMode)
  {
    Serial.print("Message arrived in topic: ");
    Serial.println(topic);
  }

  String valueString = "";

  if (isDebugMode) 
    Serial.print("Message:");

  for (int i = 0; i < length; i++) {
    valueString += (char)payload[i]; 
    if (isDebugMode)
      Serial.print((char)payload[i]);
  }

  Serial.println();

  String prefix = "/";
  prefix += MQTT_DEVICE_NAME;
  prefix += "/";

  String postFix = "/in";

  String topicString = (String)topic;

  String subTopic = topicString;
  subTopic.replace(prefix, "");
  subTopic.replace(postFix, "");

  if (isDebugMode)
  {
    Serial.println("Subtopic");
    Serial.println(subTopic);
    Serial.println("Value");
    Serial.println(valueString);
  }

  String msgString = subTopic + valueString;
  Serial.println("Msg");
  Serial.println(msgString);

  char msg[msgString.length()+1];
  msgString.toCharArray(msg, msgString.length()+1);
  handleCommand(msg);

 
  Serial.println();
  Serial.println("---");
 
}

void loop()
{
  loopNumber++;

  serialPrintLoopHeader();
  
  timeClient.update();
    
  loopWiFi();

  checkCommand();

  takeSoilMoistureSensorReading();

  serialPrintData();
  
  mqttPublishData();

  // Reset flag for this loop
  soilMoistureSensorReadingHasBeenTaken = false;

  serialPrintLoopFooter();

  delay(1);
}

void loopWiFi()
{
  client.loop();
}

void serialPrintDeviceInfo()
{
  Serial.println("");
  Serial.println("Family: GreenSense");
  Serial.println("Group: monitor");
  Serial.println("Project: SoilMoistureSensorCalibratedSerialESP");
  Serial.print("Board: ");
  Serial.println(BOARD_TYPE);
  Serial.print("Version: ");
  Serial.println(VERSION);
  Serial.println("");
}

/* Commands */
void checkCommand()
{
  if (isDebugMode)
  {
    Serial.println("Checking incoming serial commands");
  }

  if (checkMsgReady())
  {
    char* msg = getMsg();
       
    handleCommand(msg);
  }
  delay(1);
}

void handleCommand(char* msg)
{
  char letter = msg[0];

  int length = strlen(msg);

  Serial.print("Received message: ");
  Serial.println(msg);

  switch (letter)
  {
    case '#':
      serialPrintDeviceInfo();
      break;
    case 'D':
      setDrySoilMoistureCalibrationValue(msg);
      break;
    case 'W':
      setWetSoilMoistureCalibrationValue(msg);
      break;
    case 'I':
      setSoilMoistureSensorReadingInterval(msg);
      break;
    case 'X':
      restoreDefaultSettings();
      break;
    case 'R':
      reverseSoilMoistureCalibrationValues();
      break;
    case 'Z':
      Serial.println("Toggling isDebugMode");
      isDebugMode = !isDebugMode;
      break;
  }
  
  forceSerialOutput();
  forceMqttOutput();
}

/* Settings */
void restoreDefaultSettings()
{
  Serial.println("Restoring default settings");

  restoreDefaultSoilMoistureSensorSettings();
}

/* MQTT Publish */
void mqttPublishData()
{
  if (soilMoistureSensorReadingHasBeenTaken)
  {
    if (isDebugMode)
      Serial.println("Publishing");
    publishMqttValue("R", soilMoistureLevelRaw);
    publishMqttValue("C", soilMoistureLevelCalibrated);
    publishMqttValue("I", soilMoistureSensorReadingIntervalInSeconds);
    publishMqttValue("D", drySoilMoistureCalibrationValue);
    publishMqttValue("W", wetSoilMoistureCalibrationValue);
    publishMqttValue("Z", VERSION);
    publishMqttValue("Time", timeClient.getFormattedTime());
    publishMqttPush(soilMoistureLevelCalibrated);
  }
}

void publishMqttValue(char* subTopic, int value)
{
  char valueString[16];
  itoa(value, valueString, 10);

  publishMqttValue(subTopic, valueString);

}

void publishMqttValue(char* subTopic, char* value)
{
  String topic = "/";
  topic += MQTT_DEVICE_NAME;
  topic += "/";
  topic += subTopic;

  client.publish(topic.c_str(), value);

}

void publishMqttValue(char* subTopic, String value)
{
  String topic = "/";
  topic += MQTT_DEVICE_NAME;
  topic += "/";
  topic += subTopic;
  
  char valueArray[16];
  value.toCharArray(valueArray, 12);

  client.publish(topic.c_str(), valueArray);

}

void publishMqttPush(int soilMoistureValue)
{
  String topic = "/push/";
  topic += MQTT_DEVICE_NAME;

  char valueString[16];
  itoa(soilMoistureValue, valueString, 10);
  
  client.publish(topic.c_str(), valueString);

}

/* Serial Output */
void serialPrintData()
{
  bool isTimeToPrintData = lastSerialOutputTime + secondsToMilliseconds(serialOutputIntervalInSeconds) < millis()
      || lastSerialOutputTime == 0;

  bool isReadyToPrintData = isTimeToPrintData && soilMoistureSensorReadingHasBeenTaken;

  if (isReadyToPrintData)
  {
    if (isDebugMode)
      Serial.println("Ready to serial print data");
  
    long numberOfSecondsOnline = millis()/1000;

    if (serialMode == SERIAL_MODE_CSV)
    {
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
    else if (serialMode == SERIAL_MODE_QUERYSTRING)
    {
      //Serial.print("time=");
      //Serial.print(numberOfSecondsOnline);
      //Serial.print("&");
      Serial.print("raw=");
      Serial.print(soilMoistureLevelRaw);
      Serial.print("&");
      Serial.print("calibrated=");
      Serial.print(soilMoistureLevelCalibrated);
      Serial.print("&");
      Serial.print("readInterval=");
      Serial.print(soilMoistureSensorReadingIntervalInSeconds); // Convert to seconds
      Serial.print("&");
      Serial.print("dry=");
      Serial.print(drySoilMoistureCalibrationValue);
      Serial.print("&");
      Serial.print("wet=");
      Serial.print(wetSoilMoistureCalibrationValue);
      Serial.println();
    }
    else if (serialMode == SERIAL_MODE_CALIBRATED)
    {
      Serial.println(soilMoistureLevelCalibrated);
    }
    else if (serialMode == SERIAL_MODE_RAW)
    {
      Serial.println(soilMoistureLevelRaw);
    }

    lastSerialOutputTime = millis();
  }
  else
  {
    if (isDebugMode)
    {    
      Serial.println("Not ready to serial print data");

      Serial.print("  Is time to serial print data: ");
      Serial.println(isTimeToPrintData);
      if (!isTimeToPrintData)
      {
        Serial.print("    Time remaining before printing data: ");
        Serial.print(millisecondsToSecondsWithDecimal(lastSerialOutputTime + secondsToMilliseconds(serialOutputIntervalInSeconds) - millis()));
        Serial.println(" seconds");
      }
      Serial.print("  Soil moisture sensor reading has been taken: ");
      Serial.println(soilMoistureSensorReadingHasBeenTaken);
      Serial.print("  Is ready to print data: ");
      Serial.println(isReadyToPrintData);

    }
  }
}

void forceMqttOutput()
{
  soilMoistureSensorReadingHasBeenTaken = true;
}
