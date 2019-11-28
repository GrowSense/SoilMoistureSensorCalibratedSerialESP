#include <Arduino.h>
#include <EEPROM.h>

#include "Common.h"
#include "EEPROMHelper.h"
#include "WiFi.h"

#define WIFI_NETWORK_EEPROM_FLAG_ADDRESS 100
#define WIFI_NETWORK_EEPROM_ADDRESS 101

#define WIFI_PASSWORD_EEPROM_FLAG_ADDRESS 120
#define WIFI_PASSWORD_EEPROM_ADDRESS 121

WiFiClient wifiClient;

WiFiUDP ntpUDP;

NTPClient timeClient(ntpUDP, NTP_ADDRESS, NTP_OFFSET, NTP_INTERVAL);

bool isWiFiEnabled = true;
bool isWiFiConnected = false;
bool isWiFiConnecting = false;

String wifiNetwork = "network";
String wifiPassword = "password";

long wifiStartConnectingTime = 0;
long wifiRetryInterval = 15 * 1000;

bool areWiFiSettingsLoadedFromEEPROM = false;

bool isWiFiConnectionFailureReported = false;

void setupWiFi()
{
  if (isWiFiEnabled)
  {
    if (WiFi.status() != WL_CONNECTED)
      isWiFiConnected = false;

    if (!isWiFiConnected)
    {
      bool isReconnectRetryTime = wifiStartConnectingTime > 0 && hasDelayElapsed(wifiRetryInterval, wifiStartConnectingTime);
      
      bool wifiConnectionFailureReportNeeded = WiFi.status() == WL_CONNECT_FAILED
                                               && !isReconnectRetryTime
                                               && !isWiFiConnectionFailureReported;
          
      if (wifiConnectionFailureReportNeeded)
      {
        Serial.println("Connection to WiFi failed");
        
        isWiFiConnected = false;
        isWiFiConnectionFailureReported = true;
      }
      else if (WiFi.status() == WL_CONNECTED)
      {
        Serial.println("Connected to WiFi");

        isWiFiConnected = true;
        
        timeClient.begin();
      }
      else if (WiFi.status() != WL_CONNECTED)
      {    
        if (isWiFiConnecting && isReconnectRetryTime)
        {
          Serial.println("Failed to connect to WiFi. Retrying...");
        }
      
        if (!isWiFiConnecting || isReconnectRetryTime)
        {
          Serial.println();
          Serial.println("Setting up WiFi...");
          
          loadWiFiSettingsFromEEPROM();
          
          Serial.print("  WiFi network: ");
          Serial.println(wifiNetwork);
          Serial.print("  WiFi password: ");
          Serial.println(wifiPassword);
          
          Serial.println("Connecting to WiFi network...");
            
          char networkBuffer[20];
          char passwordBuffer[20];
          
          wifiNetwork.toCharArray(networkBuffer, wifiNetwork.length()+1); 
          wifiPassword.toCharArray(passwordBuffer, wifiPassword.length()+1);
           
          WiFi.begin(networkBuffer, passwordBuffer);
            
          wifiStartConnectingTime = millis();
            
          isWiFiConnecting = true;    
          isWiFiConnectionFailureReported = false;
        }
      }
    }
  }
}

void loadWiFiSettingsFromEEPROM()
{
  if (!areWiFiSettingsLoadedFromEEPROM)
  {
    if (isDebugMode)
      Serial.println("Loading WiFi settings from EEPROM...");
    
    if (isDebugMode)
      Serial.println("  Reading WiFi network from EEPROM");
      
    if (EEPROMFlagIsSet(WIFI_NETWORK_EEPROM_FLAG_ADDRESS))
    {
      wifiNetwork = EEPROMReadString(WIFI_NETWORK_EEPROM_ADDRESS);
    }
    else
    {
      if (isDebugMode)
        Serial.println("  WiFi network has not been set to EEPROM. Using default.");
    }
    
    if (isDebugMode)
    {
      Serial.print("  WiFi network: ");
      Serial.println(wifiNetwork);
      Serial.println();
    }
    
    if (isDebugMode)
      Serial.println("  Reading WiFi password from EEPROM");
      
    if (EEPROMFlagIsSet(WIFI_PASSWORD_EEPROM_FLAG_ADDRESS))
    {
      wifiPassword = EEPROMReadString(WIFI_PASSWORD_EEPROM_ADDRESS);
    }
    else
    {
      if (isDebugMode)
        Serial.println("  WiFi network has not been set to EEPROM. Using default.");
    }
    
    if (isDebugMode)
    {
      Serial.print("  WiFi password: ");
      Serial.println(wifiPassword);
      Serial.println();
      Serial.println("Finished loading WiFi details from EEPROM");
      Serial.println();
    }
    
    areWiFiSettingsLoadedFromEEPROM = true;
  }
}

void setWiFiNetwork(char* networkName)
{
  Serial.print("Setting WiFi network: ");
  Serial.println(networkName);
  
  wifiNetwork = networkName;
  
  EEPROMWriteCharsAndSetFlag(WIFI_NETWORK_EEPROM_FLAG_ADDRESS, WIFI_NETWORK_EEPROM_ADDRESS, networkName);
  
  isWiFiConnected = false;
  isWiFiConnecting = false;
  wifiStartConnectingTime = 0;
  
  WiFi.disconnect();
}

void setWiFiPassword(char* password)
{
  Serial.print("Setting WiFi password: ");
  Serial.println(password);
  
  wifiPassword = password;
  
  EEPROMWriteCharsAndSetFlag(WIFI_PASSWORD_EEPROM_FLAG_ADDRESS, WIFI_PASSWORD_EEPROM_ADDRESS, password);
  
  isWiFiConnected = false;
  isWiFiConnecting = false;
  wifiStartConnectingTime = 0;
  
  WiFi.disconnect();
}

void loopWiFi()
{
  // MQTT setup is attempted each loop until connected, then it's skipped
  setupWiFi();
  if (isWiFiConnected)
    timeClient.update();
}

void disableWiFi()
{
  isWiFiEnabled = false;
  
  isWiFiConnected = false;
  isWiFiConnecting = false;
  wifiStartConnectingTime = 0;
  
  if (WiFi.status() == WL_CONNECTED)
    WiFi.disconnect();
}

