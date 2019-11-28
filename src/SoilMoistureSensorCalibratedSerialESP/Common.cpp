#include <Arduino.h>
#include <EEPROM.h>

#include "Common.h"

const int ANALOG_MAX = 1024;

unsigned long lastSerialOutputTime = 0;
long serialOutputIntervalInSeconds = 1;

bool isDebugMode = false;

unsigned long loopNumber = 0;

void serialPrintLoopHeader()
{
  if (isDebugMode)
  {
    Serial.println("==========");
    Serial.print("Start Loop: ");
    Serial.println(loopNumber);
    Serial.print("Time: ");
    Serial.print(millisecondsToSecondsWithDecimal(millis()));
    Serial.println(" seconds");
    Serial.println("");
  }
}

void serialPrintLoopFooter()
{
  if (isDebugMode)
  {
    Serial.print("End Loop: ");
    Serial.println(loopNumber);
    Serial.println("==========");
    Serial.println("");
  }
}

unsigned long secondsToMilliseconds(int seconds)
{
  return seconds * 1000;
}

float millisecondsToSecondsWithDecimal(int milliseconds)
{
  return float(milliseconds) / float(1000);
}


int readInt(char* text, int startPosition, int digitCount)
{
  char buffer[digitCount];

  if (isDebugMode)
    Serial.println("Reading int");

  for (int i = 0; i < digitCount; i++)
  {
    buffer[i] = text[startPosition+i];

    //if (verboseCom)
    //  Serial.println(buffer[i]);
  }

  int number = atoi(buffer);

  return number;
}

long readLong(char* text, int startPosition, int digitCount)
{
  char buffer[digitCount];

  if (isDebugMode)
    Serial.println("Reading long");

  for (int i = 0; i < digitCount; i++)
  {
    buffer[i] = text[startPosition+i];

    //if (verboseCom)
    //  Serial.println(buffer[i]);
  }

  long number = atol(buffer);

  return number;
}

bool hasDelayElapsed(long interval, unsigned long previousTime)
{
  return millis() - previousTime >= interval;
}

