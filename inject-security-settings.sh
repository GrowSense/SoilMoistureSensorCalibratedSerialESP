WIFI_NAME=$(cat wifi-name.security)
WIFI_PASSWORD=$(cat wifi-password.security)
MQTT_HOST=$(cat mqtt-host.security)
MQTT_USERNAME=$(cat mqtt-username.security)
MQTT_PASSWORD=$(cat mqtt-password.security)
MQTT_PORT=$(cat mqtt-port.security)

echo "Injecting security settings..."

echo "WiFi Name: $WIFI_NAME"
echo "MQTT Host: $MQTT_HOST"
echo "MQTT Username: $MQTT_USERNAME"
echo "MQTT Port: $MQTT_PORT"

SOURCE_FILE="src/SoilMoistureSensorCalibratedSerialESP/SoilMoistureSensorCalibratedSerialESP.ino"

sed -i "s/#define WIFI_NAME .*/#define WIFI_NAME \"$WIFI_NAME\"/" $SOURCE_FILE
sed -i "s/#define WIFI_PASSWORD .*/#define WIFI_PASSWORD \"$WIFI_PASSWORD\"/" $SOURCE_FILE
sed -i "s/#define MQTT_HOST .*/#define MQTT_HOST \"$MQTT_HOST\"/" $SOURCE_FILE
sed -i "s/#define MQTT_USERNAME .*/#define MQTT_USERNAME \"$MQTT_USERNAME\"/" $SOURCE_FILE
sed -i "s/#define MQTT_PASSWORD .*/#define MQTT_PASSWORD \"$MQTT_PASSWORD\"/" $SOURCE_FILE
