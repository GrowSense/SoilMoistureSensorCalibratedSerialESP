MQTT_HOST=$1
SERIAL_PORT=$2

echo "Sending MQTT host name as a command to device..."

if [ ! $MQTT_HOST ]; then
  echo "Please provide an MQTT host as an argument."
  exit 1
fi

if [ ! $SERIAL_PORT ]; then
  echo "Please provide a serial port as an argument."
  exit 1
fi

echo "  MQTT host: $MQTT_HOST"
echo "  Device port: $SERIAL_PORT"

sh send-serial-command.sh "MHost:$MQTT_HOST" $SERIAL_PORT

echo "Finished sending MQTT host command"
