#include "Arduino.h"
#include "duinocom2.h"

bool verboseCom = true;

bool isMsgReady = false;
int msgPosition = 0;
char msgBuffer[MAX_MSG_LENGTH];
char keyBuffer[MAX_KEY_LENGTH];
char valueBuffer[MAX_VALUE_LENGTH];
int msgLength = 0;

// Check whether a message is available and add it to the 'msgBuffer' buffer
bool checkMsgReady()
{
  while (Serial.available() > 0 && !isMsgReady) {
    //if (verboseCom)
    //{
    //  Serial.println("Reading serial...");
    //}
    byte b = Serial.read();

    // The end of a message
    if ((b == ';'
      || b == '\n'
      || b == '\r')
      && msgPosition > 0
      )
    {
      //if (verboseCom)
      //{
      //  Serial.print("In:");
      //  if (b == '\n'
      //    || b == '\r')
      //    Serial.println("[newline]");
      //  else
      //    Serial.println(char(b));
      //}

      isMsgReady = true;
      
      for (int i = msgPosition; i < MAX_MSG_LENGTH; i++)
        msgBuffer[i] = '\0';
        
      msgPosition = 0;

      //if (verboseCom)
      //{
      //  Serial.println("Message ready");

      //  Serial.print("Length:");
      //   Serial.println(msgLength);
      //}
    }
    else if (byte(b) == '\n' // New line
      || byte(b) == '\r') // Carriage return
    {
      //if (verboseCom)
      //  Serial.println("[newline]");
    }
    else // Message bytes
    {
      if (msgPosition == 0)
        clearMsg(msgBuffer);

      msgBuffer[msgPosition] = b;
      msgLength = msgPosition+1;
      msgPosition++;
      isMsgReady = false;

      //if (verboseCom)
      //{
      //  Serial.print("In:");
      //  Serial.println(char(b));
      //}
    }

    delay(1);
  }

  return isMsgReady;
}

// Get the message from the 'msgBuffer' buffer
char* getMsg()
{
  // Reset the isMsgReady flag until a new message is received
  isMsgReady = false;

  if (verboseCom)
   printMsg(msgBuffer);

  return msgBuffer;
}

int getMsgLength()
{
  return msgLength;
}

void printMsg(char msg[MAX_MSG_LENGTH])
{
  if (msgLength > 0)
  {
    Serial.print("Message: ");
    for (int i = 0; i < MAX_MSG_LENGTH; i++)
    {
      if (msg[i] != '\0')
        Serial.print(char(msg[i]));
    }
    Serial.println();
  }
}

void clearMsg(char msgBuffer[MAX_MSG_LENGTH])
{
  for (int i = 0; i < 10; i++)
  {
    msgBuffer[i] = '\0';
  }
}

char getCmdChar(char msg[MAX_MSG_LENGTH], int characterPosition)
{
  return msg[characterPosition];
}

void readSubstring(char msg[MAX_MSG_LENGTH], int startPosition, int length, char* output)
{
  /*if (verboseCom)
  {
    Serial.println("Reading substring");
    Serial.print("  Start position: ");
    Serial.println(startPosition);
    Serial.print("  Length: ");
    Serial.println(length);
  }*/

  for (int i = 0; i < length; i++)
  {
    output[i] = msg[startPosition+i];

    //if (verboseCom)
    //  Serial.println(output[i]);
    
    if (i == length-1)
      output[i+1] = '\0';
  }
  
  /*if (verboseCom)
  {
    Serial.print("  Output: ");
    Serial.println(output);
  }*/
}

bool isKeyValue(char msg[MAX_MSG_LENGTH])
{
  return indexOf(msg, ':') > 0;
}

char* getKey(char msg[MAX_MSG_LENGTH])
{
  /*if (verboseCom)
  {
    Serial.println("Getting key...");
  }*/

  int colonPosition = indexOf(msg, ':');

  /*if (verboseCom)
  {
    Serial.print("  Colon position: ");
    Serial.println(colonPosition);
  }*/

  int keyLength = colonPosition;

  /*if (verboseCom)
  {
    Serial.print("  Key length: ");
    Serial.println(keyLength);
  }*/

  int keyEndPosition = colonPosition;

  /*if (verboseCom)
  {
    Serial.print("  Key end position: ");
    Serial.println(keyEndPosition);
  }*/
  
  readSubstring(msg, 0, keyEndPosition, keyBuffer);
  
  /*if (verboseCom)
  {
    Serial.print("  Key: ");
    Serial.println(keyBuffer);
  }*/
  
  return keyBuffer;
}

char* getValue(char msg[MAX_MSG_LENGTH])
{
  /*if (verboseCom)
  {
    Serial.println("Getting value");
  }*/

  int colonPosition = indexOf(msg, ':');

  /*if (verboseCom)
  {
    Serial.print("  Colon position: ");
    Serial.println(colonPosition);
  }*/

  int valueLength = strlen(msg)-colonPosition-1;

  /*if (verboseCom)
  {
    Serial.print("  Value length: ");
    Serial.println(valueLength);
  }*/

  int valueStartPosition = colonPosition+1;

  /*if (verboseCom)
  {
    Serial.print("  Value start position: ");
    Serial.println(valueStartPosition);
  }*/
  
  //int valueLength = indexOf(msg, '\n')-valueStartPosition;
  
  /*if (verboseCom)
  {
    Serial.print("  Value length: ");
    Serial.println(valueLength);
  }*/
  
  readSubstring(msg, valueStartPosition, valueLength, valueBuffer);
  
  /*if (verboseCom)
  {
    Serial.print("  Value: ");
    Serial.println(valueBuffer);
  }*/
  
  return valueBuffer;
}

int indexOf(char* msg, char c)
{
  /*if (verboseCom)
  {
    Serial.print("Getting character position from: ");
    Serial.println(msg);
    Serial.print("Character: ");
    Serial.println(c);
  }*/

  int position = 0;

  unsigned int numElements = strlen(msg);
  unsigned int i;
  for(i = 0; i < numElements; ++i) {
      if (msg[i] == c) {
          position = i;
          break;
      }
  }

  return position;
}
