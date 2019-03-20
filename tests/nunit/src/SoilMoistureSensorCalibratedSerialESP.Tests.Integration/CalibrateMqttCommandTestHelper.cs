using System;
using System.Threading;

namespace SoilMoistureSensorCalibratedSerialESP.Tests.Integration
{
    public class CalibrateMqttCommandTestHelper : GreenSenseMqttHardwareTestHelper
    {
        public string Label;
        public string Letter;
        public int RawSoilMoistureValue = 0;

        public void TestCalibrateCommand ()
        {
            WriteTitleText ("Starting calibrate " + Label + " command test");

            Console.WriteLine ("Raw soil moisture value: " + RawSoilMoistureValue);
            Console.WriteLine ("");

            EnableMqtt ();

            ConnectDevices (false);

            SendMqttCalibrationCommand ();
        }

        public void SendMqttCalibrationCommand ()
        {
            Mqtt.SendCommand (Letter, RawSoilMoistureValue);

            // Skip some data
            WaitForData (5);

            var dataEntry = WaitForDataEntry ();

            // If using the soil moisture simulator then the value needs to be within a specified range
            if (SimulatorIsEnabled)
                AssertDataValueIsWithinRange (dataEntry, Letter, RawSoilMoistureValue, RawValueMarginOfError);
            else // Otherwise it needs to be exact
                AssertDataValueEquals (dataEntry, Letter, RawSoilMoistureValue);
        }
    }
}