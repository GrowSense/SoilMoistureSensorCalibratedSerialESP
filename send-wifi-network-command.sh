WIFI_NETWORK=$1
SERIAL_PORT=$2

echo "Sending WiFi network name as a command to device..."

if [ ! $WIFI_NETWORK ]; then
  echo "Please provide a WiFi network as an argument."
  exit 1
fi

if [ ! $SERIAL_PORT ]; then
  echo "Please provide a serial port as an argument."
  exit 1
fi

echo "  WiFi network: $WIFI_NETWORK"
echo "  Device port: $SERIAL_PORT"

sh send-serial-command.sh "WN:$WIFI_NETWORK" $SERIAL_PORT

echo "Finished sending WiFi network name command"
