#ifndef SOILMOISTURESENSOR_H_
#define SOILMOISTURESENSOR_H_

extern int soilMoistureLevelCalibrated;
extern int soilMoistureLevelRaw;

extern long lastSoilMoistureSensorReadingTime;
extern long soilMoistureSensorReadingInterval;
extern int soilMoistureSensorReadIntervalIsSetFlagAddress;

extern int drySoilMoistureCalibrationValue;
extern int wetSoilMoistureCalibrationValue;

extern bool soilMoistureSensorIsOn;
extern long lastSensorOnTime;
extern int delayAfterTurningSensorOn;
extern bool soilMoistureSensorReadingHasBeenTaken;

void setupSoilMoistureSensor();

void setupCalibrationValues();

void setupSoilMoistureSensorReadingInterval();

void turnSoilMoistureSensorOn();

void turnSoilMoistureSensorOff();

void takeSoilMoistureSensorReading();

double getAverageSoilMoistureSensorReading();

double calculateSoilMoistureLevel(int soilMoistureSensorReading);

void setEEPROMIsCalibratedFlag();

void setSoilMoistureSensorReadingInterval(char* msg);
void setSoilMoistureSensorReadingInterval(long readInterval);

long getSoilMoistureSensorReadingInterval();

void setEEPROMSoilMoistureSensorReadingIntervalIsSetFlag();
void removeEEPROMSoilMoistureSensorReadingIntervalIsSetFlag();

void setDrySoilMoistureCalibrationValue(char* msg);

void setDrySoilMoistureCalibrationValueToCurrent();

void setDrySoilMoistureCalibrationValue(int drySoilMoistureCalibrationValue);

void setWetSoilMoistureCalibrationValue(char* msg);

void setWetSoilMoistureCalibrationValueToCurrent();

void setWetSoilMoistureCalibrationValue(int wetSoilMoistureCalibrationValue);

void reverseSoilMoistureCalibrationValues();

int getDrySoilMoistureCalibrationValue();

int getWetSoilMoistureCalibrationValue();

void setEEPROMIsCalibratedFlag();

void removeEEPROMIsCalibratedFlag();

void restoreDefaultSoilMoistureSensorSettings();
void restoreDefaultSoilMoistureSensorReadingIntervalSettings();
void restoreDefaultCalibrationSettings();
#endif
/* SOILMOISTURESENSOR_H_ */
