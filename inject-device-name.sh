DEVICE_NAME="$1"

echo "Injecting device name into sketch..."

if [ ! "$DEVICE_NAME" ]; then
  DEVICE_NAME=$(cat "mqtt-device-name.security")
fi

if [ ! "$DEVICE_NAME" ]; then
  echo "Specify a device name as an argument."
  exit 1
fi

echo "  Device name: $DEVICE_NAME"

SOURCE_FILE="src/SoilMoistureSensorCalibratedSerialESP/MQTT.cpp"

# Inject the device name into the sketch
sed -i "s/String mqttDeviceName .*/String mqttDeviceName \"$DEVICE_NAME\";/" $SOURCE_FILE

# Create the device name security file needed for tests to run
echo $DEVICE_NAME > mqtt-device-name.security

echo "Finished injecting device name into sketch."
echo ""
