#ifndef duinocom_H_
#define duinocom_H_

const int MAX_MSG_LENGTH = 30;
const int MAX_KEY_LENGTH = 10;
const int MAX_VALUE_LENGTH = 20;


bool checkMsgReady();

char* getMsg();

int getMsgLength();

void printMsg(char msg[MAX_MSG_LENGTH]);

void clearMsg(char msg[MAX_MSG_LENGTH]);

void identify();

char getCmdChar(char msg[MAX_MSG_LENGTH], int characterPosition);

bool isKeyValue(char msg[MAX_MSG_LENGTH]);

char* getKey(char msg[MAX_MSG_LENGTH]);

char* getValue(char msg[MAX_MSG_LENGTH]);

int indexOf(char* msg, char c);



#endif
