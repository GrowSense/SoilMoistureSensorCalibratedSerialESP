#!/bin/bash

echo ""
echo "Setting device name..."
echo ""

DEVICE_NAME=$1

if [ "$DEVICE_NAME" ]; then

  echo "Device name: $DEVICE_NAME"
  
  echo $DEVICE_NAME > "device-name.security"

  echo ""
  echo "Finished setting device name"
else
  echo "Please provide device name as an argument"
  exit 1
fi
