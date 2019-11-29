#ifndef MQTT_H_
#define MQTT_H_

#include <PubSubClient.h>
#include "Common.h"
#include "WiFi.h"
#include "SoilMoistureSensor.h"
#include "Commands.h"

extern PubSubClient mqttClient;

extern String mqttHost;
extern String mqttUsername;
extern String mqttPassword;
extern long mqttPort;

extern bool areMqttSettingsLoadedFromEEPROM;

extern bool isMqttEnabled;
extern bool isMqttConnected;

extern String subscribeTopics[];

void setupMqtt();

void setupMqttSubscriptions();

void loopMqtt();

void mqttCallback(char* topic, byte* payload, unsigned int length);

void mqttPublishData();

void publishMqttValue(char* subTopic, int value);

void publishMqttValue(char* subTopic, char* value);

void publishMqttValue(char* subTopic, String value);

void publishMqttPush(int soilMoistureValue);

void forceMqttOutput();

void setMqttHost(char* host);
void setMqttUsername(char* username);
void setMqttPassword(char* password);
void setMqttPort(char* port);

void loadMqttSettingsFromEEPROM();

void disableMqtt();

void disconnectMqtt();

#endif
