#ifndef COMMANDS_H_
#define COMMANDS_H_

#include "Common.h"
#include "duinocom2.h"
#include "WiFi.h"
#include "MQTT.h"
#include "EEPROMHelper.h"
#include "SerialOutput.h"
#include "SoilMoistureSensor.h"

void checkCommand();

void handleCommand(char* msg);

void restoreDefaultSettings();

#endif
