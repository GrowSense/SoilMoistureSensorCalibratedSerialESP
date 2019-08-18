#ifndef WIFI_H_
#define WIFI_H_

#include <ESP8266WiFi.h>
#include <NTPClient.h>
#include <WiFiUdp.h>

#define NTP_OFFSET   60 * 60      // In seconds
#define NTP_INTERVAL 60 * 1000    // In miliseconds
#define NTP_ADDRESS  "europe.pool.ntp.org"

extern WiFiClient wifiClient;

extern WiFiUDP ntpUDP;

extern NTPClient timeClient;

extern bool isWiFiEnabled;
extern bool isWiFiConnected;
extern bool isWiFiConnecting;

extern String wifiNetwork;
extern String wifiPassword;

void setupWiFi();

void loopWiFi();

void loadWiFiSettingsFromEEPROM();

void setWiFiNetwork(char* networkName);
void setWiFiPassword(char* password);

void disableWiFi();

#endif
