#ifndef MQTT_H_
#define MQTT_H_

#include <PubSubClient.h>
#include "Common.h"
#include "WiFi.h"
#include "SoilMoistureSensor.h"
#include "Commands.h"

extern PubSubClient client;

extern String mqttHost;
extern String mqttUsername;
extern String mqttPassword;
extern String mqttDeviceName;
//extern char* mqttHost;
//extern char* mqttUsername;
//extern char* mqttPassword;
//extern char* mqttDeviceName;
extern long mqttPort;

extern bool areMqttSettingsLoadedFromEEPROM;

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
void setMqttDeviceName(char* password);

void loadMqttSettingsFromEEPROM();

#endif
