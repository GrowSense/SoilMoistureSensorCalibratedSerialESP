using System;

namespace SoilMoistureSensorCalibratedSerialESP.Tests.Integration
{
    public class GreenSenseMqttHardwareTestHelper : GreenSenseHardwareTestHelper
    {
        public MqttTestHelper Mqtt;

        public bool RequireMqttConnection = true;

        public GreenSenseMqttHardwareTestHelper ()
        {
        }

        public void EnableMqtt ()
        {
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
            TextToWaitForBeforeTest = "Connected to MQTT";

            base.PrepareDeviceForTest (consoleWriteDeviceOutput);

        }
    }
}
