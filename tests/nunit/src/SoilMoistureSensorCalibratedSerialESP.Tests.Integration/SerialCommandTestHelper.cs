using System;
using NUnit.Framework;
using System.Threading;

namespace SoilMoistureSensorCalibratedSerialESP.Tests.Integration
{
    public class SerialCommandTestHelper : GrowSenseMqttHardwareTestHelper
    {
        public string Key = "";
        public string Value = "0";
        public string Label = "";
        public bool ValueIsOutputAsData = true;
        public bool ValueIsSavedInEEPROM = true;
        public string ExpectedSerialOutputAfterCommand;
        public bool CheckExpectedSerialOutput = false;
        public bool SeparateKeyValueWithColon = false;

        public SerialCommandTestHelper ()
        {
        }

        public void TestCommand ()
        {
            if (CheckExpectedSerialOutput && String.IsNullOrEmpty (ExpectedSerialOutputAfterCommand))
                ExpectedSerialOutputAfterCommand = Label + ": " + Value;

            WriteTitleText ("Starting " + Label + " command test");

            Console.WriteLine ("Value for " + Label + ": " + Value);
            Console.WriteLine ("");

            ConnectDevices ();

            SendCommand ();

            if (ValueIsSavedInEEPROM)
                ResetAndCheckSettingIsPreserved ();
        }

        public void SendCommand ()
        {
            WriteParagraphTitleText ("Sending " + Label + " command...");

            var command = Key + Value;

            if (SeparateKeyValueWithColon)
                command = Key + ":" + Value;

            SendDeviceCommand (command);

            WriteParagraphTitleText ("Checking " + Label + " value was set...");

            if (ValueIsOutputAsData) {
                var dataEntry = WaitForDataEntry ();

                AssertDataValueEquals (dataEntry, Key, Value);
            }

            if (!String.IsNullOrEmpty (ExpectedSerialOutputAfterCommand))
                WaitForText (ExpectedSerialOutputAfterCommand);
        }

        public void ResetAndCheckSettingIsPreserved ()
        {
            ResetDeviceViaPin ();

            WriteParagraphTitleText ("Checking " + Label + " value is preserved after reset...");

            if (ValueIsOutputAsData) {
                var dataEntry = WaitForDataEntry ();

                AssertDataValueEquals (dataEntry, Key, Value);
            }

            if (!String.IsNullOrEmpty (ExpectedSerialOutputAfterCommand))
                WaitForText (ExpectedSerialOutputAfterCommand);
        }
    }
}

