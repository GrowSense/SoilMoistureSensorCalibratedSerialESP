#!/bin/bash

DEVICE_PORT=$1
SIMULATOR_PORT=$2

if [ ! $DEVICE_PORT ]; then
  PORT_NAME=$IRRIGATOR_ESP_PORT
fi

if [ ! $DEVICE_PORT ]; then
  echo "Provide a device port as an argument."
  exit 1
fi

if [ ! $DEVICE_PORT ]; then
  PORT_NAME=$IRRIGATOR_ESP_PORT
fi

if [ ! $SIMULATOR_PORT ]; then
  echo "Provide a simulator port as an argument."
  exit 1
fi

echo "Uploading device to: $DEVICE_PORT"
echo "Uploading simulator to: $SIMULATOR_PORT"

bash generate-device-name.sh && \
sh inject-device-name.sh && \

sh upload.sh $DEVICE_PORT && \

sh upload-simulator.sh $SIMULATOR_PORT && \

echo "Done"
