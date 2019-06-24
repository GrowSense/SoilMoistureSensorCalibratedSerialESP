MQTT_PASSWORD=$1
SERIAL_PORT=$2

echo "Sending MQTT password name as a command to device..."

if [ ! $MQTT_PASSWORD ]; then
  echo "Please provide an MQTT password as an argument."
  exit 1
fi

if [ ! $SERIAL_PORT ]; then
  echo "Please provide a serial port as an argument."
  exit 1
fi

echo "  MQTT password: $MQTT_PASSWORD"
echo "  Device port: $SERIAL_PORT"

sh send-serial-command.sh "MPass:$MQTT_PASSWORD" $SERIAL_PORT

echo "Finished sending MQTT password command"
