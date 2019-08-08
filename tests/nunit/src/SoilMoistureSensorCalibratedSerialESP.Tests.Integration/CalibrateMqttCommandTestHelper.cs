using System;
using System.Threading;

namespace SoilMoistureSensorCalibratedSerialESP.Tests.Integration
{
    public class CalibrateMqttCommandTestHelper : GreenSenseMqttHardwareTestHelper
    {
        public string Label;
        public string Key;
        public int RawSoilMoistureValue = 0;

        public void TestCalibrateCommand ()
        {
            WriteTitleText ("Starting calibrate " + Label + " command test");

            Console.WriteLine ("Raw soil moisture value: " + RawSoilMoistureValue);
            Console.WriteLine ("");

            EnableMqtt ();

            ConnectDevices ();

            SendMqttCalibrationCommand ();
        }

        public void SendMqttCalibrationCommand ()
        {
            Mqtt.Data.Clear ();

            Mqtt.SendCommand (Letter, RawSoilMoistureValue);

            // Skip some data
            WaitForData (1);

            var dataEntry = WaitForDataEntry ();

            AssertDataValueEquals (dataEntry, Key, RawSoilMoistureValue);
        }
    }
}