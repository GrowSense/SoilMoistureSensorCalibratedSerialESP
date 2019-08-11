using System;
using NUnit.Framework;

namespace SoilMoistureSensorCalibratedSerialESP.Tests.Integration
{
    public class CalibrateToCurrentCommandTestHelper : GreenSenseHardwareTestHelper
    {
        public string Label;
        public string Key;
        public int SimulatedSoilMoisturePercentage = -1;
        public int RawSoilMoistureValue = 0;

        public CalibrateToCurrentCommandTestHelper ()
        {
        }

        public void TestCalibrateCommand ()
        {
            WriteTitleText ("Starting calibrate " + Label + " command test");


            Console.WriteLine ("Simulated soil moisture: " + SimulatedSoilMoisturePercentage + "%");

            if (RawSoilMoistureValue == 0)
                RawSoilMoistureValue = SimulatedSoilMoisturePercentage * AnalogPinMaxValue / 100;

            Console.WriteLine ("Raw soil moisture value: " + RawSoilMoistureValue);
            Console.WriteLine ("");

            ConnectDevices ();

            SimulateSoilMoisture (SimulatedSoilMoisturePercentage);

            // Skip the first X entries to give the value time to stabilise
            WaitForData (3);

            var dataEntry = WaitForDataEntry ();

            AssertDataValueIsWithinRange (dataEntry, "R", RawSoilMoistureValue, RawValueMarginOfError);

            SendCalibrationCommand ();
        }

        public void SendCalibrationCommand ()
        {
            var command = Key;

            SendDeviceCommand (command);

            // Skip the first entries to give the value time to stabilise
            WaitForData (3);

            var dataEntry = WaitForDataEntry ();

            Assert.IsTrue (dataEntry.ContainsKey (Key), "Data entry doesn't contain " + Label + " '" + Key + "' key/value.");

            var value = Convert.ToInt32 (dataEntry [Key]);

            AssertIsWithinRange (Label, value, ApplyOffset (RawSoilMoistureValue, ExpectedRawValueOffset), RawValueMarginOfError);
        }
    }
}

