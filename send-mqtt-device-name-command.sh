MQTT_DEVICE_NAME=$1
SERIAL_PORT=$2

echo "Sending MQTT device name name as a command to device..."

if [ ! $MQTT_DEVICE_NAME ]; then
  echo "Please provide an MQTT device name as an argument."
  exit 1
fi

if [ ! $SERIAL_PORT ]; then
  echo "Please provide a serial port as an argument."
  exit 1
fi

echo "  MQTT device name: $MQTT_DEVICE_NAME"
echo "  Device port: $SERIAL_PORT"

sh send-serial-command.sh "MDevice:$MQTT_DEVICE_NAME" $SERIAL_PORT

echo "Finished sending MQTT device name command"
