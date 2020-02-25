DEVICE_NAME=$1
SERIAL_PORT=$2

echo "Sending WiFi and MQTT settings to device as serial commands..."

if [ ! $DEVICE_NAME ]; then
  echo "Please provide a device name as an argument."
  exit 1
fi

if [ ! $SERIAL_PORT ]; then
  echo "Please provide a serial port as an argument."
  exit 1
fi

WIFI_NAME=$(cat wifi-name.security)
WIFI_PASSWORD=$(cat wifi-password.security)

MQTT_HOST=$(cat mqtt-host.security)
MQTT_USERNAME=$(cat mqtt-username.security)
MQTT_PASSWORD=$(cat mqtt-password.security)
MQTT_PORT=$(cat mqtt-port.security)

echo "$DEVICE_NAME" > device-name.security

echo "  Device Name: $DEVICE_NAME"
echo ""
echo "  WiFi Network: $WIFI_NAME"
echo "  WiFi Password: [hidden]"
echo ""
echo "  MQTT Host: $MQTT_HOST"
echo "  MQTT Username: $MQTT_USERNAME"
echo "  MQTT Password: [hidden]"
echo "  MQTT Port: $MQTT_PORT"
echo ""
echo "  Serial port: $SERIAL_PORT"
echo ""

COMMAND="WN:$WIFI_NAME"
COMMAND="$COMMAND;WPass:$WIFI_PASSWORD"
COMMAND="$COMMAND;MHost:$MQTT_HOST"
COMMAND="$COMMAND;MUser:$MQTT_USERNAME"
COMMAND="$COMMAND;MPass:$MQTT_PASSWORD"
COMMAND="$COMMAND;MPort:$MQTT_PORT"
COMMAND="$COMMAND;Name:$DEVICE_NAME"
COMMAND="$COMMAND;"

echo ""
echo "  Full command:"
echo "    $COMMAND"
echo ""

stty -F $SERIAL_PORT 9600 cs8 -cstopb
sleep 0.1
echo "$COMMAND" > $SERIAL_PORT

echo "Finished sending WiFi and MQTT settings to device as serial commands"
