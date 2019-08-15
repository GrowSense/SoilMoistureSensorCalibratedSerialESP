#ifndef COMMON_H_
#define COMMON_H_

#define VERSION "1-0-0-1"
#define BOARD_TYPE "esp"

extern const int ANALOG_MAX;

extern unsigned long lastSerialOutputTime; // Milliseconds
extern long serialOutputIntervalInSeconds;

extern bool isDebugMode;

extern unsigned long loopNumber;

void serialPrintLoopHeader();
void serialPrintLoopFooter();

unsigned long secondsToMilliseconds(int seconds);
float millisecondsToSecondsWithDecimal(int milliseconds);

long readLong(char* text, int startPosition, int digitCount);
int readInt(char* text, int startPosition, int digitCount);

#endif
