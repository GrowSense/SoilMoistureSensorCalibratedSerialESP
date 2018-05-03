#include <Arduino.h>
#include <EEPROM.h>
#include <stdlib.h>

#include <ESP8266WiFi.h>
#include <PubSubClient.h>

#include <duinocom.h>

#include "Common.h"
#include "SoilMoistureSensor.h"


#define SERIAL_MODE_CALIBRATED 1
#define SERIAL_MODE_RAW 2
#define SERIAL_MODE_CSV 3
#define SERIAL_MODE_QUERYSTRING 4

#define VERSION "1-0-0-73"

int serialMode = SERIAL_MODE_CSV;

#define WIFI_NAME "accesspoint"
#define WIFI_PASSWORD "password"

#define MQTT_HOST "garden"
#define MQTT_PORT 1883
#define MQTT_USERNAME "username"
#define MQTT_PASSWORD "password"
#define MQTT_DEVICE_NAME "monitor1"

int totalSubscribeTopics = 3;
String subscribeTopics[] = {"D", "W", "V"};

WiFiClient espClient;
PubSubClient client(espClient);

int loopNumber = 0;



void setup()
{
  
  Serial.begin(115200);
  //Serial.begin(115200);

  if (isDebugMode)
    Serial.println("Starting soil moisture sensor");

  setupWiFi();

  setupSoilMoistureSensor();

  serialOutputInterval = soilMoistureSensorReadingInterval;
}

void setupWiFi()
{
  WiFi.begin(WIFI_NAME, WIFI_PASSWORD);
   
  Serial.print("Connecting to WiFi..");
  while (WiFi.status() != WL_CONNECTED) {
    delay(250);
    Serial.print(".");
  }

  Serial.println();

  Serial.println("Connected to the WiFi network");

  setupMqtt();
}

void setupMqtt()
{
  client.setServer(MQTT_HOST, MQTT_PORT);

  client.setCallback(callback);

  while (!client.connected()) {
    Serial.println("Connecting to MQTT...");
 
    if (client.connect(MQTT_DEVICE_NAME, MQTT_USERNAME, MQTT_PASSWORD )) {
 
      Serial.println("connected");  
 
    } else {
 
      Serial.print("failed with state ");
      Serial.print(client.state());
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

  if (isDebugMode)
  {
    Serial.println("==============================");
    Serial.print("===== Start Loop: ");
    Serial.println(loopNumber);
    Serial.println("==============================");
  }

  loopWiFi();

  checkCommand();

  takeSoilMoistureSensorReading();

  serialPrintData();

  mqttPublishData();

  // Reset flag for this loop
  soilMoistureSensorReadingHasBeenTaken = false;

  if (isDebugMode)
  {
    Serial.println("==============================");
    Serial.print("===== End Loop: ");
    Serial.println(loopNumber);
    Serial.println("==============================");
    Serial.println("");
    Serial.println("");
  }
}

void loopWiFi()
{
  client.loop();
}

/* Commands */
void checkCommand()
{

  if (checkMsgReady())
  {
    char* msg = getMsg();
       
    handleCommand(msg);

    forceSerialOutput();
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
    case 'D':
      setDrySoilMoistureCalibrationValue(msg);
      break;
    case 'W':
      setWetSoilMoistureCalibrationValue(msg);
      break;
    case 'V':
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
    publishMqttValue("V", soilMoistureSensorReadingInterval);
    publishMqttValue("D", drySoilMoistureCalibrationValue);
    publishMqttValue("W", wetSoilMoistureCalibrationValue);
    publishMqttValue("Z", VERSION);
    publishMqttPush(soilMoistureLevelCalibrated);
  }
}

void publishMqttValue(char* subTopic, int value)
{
  char valueString[20];
  dtostrf(value,3,1,valueString);

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

void publishMqttPush(int soilMoistureValue)
{
  String topic = "/push/";
  topic += MQTT_DEVICE_NAME;

  char valueString[20];
  dtostrf(soilMoistureValue,3,1,valueString);

  client.publish(topic.c_str(), valueString);

}

/* Serial Output */
void serialPrintData()
{
  bool isTimeToPrintData = lastSerialOutputTime + secondsToMilliseconds(serialOutputInterval) < millis()
      || lastSerialOutputTime == 0;

  bool isReadyToPrintData = isTimeToPrintData && soilMoistureSensorReadingHasBeenTaken;

  if (isReadyToPrintData)
  {
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
      Serial.print("V:");
      Serial.print(soilMoistureSensorReadingInterval);
      Serial.print(";");
      Serial.print("D:");
      Serial.print(drySoilMoistureCalibrationValue);
      Serial.print(";");
      Serial.print("W:");
      Serial.print(wetSoilMoistureCalibrationValue);
      Serial.print(";");
      Serial.print("Z:");
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
      Serial.print(soilMoistureSensorReadingInterval); // Convert to seconds
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
}
