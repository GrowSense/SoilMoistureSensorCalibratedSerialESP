echo ""
echo "Uploading simulator sketch"
echo ""

PORT_NAME=$MONITOR_ESP_SIMULATOR_PORT

if [ ! $PORT_NAME ]; then
  PORT_NAME="/dev/ttyUSB1"
fi

echo "Port: $PORT_NAME"

# Specify a temporary directory name
SIMULATOR_TMP_DIR="_simulatortmp"

# Remove old versions
rm -rf $SIMULATOR_TMP_DIR

# Make a new directory
mkdir -p $SIMULATOR_TMP_DIR
cd $SIMULATOR_TMP_DIR

echo "Preparing to clone ArduinoSerialController"
echo "Dir:"
echo "  $PWD"

# Clone the latest version
git clone https://github.com/CompulsiveCoder/ArduinoSerialController.git

cd ArduinoSerialController

# Upload
sh upload-to-port.sh $PORT_NAME

# Remove the temporary directory
rm -rf $SIMULATOR_TMP_DIR
