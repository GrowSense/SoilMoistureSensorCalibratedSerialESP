#include <Arduino.h>

#include "WiFi.h"
#include "MQTT.h"
#include "EEPROMHelper.h"

#define MQTT_HOST_EEPROM_FLAG_ADDRESS 140
#define MQTT_HOST_EEPROM_ADDRESS 141

#define MQTT_USERNAME_EEPROM_FLAG_ADDRESS 160
#define MQTT_USERNAME_EEPROM_ADDRESS 161

#define MQTT_PASSWORD_EEPROM_FLAG_ADDRESS 180
#define MQTT_PASSWORD_EEPROM_ADDRESS 181

#define MQTT_PORT_EEPROM_FLAG_ADDRESS 200
#define MQTT_PORT_EEPROM_ADDRESS 201

#define MQTT_DEVICE_NAME_EEPROM_FLAG_ADDRESS 210
#define MQTT_DEVICE_NAME_EEPROM_ADDRESS 211

String mqttHost = "garden";
String mqttUsername = "username";
String mqttPassword = "password";
String mqttDeviceName = "wiFiMonitor1";
long mqttPort = 1883;

int totalSubscribeTopics = 5;
String subscribeTopics[] = {"D", "W", "I", "F", "Q"};

PubSubClient pubSubClient(wifiClient);

bool isMqttEnabled = true;
bool isMqttConnected = false;

bool areMqttSettingsLoadedFromEEPROM = false;

long lastMqttConnectionAttemptTime = 0;
long mqttConnectionAttemptInterval = 15 * 1000;

void setupMqtt()
{
  if (isMqttEnabled)
  {
    loadMqttSettingsFromEEPROM();

    bool isTimeToConnectToMqtt = isWiFiConnected && !isMqttConnected
              && (lastMqttConnectionAttemptTime == 0 || lastMqttConnectionAttemptTime + mqttConnectionAttemptInterval < millis());

    if (isTimeToConnectToMqtt)
    {
      lastMqttConnectionAttemptTime = millis();
        
      Serial.println("Setting up MQTT");
    
      char hostBuffer[20];
    
      mqttHost.toCharArray(hostBuffer, mqttHost.length()+1);
      
      pubSubClient.setServer(hostBuffer, mqttPort);

      pubSubClient.setCallback(mqttCallback);

      if (!pubSubClient.connected()) {
        Serial.println("Connecting to MQTT...");
        Serial.print("  MQTT host: ");
        Serial.println(mqttHost);
        Serial.print("  MQTT port: ");
        Serial.println(mqttPort);
        Serial.print("  MQTT username: ");
        Serial.println(mqttUsername);
        Serial.print("  MQTT password: ");
        Serial.println("[hidden]");
        //Serial.println(mqttPassword); // Disabled to hide the MQTT password
        Serial.print("  Device name: ");
        Serial.println(mqttDeviceName);
     
        char deviceNameBuffer[20];
        char usernameBuffer[20];
        char passwordBuffer[20];
        
        mqttDeviceName.toCharArray(deviceNameBuffer, mqttDeviceName.length()+1);
        mqttUsername.toCharArray(usernameBuffer, mqttUsername.length()+1);
        mqttPassword.toCharArray(passwordBuffer, mqttPassword.length()+1);
      
        if (pubSubClient.connect(deviceNameBuffer, usernameBuffer, passwordBuffer)) {
          Serial.println("  Connected to MQTT");  

          isMqttConnected = true;

          setupMqttSubscriptions();
        } else {
          Serial.print("  Failed to connect to MQTT. State: ");
          Serial.println(pubSubClient.state());
        }
      }
    }
  }
}

void setupMqttSubscriptions()
{
  Serial.println("Setting up MQTT subscriptions...");

  String baseTopic = "/";
  baseTopic += mqttDeviceName;
  baseTopic += "/";

  Serial.print("  Total subscribe topics: ");
  Serial.println(totalSubscribeTopics);
  
  for (int i = 0; i < totalSubscribeTopics; i++)
  {
    String topic = baseTopic + subscribeTopics[i] + "/in";

    Serial.print("  ");
    Serial.println(topic);

    pubSubClient.subscribe(topic.c_str());
  }
  
  Serial.println("Subscribed to MQTT topics");
  Serial.println();
}

void mqttCallback(char* topic, byte* payload, unsigned int length) {
 
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
  prefix += mqttDeviceName;
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
  Serial.print("MQTT Message: ");
  Serial.println(msgString);

  char msg[msgString.length()+1];
  msgString.toCharArray(msg, msgString.length()+1);
  handleCommand(msg);

 
  Serial.println();
  Serial.println("---");
 
}

/* MQTT Publish */
void mqttPublishData()
{
  if (isMqttConnected && soilMoistureSensorReadingHasBeenTaken)
  {
    if (isDebugMode)
      Serial.println("Publishing");
    publishMqttValue("R", soilMoistureLevelRaw);
    publishMqttValue("C", soilMoistureLevelCalibrated);
    publishMqttValue("I", soilMoistureSensorReadingIntervalInSeconds);
    publishMqttValue("D", drySoilMoistureCalibrationValue);
    publishMqttValue("W", wetSoilMoistureCalibrationValue);
    publishMqttValue("V", VERSION);
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

/* Settings */
void loadMqttSettingsFromEEPROM()
{
  if (!areMqttSettingsLoadedFromEEPROM)
  {
    Serial.println("Loading MQTT settings from EEPROM...");
    
    if (isDebugMode)
      Serial.println("  Reading MQTT host from EEPROM");
    
    if (EEPROMFlagIsSet(MQTT_HOST_EEPROM_FLAG_ADDRESS))
    {
      mqttHost = EEPROMReadString(MQTT_HOST_EEPROM_ADDRESS);
      //String host = EEPROMReadString(MQTT_HOST_EEPROM_ADDRESS);
      //host.toCharArray(mqttHost, host.length()+1);
    }
    else
    {
      //if (isDebugMode)
        Serial.println("  MQTT host has not been set to EEPROM. Using default.");
    }
    
    if (isDebugMode)
      Serial.println("  Reading MQTT username from EEPROM");
    if (EEPROMFlagIsSet(MQTT_USERNAME_EEPROM_FLAG_ADDRESS))
    {
      mqttUsername = EEPROMReadString(MQTT_USERNAME_EEPROM_ADDRESS);
    }
    else
    {
      //if (isDebugMode)
        Serial.println("  MQTT username has not been set to EEPROM. Using default.");
    }
    
    if (isDebugMode)
      Serial.println("  Reading MQTT password from EEPROM");
    if (EEPROMFlagIsSet(MQTT_PASSWORD_EEPROM_FLAG_ADDRESS))
    {
      mqttPassword = EEPROMReadString(MQTT_PASSWORD_EEPROM_ADDRESS);
    }
    else
    {
      //if (isDebugMode)
        Serial.println("  MQTT password has not been set to EEPROM. Using default.");
    }
    
    if (isDebugMode)
      Serial.println("  Reading MQTT port from EEPROM");
    if (EEPROMFlagIsSet(MQTT_PORT_EEPROM_FLAG_ADDRESS))
    {
      mqttPort = EEPROMReadLong(MQTT_PORT_EEPROM_ADDRESS);
    }
    else
    {
      //if (isDebugMode)
        Serial.println("  MQTT pasword has not been set to EEPROM. Using default.");
    }
    
    if (isDebugMode)
      Serial.println("  Reading MQTT device name from EEPROM");
    if (EEPROMFlagIsSet(MQTT_DEVICE_NAME_EEPROM_FLAG_ADDRESS))
    {
      mqttDeviceName = EEPROMReadString(MQTT_DEVICE_NAME_EEPROM_ADDRESS);
    }
    else
    {
      if (isDebugMode)
        Serial.println("  MQTT device name has not been set to EEPROM. Using default.");
    }
    
    //if (isDebugMode)
    //{
      Serial.print("  MQTT device name: ");
      Serial.println(mqttDeviceName);
      Serial.print("  MQTT host: ");
      Serial.println(mqttHost);
      Serial.print("  MQTT username: ");
      Serial.println(mqttUsername);
      Serial.print("  MQTT password: ");
      Serial.println(mqttPassword);
      Serial.print("  MQTT port: ");
      Serial.println(mqttPort);
      Serial.println("Finished loading MQTT settings from EEPROM");
      Serial.println();
    //}
    
    areMqttSettingsLoadedFromEEPROM = true;
  }
}

void setMqttHost(char* host)
{
  Serial.print("Setting MQTT host: ");
  Serial.println(host);
  
  mqttHost = host;
  
  EEPROMWriteCharsAndSetFlag(MQTT_HOST_EEPROM_FLAG_ADDRESS, MQTT_HOST_EEPROM_ADDRESS, host);
  
  lastMqttConnectionAttemptTime = 0;

  pubSubClient.disconnect();
  isMqttConnected = false;
}

void setMqttUsername(char* username)
{
  Serial.print("Setting MQTT username: ");
  Serial.println(username);
  
  mqttUsername = username;
  
  EEPROMWriteCharsAndSetFlag(MQTT_USERNAME_EEPROM_FLAG_ADDRESS, MQTT_USERNAME_EEPROM_ADDRESS, username);
  
  lastMqttConnectionAttemptTime = 0;
  
  pubSubClient.disconnect();
  isMqttConnected = false;
}

void setMqttPassword(char* password)
{
  Serial.print("Setting MQTT password: ");
  Serial.println(password);
  
  mqttPassword = password;
  
  EEPROMWriteCharsAndSetFlag(MQTT_PASSWORD_EEPROM_FLAG_ADDRESS, MQTT_PASSWORD_EEPROM_ADDRESS, password);
  
  lastMqttConnectionAttemptTime = 0;
  
  pubSubClient.disconnect();
  isMqttConnected = false;
}

void setMqttDeviceName(char* deviceName)
{
  Serial.print("Setting MQTT device name: ");
  Serial.println(deviceName);
  
  mqttDeviceName = deviceName;
  
  EEPROMWriteCharsAndSetFlag(MQTT_DEVICE_NAME_EEPROM_FLAG_ADDRESS, MQTT_DEVICE_NAME_EEPROM_ADDRESS, deviceName);
  
  lastMqttConnectionAttemptTime = 0;
  
  pubSubClient.disconnect();
  isMqttConnected = false;
}

void setMqttPort(char* port)
{
  Serial.print("Setting MQTT port: ");
  Serial.println(port);
  
  mqttPort = readLong(port, 0, strlen(port));
  
  EEPROMWriteLongAndSetFlag(MQTT_PORT_EEPROM_FLAG_ADDRESS, MQTT_PORT_EEPROM_ADDRESS, mqttPort);
  
  lastMqttConnectionAttemptTime = 0;
  
  pubSubClient.disconnect();
  isMqttConnected = false;
}

/* MQTT Publish */
void publishMqttValue(char* subTopic, char* value)
{
  String topic = "/";
  topic += mqttDeviceName;
  topic += "/";
  topic += subTopic;

  pubSubClient.publish(topic.c_str(), value, true);

}

void publishMqttValue(char* subTopic, String value)
{
  String topic = "/";
  topic += mqttDeviceName;
  topic += "/";
  topic += subTopic;
  
  char valueArray[16];
  value.toCharArray(valueArray, 12);

  pubSubClient.publish(topic.c_str(), valueArray, true);

}

void publishMqttPush(int soilMoistureValue)
{
  String topic = "/push/";
  topic += mqttDeviceName;

  char valueString[16];
  itoa(soilMoistureValue, valueString, 10);
  
  pubSubClient.publish(topic.c_str(), valueString, true);

}

void loopMqtt()
{
  // MQTT setup is attempted each loop until connected, then it's skipped
  setupMqtt();

  if (isWiFiConnected && isMqttConnected)
  {
    if (!pubSubClient.loop())
    {
      Serial.println("MQTT is not connected");
      isMqttConnected = false;
    }
  }
}

void disableMqtt()
{
  lastMqttConnectionAttemptTime = 0;

  pubSubClient.disconnect();
  isMqttConnected = false;
  
  isMqttEnabled = false;
}

void forceMqttOutput()
{
  soilMoistureSensorReadingHasBeenTaken = true;
}
