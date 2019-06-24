WIFI_PASSWORD=$1
SERIAL_PORT=$2

echo "Sending WiFi password as a command to device..."

if [ ! $WIFI_PASSWORD ]; then
  echo "Please provide a WiFi password as an argument."
  exit 1
fi

if [ ! $SERIAL_PORT ]; then
  echo "Please provide a serial port as an argument."
  exit 1
fi

echo "  WiFi password: $WIFI_NETWORK"
echo "  Device port: $SERIAL_PORT"

sh send-serial-command.sh "WPass:$WIFI_PASSWORD" $SERIAL_PORT

echo "Finished sending WiFi network name command"
