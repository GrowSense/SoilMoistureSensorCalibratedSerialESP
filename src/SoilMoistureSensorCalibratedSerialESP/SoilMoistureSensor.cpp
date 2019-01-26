#include <Arduino.h>
#include <EEPROM.h>

#include <duinocom.h>

#include "Common.h"
#include "SoilMoistureSensor.h"

#define soilMoistureSensorPin A0
#define soilMoistureSensorPowerPin 12

bool soilMoistureSensorIsOn = true;
long lastSensorOnTime = 0;
int delayAfterTurningSoilMoistureSensorOn = 3 * 1000;

bool soilMoistureSensorReadingHasBeenTaken = false;
long soilMoistureSensorReadingIntervalInSeconds = 5;
long lastSoilMoistureSensorReadingTime = 0; // Milliseconds

int soilMoistureLevelCalibrated = 0;
int soilMoistureLevelRaw = 0;

bool reverseSoilMoistureSensor = false;
//int drySoilMoistureCalibrationValue = ANALOG_MAX;
int drySoilMoistureCalibrationValue = (reverseSoilMoistureSensor ? 0 : ANALOG_MAX);
//int wetSoilMoistureCalibrationValue = 0;
int wetSoilMoistureCalibrationValue = (reverseSoilMoistureSensor ? ANALOG_MAX : 0);

#define soilMoistureSensorIsCalibratedFlagAddress 1
#define drySoilMoistureCalibrationValueAddress 2
#define wetSoilMoistureCalibrationValueAddress 6

#define soilMoistureSensorReadIntervalIsSetFlagAddress 10
#define soilMoistureSensorReadingIntervalAddress 13

/* Setup */
void setupSoilMoistureSensor()
{
  setupCalibrationValues();

  setupSoilMoistureSensorReadingInterval();

  pinMode(soilMoistureSensorPowerPin, OUTPUT);

  // If the interval is less than specified delay then turn the sensor on and leave it on (otherwise it will be turned on each time it's needed)
  if (secondsToMilliseconds(soilMoistureSensorReadingIntervalInSeconds) <= delayAfterTurningSoilMoistureSensorOn)
  {
    turnSoilMoistureSensorOn();
  }
}

/* Sensor On/Off */
void turnSoilMoistureSensorOn()
{
  if (isDebugMode)
    Serial.println("Turning sensor on");

  digitalWrite(soilMoistureSensorPowerPin, HIGH);

  lastSensorOnTime = millis();

  soilMoistureSensorIsOn = true;
}

void turnSoilMoistureSensorOff()
{
  if (isDebugMode)
    Serial.println("Turning sensor off");

  digitalWrite(soilMoistureSensorPowerPin, LOW);

  soilMoistureSensorIsOn = false;
}

/* Sensor Readings */
void takeSoilMoistureSensorReading()
{
  bool sensorReadingIsDue = lastSoilMoistureSensorReadingTime + secondsToMilliseconds(soilMoistureSensorReadingIntervalInSeconds) < millis()
    || lastSoilMoistureSensorReadingTime == 0;

  if (sensorReadingIsDue)
  {
    if (isDebugMode)
      Serial.println("Sensor reading is due");

    bool sensorGetsTurnedOff = secondsToMilliseconds(soilMoistureSensorReadingIntervalInSeconds) > delayAfterTurningSoilMoistureSensorOn;
  
    bool sensorIsOffAndNeedsToBeTurnedOn = !soilMoistureSensorIsOn && sensorGetsTurnedOff;
  
    bool postSensorOnDelayHasPast = lastSensorOnTime + delayAfterTurningSoilMoistureSensorOn < millis();
  
    bool soilMoistureSensorIsOnAndReady = soilMoistureSensorIsOn && (postSensorOnDelayHasPast || !sensorGetsTurnedOff);

    bool soilMoistureSensorIsOnButSettling = soilMoistureSensorIsOn && !postSensorOnDelayHasPast && sensorGetsTurnedOff;

    if (isDebugMode)
    {
        Serial.print("  Sensor is on: ");
        Serial.println(soilMoistureSensorIsOn);
        
        Serial.print("  Last sensor on time: ");
        Serial.print(millisecondsToSecondsWithDecimal(millis() - lastSensorOnTime));
        Serial.println(" seconds ago");
        
        Serial.print("  Sensor gets turned off: ");
        Serial.println(sensorGetsTurnedOff);
        
        Serial.print("  Sensor is off and needs to be turned on: ");
        Serial.println(sensorIsOffAndNeedsToBeTurnedOn);
        
        Serial.print("  Post sensor on delay has past: ");
        Serial.println(postSensorOnDelayHasPast);
        
        Serial.print("  Sensor is off and needs to be turned on: ");
        Serial.println(sensorIsOffAndNeedsToBeTurnedOn);
        
        Serial.print("  Sensor is on and ready: ");
        Serial.println(soilMoistureSensorIsOnAndReady);
        
        Serial.print("  Sensor is on but settling: ");
        Serial.println(soilMoistureSensorIsOnButSettling);
        
        if (soilMoistureSensorIsOnButSettling)
        {
          Serial.print("    Time remaining to settle: ");
          long timeRemainingToSettle = lastSensorOnTime + delayAfterTurningSoilMoistureSensorOn - millis();
          Serial.print(millisecondsToSecondsWithDecimal(timeRemainingToSettle));
          Serial.println(" seconds");
        }
    }

    if (sensorIsOffAndNeedsToBeTurnedOn)
    {
      turnSoilMoistureSensorOn();
    }
    else if (soilMoistureSensorIsOnButSettling)
    {
      // Skip this loop. Wait for the sensor to settle in before taking a reading.
      if (isDebugMode)
        Serial.println("Soil moisture sensor is settling after being turned on");
    }
    else if (soilMoistureSensorIsOnAndReady)
    {
      if (isDebugMode)
        Serial.println("Preparing to take reading");

      lastSoilMoistureSensorReadingTime = millis();
      
      // Remove the delay (after turning soil moisture sensor on) from the last reading time to get more accurate timing
      if (sensorGetsTurnedOff)
        lastSoilMoistureSensorReadingTime = lastSoilMoistureSensorReadingTime - delayAfterTurningSoilMoistureSensorOn;

      soilMoistureLevelRaw = getAverageSoilMoistureSensorReading();

      soilMoistureLevelCalibrated = calculateSoilMoistureLevel(soilMoistureLevelRaw);

      if (soilMoistureLevelCalibrated < 0)
        soilMoistureLevelCalibrated = 0;

      if (soilMoistureLevelCalibrated > 100)
        soilMoistureLevelCalibrated = 100;

      soilMoistureSensorReadingHasBeenTaken = true;

      if (secondsToMilliseconds(soilMoistureSensorReadingIntervalInSeconds) > delayAfterTurningSoilMoistureSensorOn)
      {
        turnSoilMoistureSensorOff();
      }
    }
  }
  else
  {
    if (isDebugMode)
    {
      Serial.println("Sensor reading is not due");
      
      Serial.print("  Last soil moisture sensor reading time: ");
      Serial.print(millisecondsToSecondsWithDecimal(lastSoilMoistureSensorReadingTime));
      Serial.println(" seconds");
      
      Serial.print("  Last soil moisture sensor reading interval: ");
      Serial.print(soilMoistureSensorReadingIntervalInSeconds);
      Serial.println(" seconds");
    
      int timeLeftUntilNextReading = lastSoilMoistureSensorReadingTime + secondsToMilliseconds(soilMoistureSensorReadingIntervalInSeconds) - millis();
      Serial.print("  Time left until next soil moisture sensor reading: ");
      Serial.print(millisecondsToSecondsWithDecimal(timeLeftUntilNextReading));
      Serial.println(" seconds");
    }
  }
}

double getAverageSoilMoistureSensorReading()
{
  int readingSum  = 0;
  int totalReadings = 10;

  for (int i = 0; i < totalReadings; i++)
  {
    int reading = analogRead(soilMoistureSensorPin);

    readingSum += reading;
  }

  double averageReading = readingSum / totalReadings;

  return averageReading;
}

double calculateSoilMoistureLevel(int soilMoistureSensorReading)
{
  return map(soilMoistureSensorReading, drySoilMoistureCalibrationValue, wetSoilMoistureCalibrationValue, 0, 100);
}

/* Reading interval */
void setupSoilMoistureSensorReadingInterval()
{
  bool eepromIsSet = EEPROM.read(soilMoistureSensorReadIntervalIsSetFlagAddress) == 99;

  if (eepromIsSet)
  {
    if (isDebugMode)
    	Serial.println("EEPROM read interval value has been set. Loading.");

    soilMoistureSensorReadingIntervalInSeconds = getSoilMoistureSensorReadingInterval();
  }
  else
  {
    if (isDebugMode)
      Serial.println("EEPROM read interval value has not been set. Using defaults.");
  }
}

void setSoilMoistureSensorReadingInterval(char* msg)
{
    int value = readInt(msg, 1, strlen(msg)-1);

    setSoilMoistureSensorReadingInterval(value);
}

void setSoilMoistureSensorReadingInterval(long newValue)
{
  if (isDebugMode)
  {
    Serial.print("Set sensor reading interval: ");
    Serial.println(newValue);
  }

  EEPROMWriteLong(soilMoistureSensorReadingIntervalAddress, newValue);

  EEPROM.commit();
    
  setEEPROMSoilMoistureSensorReadingIntervalIsSetFlag();

  soilMoistureSensorReadingIntervalInSeconds = newValue; 

  serialOutputIntervalInSeconds = newValue;
  
  if (secondsToMilliseconds(newValue) <= delayAfterTurningSoilMoistureSensorOn)
    turnSoilMoistureSensorOn();
}

long getSoilMoistureSensorReadingInterval()
{
  long value = EEPROMReadLong(soilMoistureSensorReadingIntervalAddress);

  if (value == 0
      || value == 255)
    return soilMoistureSensorReadingIntervalInSeconds;
  else
  {
    if (isDebugMode)
    {
      Serial.print("Read interval found in EEPROM: ");
      Serial.println(value);
    }

    return value;
  }
}

void setEEPROMSoilMoistureSensorReadingIntervalIsSetFlag()
{
  if (EEPROM.read(soilMoistureSensorReadIntervalIsSetFlagAddress) != 99)
    EEPROM.write(soilMoistureSensorReadIntervalIsSetFlagAddress, 99);
    
  EEPROM.commit();
}

void removeEEPROMSoilMoistureSensorReadingIntervalIsSetFlag()
{
  EEPROM.write(soilMoistureSensorReadIntervalIsSetFlagAddress, 0);
    
  EEPROM.commit();
}

/* Calibration */
void setupCalibrationValues()
{
  bool eepromIsSet = EEPROM.read(soilMoistureSensorIsCalibratedFlagAddress) == 99;

  if (eepromIsSet)
  {
    if (isDebugMode)
    	Serial.println("EEPROM calibration values have been set. Loading.");

    drySoilMoistureCalibrationValue = getDrySoilMoistureCalibrationValue();
    wetSoilMoistureCalibrationValue = getWetSoilMoistureCalibrationValue();
  }
  else
  {
    if (isDebugMode)
      Serial.println("EEPROM calibration values have not been set. Using defaults.");
    
    //setDrySoilMoistureCalibrationValue(drySoilMoistureCalibrationValue);
    //setWetSoilMoistureCalibrationValue(wetSoilMoistureCalibrationValue);
  }
}

void setDrySoilMoistureCalibrationValue(char* msg)
{
  int length = strlen(msg);

  if (length == 1)
    setDrySoilMoistureCalibrationValueToCurrent();
  else
  {
    int value = readInt(msg, 1, length-1);

//    Serial.println("Value:");
//    Serial.println(value);

    setDrySoilMoistureCalibrationValue(value);
  }
}

void setDrySoilMoistureCalibrationValueToCurrent()
{
  lastSoilMoistureSensorReadingTime = 0;
  takeSoilMoistureSensorReading();
  setDrySoilMoistureCalibrationValue(soilMoistureLevelRaw);
}

void setDrySoilMoistureCalibrationValue(int newValue)
{
  if (isDebugMode)
  {
    Serial.print("Setting dry soil moisture sensor calibration value: ");
    Serial.println(newValue);
  }

  drySoilMoistureCalibrationValue = newValue;
  
  EEPROMWriteLong(drySoilMoistureCalibrationValueAddress, newValue); // Must divide by 4 to make it fit in eeprom

  EEPROM.commit();

  setEEPROMIsCalibratedFlag();
}

void setWetSoilMoistureCalibrationValue(char* msg)
{
  int length = strlen(msg);

  if (length == 1)
    setWetSoilMoistureCalibrationValueToCurrent();
  else
  {
    int value = readInt(msg, 1, length-1);

//    Serial.println("Value:");
//    Serial.println(value);

    setWetSoilMoistureCalibrationValue(value);
  }
}

void setWetSoilMoistureCalibrationValueToCurrent()
{
  lastSoilMoistureSensorReadingTime = 0;
  takeSoilMoistureSensorReading();
  setWetSoilMoistureCalibrationValue(soilMoistureLevelRaw);
}

void setWetSoilMoistureCalibrationValue(int newValue)
{
  if (isDebugMode)
  {
    Serial.print("Setting wet soil moisture sensor calibration value: ");
    Serial.println(newValue);
  }

  wetSoilMoistureCalibrationValue = newValue;

  EEPROMWriteLong(wetSoilMoistureCalibrationValueAddress, newValue);
  
  EEPROM.commit();
  
  setEEPROMIsCalibratedFlag();
}

void reverseSoilMoistureCalibrationValues()
{
  if (isDebugMode)
    Serial.println("Reversing soil moisture sensor calibration values");

  int tmpValue = drySoilMoistureCalibrationValue;

  drySoilMoistureCalibrationValue = wetSoilMoistureCalibrationValue;

  wetSoilMoistureCalibrationValue = tmpValue;

  if (EEPROM.read(soilMoistureSensorIsCalibratedFlagAddress) == 99)
  {
    setWetSoilMoistureCalibrationValue(wetSoilMoistureCalibrationValue);
    setDrySoilMoistureCalibrationValue(drySoilMoistureCalibrationValue);
  }
}

int getDrySoilMoistureCalibrationValue()
{
  int value = EEPROMReadLong(drySoilMoistureCalibrationValueAddress);

  if (value < 0
      || value > ANALOG_MAX)
    return drySoilMoistureCalibrationValue;
  else
  {
    int drySoilMoistureSensorValue = value;
  
    if (isDebugMode)
    {
      Serial.print("Dry calibration value found in EEPROM: ");
      Serial.println(drySoilMoistureSensorValue);
    }

    return drySoilMoistureSensorValue;
  }
}

int getWetSoilMoistureCalibrationValue()
{
  int value = EEPROMReadLong(wetSoilMoistureCalibrationValueAddress);

  if (value <= 0
      || value > ANALOG_MAX)
    return wetSoilMoistureCalibrationValue;
  else
  {
    if (isDebugMode)
    {
      Serial.print("Wet calibration value found in EEPROM: ");
      Serial.println(value);
    }
  }

  return value;
}

void setEEPROMIsCalibratedFlag()
{
  if (EEPROM.read(soilMoistureSensorIsCalibratedFlagAddress) != 99)
    EEPROM.write(soilMoistureSensorIsCalibratedFlagAddress, 99);
    
  EEPROM.commit();
}

void removeEEPROMIsCalibratedFlag()
{
    EEPROM.write(soilMoistureSensorIsCalibratedFlagAddress, 0);
    
    EEPROM.commit();
}

void restoreDefaultSoilMoistureSensorSettings()
{
  restoreDefaultCalibrationSettings();
  restoreDefaultSoilMoistureSensorReadingIntervalSettings();
}

void restoreDefaultSoilMoistureSensorReadingIntervalSettings()
{
  removeEEPROMSoilMoistureSensorReadingIntervalIsSetFlag();

  soilMoistureSensorReadingIntervalInSeconds = 5;
  serialOutputIntervalInSeconds = 5;

  setSoilMoistureSensorReadingInterval(soilMoistureSensorReadingIntervalInSeconds);
}

void restoreDefaultCalibrationSettings()
{
  removeEEPROMIsCalibratedFlag();

  drySoilMoistureCalibrationValue = (reverseSoilMoistureSensor ? 0 : ANALOG_MAX);
  wetSoilMoistureCalibrationValue = (reverseSoilMoistureSensor ? ANALOG_MAX : 0);

  setDrySoilMoistureCalibrationValue(drySoilMoistureCalibrationValue);
  setWetSoilMoistureCalibrationValue(wetSoilMoistureCalibrationValue);
}
