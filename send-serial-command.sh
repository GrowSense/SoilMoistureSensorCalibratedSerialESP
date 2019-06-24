COMMAND=$1
SERIAL_PORT=$2

echo "Sending command to device via serial..."

if [ ! $COMMAND ]; then
  echo "Please provide a command as an argument."
  exit 1
fi

if [ ! $SERIAL_PORT ]; then
  echo "Please provide a serial port as an argument."
  exit 1
fi

echo "  Command: $COMMAND"
echo "  Device port: $SERIAL_PORT"

#stty -F $SERIAL_PORT 9600 cs8 cread clocal

#echo "WN:$WIFI_NETWORK" > $SERIAL_PORT

exec 3<> $SERIAL_PORT

#sleep 2

echo "$COMMAND" >&3

#sleep 2

echo ""
echo "--- Start Output from Device"

cat <&3


echo "--- End Output from Device"
echo ""

exec 3>&-

echo "Finished sending command to device via serial"
