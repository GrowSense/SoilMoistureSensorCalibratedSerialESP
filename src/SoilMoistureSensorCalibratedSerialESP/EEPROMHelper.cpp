#include <Arduino.h>
#include <EEPROM.h>
#include <EEPROMHelper.h>

#include "Common.h"

bool isDebugEEPROMMode = false;

void EEPROMWriteLong(int address, long value)
{
  //Decomposition from a long to 4 bytes by using bitshift.
  //One = Most significant -> Four = Least significant byte
  byte four = (value & 0xFF);
  byte three = ((value >> 8) & 0xFF);
  byte two = ((value >> 16) & 0xFF);
  byte one = ((value >> 24) & 0xFF);

  //Write the 4 bytes into the eeprom memory.
  EEPROM.write(address, four);
  EEPROM.write(address + 1, three);
  EEPROM.write(address + 2, two);
  EEPROM.write(address + 3, one);
  EEPROM.write(address + 4, '\0');
  
  EEPROM.commit();
}

void EEPROMWriteLongAndSetFlag(int flagAddress, int address, long value)
{
  EEPROMWriteLong(address, value);
  EEPROMSetFlag(flagAddress);
}

long EEPROMReadLong(int address)
{
  //Read the 4 bytes from the eeprom memory.
  long four = EEPROM.read(address);
  long three = EEPROM.read(address + 1);
  long two = EEPROM.read(address + 2);
  long one = EEPROM.read(address + 3);

  //Return the recomposed long by using bitshift.
  return ((four << 0) & 0xFF) + ((three << 8) & 0xFFFF) + ((two << 16) & 0xFFFFFF) + ((one << 24) & 0xFFFFFFFF);
}

void EEPROMWriteChars(int address, char* data)
{
  //Serial.println("Writing char array to eeprom...");
  //Serial.print("Length:");
  //Serial.println(length);
  int length = strlen(data);
  int i;
  for(i=0;i<length;i++)
  {
    //Serial.println("Writing char to eeprom...");
    //Serial.print("Address:");
    //Serial.println(address+i);
    //Serial.print("Char:");
    //Serial.println(data[i]);
    EEPROM.write(address+i,data[i]);
  }
  EEPROM.write(address+length,'\0');   //Add termination null character for String Data
  EEPROM.commit();
}

void EEPROMWriteCharsAndSetFlag(int flagAddress, int address, char* data)
{
  EEPROMSetFlag(flagAddress);
  EEPROMWriteChars(address, data);
}
 
String EEPROMReadString(int address)
{
  if (isDebugEEPROMMode)
    Serial.println("Reading string from EEPROM...");
    
  int i = 0;
  char k;
  char output[10];
  k=EEPROM.read(address);
  while(k != '\0' && i<500)   //Read until null character
  {    
    k=EEPROM.read(address+i);
    //if (isDebugEEPROMMode)
    //  Serial.println(k);
    output[i]=k;
    i++;
  }
  output[i]='\0';
  
  if (isDebugEEPROMMode)
  {
    Serial.print("  ");
    Serial.println(output);
    Serial.println();
  }
  
  return String(output);
}

String EEPROMReadStringIfSet(int flagAddress, int valueAddress, char* defaultValue)
{
  String output = "";
  if (isDebugEEPROMMode)
  {
    Serial.println("");
    Serial.println("  Reading chars from EEPROM if set...");
    //Serial.print("  Flag address: ");
    //Serial.println(flagAddress);
    //Serial.print("  Value address: ");
    //Serial.println(valueAddress);
  }

  if (EEPROMFlagIsSet(flagAddress))
  {
    if (isDebugEEPROMMode)
      Serial.println("    Flag is set. Reading from EEPROM");
      
    output = EEPROMReadString(valueAddress);
  }
  else
  {
    if (isDebugEEPROMMode)
    {
      Serial.println("    Flag is not set. Using default value");
      output = String(defaultValue);
    }
  }
  
  if (isDebugEEPROMMode)
  {
    Serial.print("    Value: ");
    Serial.println(output);
    Serial.println("  Finished reading chars from EEPROM if set");
    Serial.println();
  }
  
  return output;
}

void EEPROMSetFlag(int address)
{
  if (EEPROM.read(address) != 99)
  {
    EEPROM.write(address, 99);
    
    EEPROM.commit();
  }
}

void EEPROMRemoveFlag(int address)
{
    EEPROM.write(address, 0);
    
    EEPROM.commit();
}

bool EEPROMFlagIsSet(int address)
{
    return EEPROM.read(address) == 99;
}

void EEPROMReset()
{
  for (int i = 0; i < 512; i++)
  {
    if (EEPROM.read(i) != 0)
      EEPROM.write(i, 0);
  }
  
  EEPROM.commit();
}
