using System;

namespace SoilMoistureSensorCalibratedSerialESP.Tests.Integration
{
    public class MqttOutputTimeTestHelper : GreenSenseMqttHardwareTestHelper
    {
        public int ReadInterval = 1;

        public void TestMqttOutputTime ()
        {
            WriteTitleText ("Starting MQTT output time test");

            Console.WriteLine ("Read interval: " + ReadInterval);

            ConnectDevices ();

            SetDeviceReadInterval (ReadInterval);

            EnableMqtt ();

            Mqtt.Data.Clear ();

            Console.WriteLine ("Skipping next data entries in case they're out of date...");

            Mqtt.WaitUntilData (2);

            Console.WriteLine ("Waiting for the next data entry...");

            var secondsBetweenData = Mqtt.WaitUntilData (1);

            Console.WriteLine ("Time between data entries: " + secondsBetweenData + " seconds");

            var expectedMqttOutputTime = ReadInterval;

            AssertIsWithinRange ("mqtt output time", expectedMqttOutputTime, secondsBetweenData, TimeErrorMargin);
        }
    }
}
