DEVICE_NAME="$1"

echo "Injecting device name into sketch..."

if [ ! "$DEVICE_NAME" ]; then
  DEVICE_NAME=$(cat "mqtt-device-name.security")
fi

if [ ! "$DEVICE_NAME" ]; then
  echo "Specify a device name as an argument."
  exit 1
fi

echo "Device name: $DEVICE_NAME"

SOURCE_FILE="src/SoilMoistureSensorCalibratedSerialESP/SoilMoistureSensorCalibratedSerialESP.ino"

# Inject the device name into the sketch
sed -i "s/#define MQTT_DEVICE_NAME .*/#define MQTT_DEVICE_NAME \"$DEVICE_NAME\"/" $SOURCE_FILE

# Create the device name security file needed for tests to run
echo $DEVICE_NAME > mqtt-device-name.security
