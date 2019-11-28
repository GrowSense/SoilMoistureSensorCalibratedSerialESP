DEVICE_NAME="$1"

echo "Injecting device name into sketch..."

if [ ! "$DEVICE_NAME" ]; then
  DEVICE_NAME=$(cat "device-name.security")
fi

if [ ! "$DEVICE_NAME" ]; then
  echo "Specify a device name as an argument."
  exit 1
fi

echo "  Device name: $DEVICE_NAME"

SOURCE_FILE="src/SoilMoistureSensorCalibratedSerialESP/DeviceName.cpp"

# Inject the device name into the sketch
sed -i "s/String deviceName = .*/String deviceName = \"$DEVICE_NAME\";/" $SOURCE_FILE || exit 1

# Create the device name security file needed for tests to run
echo $DEVICE_NAME > device-name.security

echo "Finished injecting device name into sketch."
echo ""
