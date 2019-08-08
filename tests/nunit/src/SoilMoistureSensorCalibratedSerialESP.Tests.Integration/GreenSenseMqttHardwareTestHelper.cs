using System;

namespace SoilMoistureSensorCalibratedSerialESP.Tests.Integration
{
    public class GreenSenseMqttHardwareTestHelper : GreenSenseHardwareTestHelper
    {
        public MqttTestHelper Mqtt;

        public bool RequireMqttConnection = false;

        public GreenSenseMqttHardwareTestHelper ()
        {
        }

        public void EnableMqtt ()
        {
            RequireMqttConnection = true;
            Mqtt = new MqttTestHelper ();
            Mqtt.Start ();
        }

        public void EnableMqtt (string deviceName)
        {
            Mqtt = new MqttTestHelper (deviceName);
            Mqtt.Start ();
        }

        public override void WaitForDevicesToConnect ()
        {
            base.WaitForDevicesToConnect ();
        }

        public override void PrepareDeviceForTest (bool consoleWriteDeviceOutput)
        {
            if (RequireMqttConnection)
                TextToWaitForBeforeTest = "Subscribed to MQTT topics";

            base.PrepareDeviceForTest (consoleWriteDeviceOutput);

        }
    }
}
