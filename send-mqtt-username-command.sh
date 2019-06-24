MQTT_USERNAME=$1
SERIAL_PORT=$2

echo "Sending MQTT username name as a command to device..."

if [ ! $MQTT_USERNAME ]; then
  echo "Please provide an MQTT username as an argument."
  exit 1
fi

if [ ! $SERIAL_PORT ]; then
  echo "Please provide a serial port as an argument."
  exit 1
fi

echo "  MQTT username: $MQTT_USERNAME"
echo "  Device port: $SERIAL_PORT"

sh send-serial-command.sh "MUser:$MQTT_USERNAME" $SERIAL_PORT

echo "Finished sending MQTT username command"
