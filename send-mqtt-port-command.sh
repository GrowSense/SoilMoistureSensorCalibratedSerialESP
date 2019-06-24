MQTT_PORT=$1
SERIAL_PORT=$2

echo "Sending MQTT port name as a command to device..."

if [ ! $MQTT_PORT ]; then
  echo "Please provide an MQTT port as an argument."
  exit 1
fi

if [ ! $SERIAL_PORT ]; then
  echo "Please provide a serial port as an argument."
  exit 1
fi

echo "  MQTT port: $MQTT_PORT"
echo "  Device port: $SERIAL_PORT"

sh send-serial-command.sh "MPort:$MQTT_PORT" $SERIAL_PORT

echo "Finished sending MQTT port command"
