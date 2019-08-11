using System;
using System.Threading;
using NUnit.Framework;
using System.IO;

namespace SoilMoistureSensorCalibratedSerialESP.Tests.Integration
{
    public class GreenSenseHardwareTestHelper : HardwareTestHelper
    {
        public int SoilMoistureSimulatorPin = 9;
        public int SoilMoistureSimulatorPowerPin = 3;

        public int DelayAfterTurningSoilMoistureSensorOn = 3;

        // The margin of error is higher in the ESP8266 version of the test than the arduino version because of the
        // conversion from 5v (of the soil moisture simulator) to 3.3v (of the ESP8266 board) in the test system
        public int RawValueMarginOfError = 45;
        public int CalibratedValueMarginOfError = 4;
        public double TimeErrorMargin = 0.4;

        // Offset to take into account voltage drop via the simulated soil moisture sensor readings
        public int ExpectedRawValueOffset = 10;
        // Offset to take into account voltage drop via the simulated soil moisture sensor readings
        public int ExpectedCalibratedValueOffset = 2;
      
        public bool CalibrationIsReversedByDefault = true;

        public bool RequiresResetSettings = true;

        public GreenSenseHardwareTestHelper ()
        {
        }

        #region Enable Devices Functions

        public override void ConnectDevices (bool enableSimulator)
        {
            Console.WriteLine ("Connecting devices...");

            base.ConnectDevices (enableSimulator);

            PrepareDeviceForTest ();
        }

        #endregion

        #region Prepare Device Functions

        public virtual void PrepareDeviceForTest ()
        {
            PrepareDeviceForTest (true);
        }

        public virtual void PrepareDeviceForTest (bool consoleWriteDeviceOutput)
        {
            Console.WriteLine ("Preparing device for test...");

            if (RequiresResetSettings) {
                ResetDeviceSettings ();

                SetDeviceReadInterval (1);

                SetDryCalibrationValue (AnalogPinMaxValue);

                SetWetCalibrationValue (0);

                if (CalibrationIsReversedByDefault)
                    ReverseDeviceCalibration ();
            }

            if (consoleWriteDeviceOutput)
                ReadFromDeviceAndOutputToConsole ();

            if (!String.IsNullOrEmpty (TextToWaitForBeforeTest))
                WaitForText (TextToWaitForBeforeTest);
        }

        #endregion

        #region General Device Command Settings

        public void SendDeviceCommand (string command)
        {
            WriteToDevice (command);

            WaitForMessageReceived (command);
        }

        public void WaitForMessageReceived (string message)
        {
            Console.WriteLine ("");
            Console.WriteLine ("Waiting for message: " + message);

            var output = String.Empty;
            var wasMessageReceived = false;

            var startTime = DateTime.Now;

            while (!wasMessageReceived) {
                output += ReadLineFromDevice ();

                var expectedText = "Received message: " + message;
                if (output.Contains (expectedText)) {
                    wasMessageReceived = true;

                    //Console.WriteLine ("  Message was received");

                    //ConsoleWriteSerialOutput (output);
                }

                var hasTimedOut = DateTime.Now.Subtract (startTime).TotalSeconds > TimeoutWaitingForResponse;
                if (hasTimedOut && !wasMessageReceived) {
                    ConsoleWriteSerialOutput (output);

                    Assert.Fail ("Timed out waiting for message received (" + TimeoutWaitingForResponse + " seconds)");
                }
            }
        }

        #endregion

        #region Specific Device Command Functions

        public void ResetDeviceSettings ()
        {
            var cmd = "X";

            Console.WriteLine ("");
            Console.WriteLine ("Resetting device default settings...");
            Console.WriteLine ("  Sending '" + cmd + "' command to device");
            Console.WriteLine ("");

            SendDeviceCommand (cmd);
        }

        public void SetDeviceReadInterval (int numberOfSeconds)
        {
            var cmd = "I" + numberOfSeconds;

            Console.WriteLine ("");
            Console.WriteLine ("Setting device read interval to " + numberOfSeconds + " second(s)...");
            Console.WriteLine ("  Sending '" + cmd + "' command to device");
            Console.WriteLine ("");

            SendDeviceCommand (cmd);
        }

        public void SetDryCalibrationValue (int value)
        {
            var cmd = "D" + value;

            Console.WriteLine ("");
            Console.WriteLine ("Setting dry calibration value to: " + value);
            Console.WriteLine ("  Sending '" + cmd + "' command to device");
            Console.WriteLine ("");

            SendDeviceCommand (cmd);
        }

        public void SetWetCalibrationValue (int value)
        {
            var cmd = "W" + value;

            Console.WriteLine ("");
            Console.WriteLine ("Setting wet calibration value to: " + value);
            Console.WriteLine ("  Sending '" + cmd + "' command to device");
            Console.WriteLine ("");

            SendDeviceCommand (cmd);
        }

        public void ReverseDeviceCalibration ()
        {
            var cmd = "R";

            Console.WriteLine ("");
            Console.WriteLine ("Reversing device calibration settings...");
            Console.WriteLine ("  Sending '" + cmd + "' command to device");
            Console.WriteLine ("");

            SendDeviceCommand (cmd);
        }

        #endregion

        #region Soil Moisture Simulator Functions

        public void SimulateSoilMoisture (int soilMoisturePercentage)
        {
            Console.WriteLine ("");
            Console.WriteLine ("Simulating soil moisture percentage");
            Console.WriteLine ("  Sending analog percentage");
            Console.WriteLine ("    PWM pin: " + SoilMoistureSimulatorPin);
            Console.WriteLine ("    Soil Moisture Percentage: " + soilMoisturePercentage + "%");
            Console.WriteLine ("");

            SimulatorClient.AnalogWritePercentage (SoilMoistureSimulatorPin, soilMoisturePercentage);
        }

        #endregion

        #region Wait for Pin Functions

        public int WaitUntilSoilMoistureSensorPowerPinIs (bool expectedValue)
        {
            return WaitUntilSimulatorPinIs ("soil moisture sensor power", SoilMoistureSimulatorPowerPin, expectedValue);
        }

        public double WaitWhileSoilMoistureSensorPowerPinIs (bool expectedValue)
        {
            return WaitWhileSimulatorPinIs ("soil moisture sensor power", SoilMoistureSimulatorPowerPin, expectedValue);
        }

        #endregion

        #region Assert Value Functions

        public override void AssertDataValueIsWithinRange (System.Collections.Generic.Dictionary<string, string> dataEntry, string dataKey, int expectedValue, int allowableMarginOfError)
        {
            if (dataKey == "C")
                expectedValue = ApplyOffset (expectedValue, ExpectedCalibratedValueOffset);

            if (dataKey == "R")
                expectedValue = ApplyOffset (expectedValue, ExpectedRawValueOffset);

            base.AssertDataValueIsWithinRange (dataEntry, dataKey, expectedValue, allowableMarginOfError);
        }

        #endregion

        #region Assert Simulator Pin Functions

        public void AssertSoilMoistureSensorPowerPinForDuration (bool expectedValue, int durationInSeconds)
        {
            AssertSimulatorPinForDuration ("soil moisture sensor power", SoilMoistureSimulatorPowerPin, expectedValue, durationInSeconds);
        }

        #endregion

        #region Apply Offset Functions

        public int ApplyOffset (int value, int offset)
        {
            Console.WriteLine ("Applying offset...");
            Console.WriteLine ("  Value: " + value);
            Console.WriteLine ("  Offset: " + offset);
            var newValue = value + offset;

            if (newValue < 0)
                newValue = 0;

            Console.WriteLine ("  New value: " + newValue);
            Console.WriteLine ("Finished applying offset.");

            return newValue;
        }

        #endregion
    }
}
