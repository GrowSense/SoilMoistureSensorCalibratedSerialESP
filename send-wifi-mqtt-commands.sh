SERIAL_PORT=$1

echo "Sending WiFi and MQTT settings to device as serial commands..."

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
MQTT_DEVICE_NAME=$(cat mqtt-device-name.security)

echo "  WiFi Network: $WIFI_NAME"
echo "  WiFi Password: [hidden]"
echo ""
echo "  MQTT Host: $MQTT_HOST"
echo "  MQTT Username: $MQTT_USERNAME"
echo "  MQTT Password: [hidden]"
echo "  MQTT Port: $MQTT_PORT"
echo ""
echo "  Device port: $SERIAL_PORT"
echo ""

exec 3<> $SERIAL_PORT

echo "  Sending..."
echo "    WiFi Network"
echo "WN:$WIFI_NAME\n" >&3
echo "    WiFi Password"
echo "WPass:$WIFI_PASSWORD\n" >&3
echo "    MQTT Host"
echo "MHost:$MQTT_HOST\n" >&3
echo "    MQTT Username"
echo "MUser:$MQTT_USERNAME\n" >&3
echo "    MQTT Password"
echo "MPass:$MQTT_PASSWORD\n" >&3
echo "    MQTT Port"
echo "MPort:$MQTT_PORT\n" >&3
echo "    MQTT Device name"
echo "MDevice:$MQTT_DEVICE_NAME\n" >&3

echo ""

exec 3>&-

echo "Finished sending WiFi and MQTT settings to device as serial commands"