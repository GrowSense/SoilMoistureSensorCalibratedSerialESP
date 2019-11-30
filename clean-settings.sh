echo "Cleaning settings..."

WIFI_SOURCE_FILE="src/SoilMoistureSensorCalibratedSerialESP/WiFi.cpp"

echo "  WiFi source file:"
echo "    $WIFI_SOURCE_FILE"

sed -i "s/String wifiNetwork = .*/String wifiNetwork = \"network\";/" $WIFI_SOURCE_FILE || exit 1
sed -i "s/String wifiPassword = .*/String wifiPassword = \"password\";/" $WIFI_SOURCE_FILE || exit 1

MQTT_SOURCE_FILE="src/SoilMoistureSensorCalibratedSerialESP/MQTT.cpp"

echo "  MQTT Source file:"
echo "    $MQTT_SOURCE_FILE"

sed -i "s/String mqttHost .*/String mqttHost = \"garden\";/" $MQTT_SOURCE_FILE || exit 1
sed -i "s/String mqttUsername .*/String mqttUsername = \"username\";/" $MQTT_SOURCE_FILE || exit 1
sed -i "s/String mqttPassword .*/String mqttPassword = \"password\";/" $MQTT_SOURCE_FILE || exit 1
sed -i "s/long mqttPort .*/long mqttPort = 1883;/" $MQTT_SOURCE_FILE || exit 1

sh set-device-name.sh "NewSMMonitorW" || exit 1

echo "Finished cleaning settings."
