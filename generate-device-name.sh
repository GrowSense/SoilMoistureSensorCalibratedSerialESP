#/bin/bash


DEVICE_NUMBER="$RANDOM"

if [ ! $DEVICE_NUMBER ]; then
  echo "Run this script with 'bash' not 'sh' so the random number generator works."
  exit 1
fi

DEVICE_NAME="ESPMonitor$DEVICE_NUMBER"

echo "Generating new device name: $DEVICE_NAME"

echo $DEVICE_NAME > mqtt-device-name.security

