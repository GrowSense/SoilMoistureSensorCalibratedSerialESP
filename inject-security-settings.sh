WIFI_NAME=$(cat wifi-name.security)
WIFI_PASSWORD=$(cat wifi-password.security)
MQTT_HOST=$(cat mqtt-host.security)
MQTT_USERNAME=$(cat mqtt-username.security)
MQTT_PASSWORD=$(cat mqtt-password.security)
MQTT_PORT=$(cat mqtt-port.security)

echo ""
echo "Injecting security settings into sketch..."

echo "  WiFi Name: $WIFI_NAME"
echo "  WiFi Password: [hidden]"
echo "  MQTT Host: $MQTT_HOST"
echo "  MQTT Username: $MQTT_USERNAME"
echo "  MQTT Password: [hidden]"
echo "  MQTT Port: $MQTT_PORT"

WIFI_SOURCE_FILE="src/SoilMoistureSensorCalibratedSerialESP/WiFi.cpp"

echo "  WiFi source file:"
echo "    $WIFI_SOURCE_FILE"

sed -i "s/String wifiNetwork = .*/String wifiNetwork = \"$WIFI_NAME\";/" $WIFI_SOURCE_FILE
sed -i "s/String wifiPassword = .*/String wifiPassword = \"$WIFI_PASSWORD\";/" $WIFI_SOURCE_FILE

MQTT_SOURCE_FILE="src/SoilMoistureSensorCalibratedSerialESP/MQTT.cpp"

echo "  MQTT Source file:"
echo "    $MQTT_SOURCE_FILE"

sed -i "s/String mqttHost .*/String mqttHost = \"$MQTT_HOST\";/" $MQTT_SOURCE_FILE
sed -i "s/String mqttUsername .*/String mqttUsername = \"$MQTT_USERNAME\";/" $MQTT_SOURCE_FILE
sed -i "s/String mqttPassword .*/String mqttPassword = \"$MQTT_PASSWORD\";/" $MQTT_SOURCE_FILE
sed -i "s/long mqttPort .*/long mqttPort = $MQTT_PORT;/" $MQTT_SOURCE_FILE

echo "Finished injecting security settings into sketch"
echo ""
