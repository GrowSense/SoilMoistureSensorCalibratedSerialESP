#ifndef EEPROMHELPER_H_
#define EEPROMHELPER_H_

extern bool isDebugEEPROM;

void EEPROMWriteLong(int address, long value);
void EEPROMWriteLongAndSetFlag(int flagAddress, int address, long value);
long EEPROMReadLong(int address);

void EEPROMWriteChars(int address, char* value); 
void EEPROMWriteCharsAndSetFlag(int flagAddress, int address, char* value); 

String EEPROMReadString(int address);
String EEPROMReadStringIfSet(int flagAddress, int valueAddress, char* defaultValue);

void EEPROMSetFlag(int address);
void EEPROMRemoveFlag(int address);
bool EEPROMFlagIsSet(int address);

void EEPROMReset();

#endif
