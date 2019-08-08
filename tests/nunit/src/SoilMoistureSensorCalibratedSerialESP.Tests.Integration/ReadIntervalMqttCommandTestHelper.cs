using System;

namespace SoilMoistureSensorCalibratedSerialESP.Tests.Integration
{
    public class ReadIntervalMqttCommandTestHelper : GreenSenseMqttHardwareTestHelper
    {
        public int ReadInterval = 1;

        public void TestSetReadIntervalCommand ()
        {
            WriteTitleText ("Starting read interval command test");

            Console.WriteLine ("Read interval: " + ReadInterval);
            
            EnableMqtt ();

            ConnectDevices ();

            Mqtt.Data.Clear ();

            // MQTT version
            Mqtt.SendCommand ("I", ReadInterval);

            // Skip the first entry
            WaitForDataEntry ();

            // Wait for the next entry
            var dataEntry = WaitForDataEntry ();

            AssertDataValueEquals (dataEntry, "I", ReadInterval);
        }
    }
}
